import 'package:local_auth/local_auth.dart';
import 'package:local_auth_android/local_auth_android.dart';
import 'package:shared_preferences/shared_preferences.dart';

class BiometricService {
  static final LocalAuthentication _localAuth = LocalAuthentication();

  /// Kiểm tra xem thiết bị có hỗ trợ sinh trắc học không
  static Future<bool> isBiometricAvailable() async {
    try {
      final bool canAuthenticateWithBiometrics = await _localAuth.canCheckBiometrics;
      final bool canAuthenticate = 
          canAuthenticateWithBiometrics || await _localAuth.isDeviceSupported();
      return canAuthenticate;
    } catch (e) {
      print('Error checking biometric availability: $e');
      return false;
    }
  }

  /// Lấy danh sách các phương thức sinh trắc học có sẵn
  static Future<List<BiometricType>> getAvailableBiometrics() async {
    try {
      return await _localAuth.getAvailableBiometrics();
    } catch (e) {
      print('Error getting available biometrics: $e');
      return [];
    }
  }

  /// Xác thực bằng sinh trắc học (vân tay/Face ID)
  static Future<bool> authenticate({
    String reason = 'Vui lòng xác thực để đăng nhập',
  }) async {
    try {
      final bool isAvailable = await isBiometricAvailable();
      if (!isAvailable) {
        return false;
      }

      return await _localAuth.authenticate(
        localizedReason: reason,
        authMessages: const <AuthMessages>[
          AndroidAuthMessages(
            signInTitle: 'Xác thực sinh trắc học',
            cancelButton: 'Hủy',
            biometricHint: 'Xác minh danh tính',
            biometricNotRecognized: 'Không nhận diện được',
            biometricSuccess: 'Xác thực thành công',
            deviceCredentialsRequiredTitle: 'Yêu cầu xác thực',
            goToSettingsButton: 'Cài đặt',
            goToSettingsDescription: 'Vui lòng cài đặt sinh trắc học',
          ),
        ],
        options: const AuthenticationOptions(
          stickyAuth: true,
          biometricOnly: false, // Cho phép dùng PIN/Password nếu vân tay không khả dụng
        ),
      );
    } catch (e) {
      print('Error during biometric authentication: $e');
      return false;
    }
  }

  /// Lưu thông tin đăng nhập cho biometric
  static Future<void> saveBiometricCredentials({
    required String phone,
    required String userId,
  }) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('biometric_phone', phone);
      await prefs.setString('biometric_userId', userId);
      await prefs.setBool('biometric_enabled', true);
    } catch (e) {
      print('Error saving biometric credentials: $e');
    }
  }

  /// Lấy thông tin đăng nhập đã lưu
  static Future<Map<String, String>?> getBiometricCredentials() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final bool enabled = prefs.getBool('biometric_enabled') ?? false;
      
      if (!enabled) return null;

      final String? phone = prefs.getString('biometric_phone');
      final String? userId = prefs.getString('biometric_userId');

      if (phone == null || userId == null) return null;

      return {
        'phone': phone,
        'userId': userId,
      };
    } catch (e) {
      print('Error getting biometric credentials: $e');
      return null;
    }
  }

  /// Kiểm tra xem biometric đã được bật chưa
  static Future<bool> isBiometricEnabled() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getBool('biometric_enabled') ?? false;
    } catch (e) {
      print('Error checking biometric status: $e');
      return false;
    }
  }

  /// Tắt biometric login
  static Future<void> disableBiometric() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove('biometric_phone');
      await prefs.remove('biometric_userId');
      await prefs.setBool('biometric_enabled', false);
    } catch (e) {
      print('Error disabling biometric: $e');
    }
  }

  /// Lấy tên phương thức sinh trắc học
  static String getBiometricTypeName(BiometricType type) {
    switch (type) {
      case BiometricType.face:
        return 'Face ID';
      case BiometricType.fingerprint:
        return 'Vân tay';
      case BiometricType.iris:
        return 'Mống mắt';
      case BiometricType.strong:
        return 'Sinh trắc học mạnh';
      case BiometricType.weak:
        return 'Sinh trắc học yếu';
      default:
        return 'Sinh trắc học';
    }
  }
}

