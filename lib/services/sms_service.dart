import 'package:smart_auth/smart_auth.dart';

class SmsService {
  static final SmartAuth _smartAuth = SmartAuth();

  /// Yêu cầu quyền đọc SMS
  static Future<bool> requestSmsPermission() async {
    try {
      final res = await _smartAuth.requestHint();
      return res?.id != null;
    } catch (e) {
      print('Error requesting SMS permission: $e');
      return false;
    }
  }

  /// Tự động điền số điện thoại từ SIM
  static Future<String?> getPhoneNumber() async {
    try {
      final res = await _smartAuth.requestHint();
      return res?.id; // Trả về số điện thoại từ SIM
    } catch (e) {
      print('Error getting phone number: $e');
      return null;
    }
  }

  /// Lắng nghe SMS OTP (auto-fill)
  /// Trả về mã OTP khi nhận được SMS
  static Future<String?> listenForOtp() async {
    try {
      // Lấy App Signature (cần thiết cho SMS Retriever API)
      final signature = await _smartAuth.getAppSignature();
      print('App Signature for SMS: $signature');
      
      // Bắt đầu lắng nghe SMS
      final credential = await _smartAuth.getSmsCode();
      
      if (credential != null) {
        print('Received SMS code: ${credential.code}');
        return credential.code;
      }
      
      return null;
    } catch (e) {
      print('Error listening for OTP: $e');
      return null;
    }
  }

  /// Hủy lắng nghe SMS
  static Future<void> stopListening() async {
    try {
      await _smartAuth.removeSmsListener();
    } catch (e) {
      print('Error stopping SMS listener: $e');
    }
  }

  /// Lấy App Signature để gửi trong tin nhắn SMS
  /// Backend cần gửi SMS theo định dạng:
  /// "Your OTP is: 123456\n<#> Your OTP is 123456 [APP_SIGNATURE]"
  static Future<String?> getAppSignature() async {
    try {
      return await _smartAuth.getAppSignature();
    } catch (e) {
      print('Error getting app signature: $e');
      return null;
    }
  }

  /// Tạo mã OTP ngẫu nhiên 6 số
  static String generateOtp() {
    final random = DateTime.now().millisecondsSinceEpoch % 1000000;
    return random.toString().padLeft(6, '0');
  }

  /// Validate OTP format
  static bool isValidOtp(String otp) {
    return RegExp(r'^\d{6}$').hasMatch(otp);
  }
}

