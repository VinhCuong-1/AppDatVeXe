# Hướng dẫn tích hợp eSMS để gửi OTP

## 🎯 Tổng quan

Dự án đã được tích hợp sẵn dịch vụ eSMS (esms.vn) để gửi mã OTP qua tin nhắn SMS thật. Hiện tại đang ở chế độ TEST MODE (không gửi SMS thật), bạn cần cấu hình để bật chế độ production.

## 📋 Bước 1: Đăng ký tài khoản eSMS

1. Truy cập: https://esms.vn
2. Đăng ký tài khoản doanh nghiệp
3. Xác minh tài khoản và nạp tiền vào tài khoản
4. Lấy thông tin:
   - **API Key**: Tìm trong phần "Cài đặt API"
   - **Secret Key**: Tìm trong phần "Cài đặt API"
   - **Brandname**: Tên thương hiệu hiển thị khi gửi SMS (ví dụ: "Baotrixemay")

## 📝 Bước 2: Cấu hình Backend (.NET)

### 2.1. Cập nhật `appsettings.json`

Mở file `BusBookingAPI/BusBookingAPI/appsettings.json` và cập nhật phần `eSMS`:

```json
{
  "eSMS": {
    "ApiKey": "YOUR_ESMS_API_KEY_HERE", // Thay bằng API Key thật
    "SecretKey": "YOUR_ESMS_SECRET_KEY_HERE", // Thay bằng Secret Key thật
    "BrandName": "YourBrandName", // Tên thương hiệu (đã đăng ký với eSMS)
    "EnableSms": true, // Đổi thành true để bật gửi SMS thật
    "TestPhoneNumbers": [
      // Danh sách số test (không gửi SMS thật)
      "0123456789", // Số này dùng cho test trên emulator
      "0987654321" // Thêm số test khác nếu cần
    ]
  }
}
```

### 2.2. Giải thích các thông số:

- **ApiKey**: API Key từ tài khoản eSMS
- **SecretKey**: Secret Key từ tài khoản eSMS
- **BrandName**: Tên thương hiệu hiển thị trên SMS
- **EnableSms**: `true` = gửi SMS thật, `false` = chế độ test
- **TestPhoneNumbers**: Danh sách số điện thoại luôn dùng TEST MODE (không gửi SMS thật)
  - ⭐ **Dùng để test trên Android Emulator** (emulator không nhận SMS thật)
  - Số trong list này sẽ KHÔNG gửi SMS thật ngay cả khi `EnableSms = true`
  - OTP sẽ hiển thị trong log backend và response message
  - Thêm số điện thoại test của developer vào đây

### 2.3. Cập nhật `appsettings.Development.json` (nếu cần test)

```json
{
  "eSMS": {
    "ApiKey": "YOUR_TEST_API_KEY",
    "SecretKey": "YOUR_TEST_SECRET_KEY",
    "BrandName": "Baotrixemay",
    "EnableSms": false, // Để false khi develop
    "TestPhoneNumbers": ["0123456789"]
  }
}
```

## 🔄 Bước 3: Chạy lại Backend

```bash
cd BusBookingAPI/BusBookingAPI
dotnet run
```

## 📱 Bước 4: Cấu hình Flutter (Optional - Auto-fill OTP)

### Bật auto-fill OTP khi test

Nếu muốn auto-fill OTP từ TEST MODE khi develop, bỏ comment dòng này trong `lib/screens/auth/login_screen.dart`:

```dart
// Dòng 116
_password2faOtpController.text = otpCode;
```

### Tắt auto-fill khi production

Giữ nguyên code hiện tại (đã comment sẵn), người dùng sẽ phải nhập OTP thủ công từ tin nhắn SMS.

## ✅ Kiểm tra hoạt động

### Test Mode (EnableSms = false)

- Backend sẽ log OTP ra console
- Response message chứa: `[TEST MODE: 123456]`
- OTP vẫn được lưu vào database và có thể verify

### Production Mode (EnableSms = true)

- Backend gửi SMS thật qua eSMS API
- Người dùng nhận tin nhắn: "Ma OTP cua ban la: 123456. Ma co hieu luc trong 5 phut..."
- OTP được lưu vào database và có thể verify

## 🎨 Format tin nhắn SMS

Tin nhắn được gửi có format:

```
Ma OTP cua ban la: [6-digit-code]. Ma co hieu luc trong 5 phut. Khong chia se ma nay voi bat ky ai.
```

Bạn có thể thay đổi nội dung này trong file `BusBookingAPI/BusBookingAPI/Services/SmsService.cs` (dòng 48):

```csharp
var smsContent = $"Ma OTP cua ban la: {otpCode}. Ma co hieu luc trong 5 phut. Khong chia se ma nay voi bat ky ai.";
```

## 💰 Chi phí eSMS

- Giá SMS OTP: Khoảng 200-350 VNĐ/tin nhắn (tùy gói)
- Brandname riêng: Phí đăng ký khoảng 500,000 - 1,000,000 VNĐ/năm
- Có thể dùng Brandname công khai miễn phí (ví dụ: "Baotrixemay")

## 🔧 Xử lý lỗi

### Lỗi "Failed to send SMS"

- Kiểm tra API Key và Secret Key
- Kiểm tra số dư tài khoản eSMS
- Kiểm tra Brandname đã được duyệt chưa

### Lỗi "Invalid phone number"

- Backend tự động chuẩn hóa số điện thoại (0xxx -> 84xxx)
- Đảm bảo số điện thoại đúng định dạng Việt Nam

### Lỗi kết nối eSMS API

- Kiểm tra kết nối internet
- Kiểm tra firewall/proxy có chặn không

## 📚 Tài liệu eSMS API

- API Documentation: https://esms.vn/blog/api-sms
- Support: https://esms.vn/lien-he

## 🔒 Bảo mật

⚠️ **QUAN TRỌNG:**

- KHÔNG commit API Key và Secret Key lên Git
- Sử dụng environment variables hoặc Azure Key Vault trong production
- Thêm `appsettings.json` vào `.gitignore` (đã có sẵn)

## 📱 Lưu ý quan trọng: Test trên Android Emulator

⚠️ **Android Emulator không thể nhận SMS thật!**

Để test OTP trên emulator, bạn có 2 cách:

### Cách 1: Dùng TestPhoneNumbers (Khuyến nghị)

Thêm số điện thoại test vào `TestPhoneNumbers` trong config:

```json
"TestPhoneNumbers": [
  "0123456789"  // Số test cho emulator
]
```

Khi đăng nhập với số này, OTP sẽ hiển thị trong:

- Log backend (terminal)
- Response message: `[TEST MODE: 123456]`

### Cách 2: Test trên điện thoại Android thật

Kết nối điện thoại thật và run app trực tiếp:

```bash
flutter run
```

📖 **Xem thêm**: `ANDROID_EMULATOR_SMS_GUIDE.md` để biết chi tiết các giải pháp khác.

---

## 🎯 Tổng kết

Sau khi hoàn thành các bước trên:
✅ Backend sẽ gửi OTP qua SMS thật
✅ Người dùng nhận tin nhắn trên điện thoại
✅ Đăng nhập 2 lớp bảo mật hoạt động hoàn chỉnh
✅ Có thể test trên emulator với TestPhoneNumbers

Chúc bạn thành công! 🚀
