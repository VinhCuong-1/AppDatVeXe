# 📱 Hướng dẫn đăng nhập OTP trên Android Emulator

## ❓ Vấn đề

Khi đã tích hợp SMS thật (eSMS), **máy ảo Android không thể nhận tin nhắn SMS thật**, dẫn đến không thể đăng nhập khi test.

## ✅ Các giải pháp

---

### **Giải pháp 1: Danh sách số điện thoại Test (Khuyến nghị) ⭐**

Cách này cho phép **một số điện thoại cụ thể** luôn ở chế độ TEST MODE (không gửi SMS thật), còn lại gửi SMS bình thường.

#### Cấu hình:

**File: `BusBookingAPI/BusBookingAPI/appsettings.json`**

```json
{
  "eSMS": {
    "ApiKey": "your_real_api_key",
    "SecretKey": "your_real_secret_key",
    "BrandName": "YourBrand",
    "EnableSms": true, // Bật SMS thật
    "TestPhoneNumbers": [
      // Danh sách số test
      "0123456789", // Số này KHÔNG gửi SMS thật
      "0987654321" // Số này KHÔNG gửi SMS thật
    ]
  }
}
```

#### Cách hoạt động:

- ✅ Số trong `TestPhoneNumbers`: Không gửi SMS, OTP hiển thị trong log/response
- ✅ Số khác: Gửi SMS thật qua eSMS

#### Ưu điểm:

- ✅ Vừa test được trên emulator
- ✅ Vừa hoạt động với SMS thật cho user thật
- ✅ Không cần tắt bật EnableSms
- ✅ Phù hợp cho cả dev và production

---

### **Giải pháp 2: Tắt SMS thật khi Develop**

Đơn giản là tắt `EnableSms` khi develop trên emulator.

#### Cấu hình:

```json
{
  "eSMS": {
    "EnableSms": false // Tắt khi develop
  }
}
```

#### Ưu điểm:

- ✅ Đơn giản nhất
- ✅ OTP hiển thị trong console và response message

#### Nhược điểm:

- ❌ Phải nhớ bật lại khi deploy production
- ❌ Không thể test SMS thật trong quá trình develop

---

### **Giải pháp 3: Gửi SMS đến điện thoại thật của developer**

Sử dụng số điện thoại thật trong emulator nhưng SMS gửi đến điện thoại thật.

#### Cách làm:

1. Đăng ký tài khoản với SĐT thật của bạn:

```dart
RegisterRequest(
  phone: "0912345678",  // SĐT thật của bạn
  ...
)
```

2. Khi đăng nhập trên emulator:
   - Nhập SĐT thật
   - SMS gửi đến điện thoại thật của bạn
   - Xem OTP và nhập vào emulator

#### Ưu điểm:

- ✅ Test được luồng SMS thật
- ✅ Không cần config gì thêm

#### Nhược điểm:

- ❌ Tốn phí SMS mỗi lần test
- ❌ Phải có điện thoại thật bên cạnh
- ❌ Chậm hơn (phải đợi SMS)

---

### **Giải pháp 4: Chạy trên điện thoại Android thật**

Cách tốt nhất để test SMS OTP là dùng thiết bị thật.

#### Cách làm:

1. Bật USB Debugging trên điện thoại Android
2. Kết nối điện thoại với máy tính
3. Cập nhật `api_config.dart`:

```dart
static const String localhostUrlPhysicalDevice = 'http://YOUR_PC_IP:5264/api';
```

4. Run Flutter app trực tiếp lên điện thoại:

```bash
flutter run
```

#### Ưu điểm:

- ✅ Test chính xác nhất
- ✅ Nhận SMS thật
- ✅ Test được tất cả tính năng (SMS, biometric, camera...)

#### Nhược điểm:

- ❌ Cần có thiết bị Android thật
- ❌ Tốn phí SMS

---

### **Giải pháp 5: Sử dụng Android Emulator với SIM ảo (Nâng cao)**

Android Emulator có thể nhận SMS ảo qua console.

#### Cách làm:

1. Mở Android Emulator
2. Mở "Extended Controls" (icon "..." bên cạnh emulator)
3. Chọn tab "Phone"
4. Gửi SMS test đến số điện thoại ảo của emulator

**Nhưng**: Cách này KHÔNG hoạt động với SMS thật từ eSMS, chỉ dùng để test SMS ảo.

---

## 🎯 Khuyến nghị

### Cho Development (Develop trên emulator):

✅ **Dùng Giải pháp 1** - Danh sách số test

- Thêm SĐT test vào `TestPhoneNumbers`
- Bật `EnableSms = true` luôn
- Developer dùng số test, user thật dùng SMS thật

### Cho Testing (Test thật trước khi deploy):

✅ **Dùng Giải pháp 4** - Điện thoại thật

- Test với SMS thật trên thiết bị thật
- Đảm bảo mọi thứ hoạt động

### Cho Production:

✅ Bật `EnableSms = true`
✅ Có thể giữ lại 1-2 số test (ví dụ: số hotline, số admin) trong `TestPhoneNumbers`

---

## 📋 Cấu hình khuyến nghị

**File: `appsettings.json` (Production)**

```json
{
  "eSMS": {
    "ApiKey": "production_api_key",
    "SecretKey": "production_secret_key",
    "BrandName": "YourBrand",
    "EnableSms": true,
    "TestPhoneNumbers": [
      "0911111111" // Chỉ giữ số admin/hotline
    ]
  }
}
```

**File: `appsettings.Development.json` (Development)**

```json
{
  "eSMS": {
    "ApiKey": "test_api_key",
    "SecretKey": "test_secret_key",
    "BrandName": "TestBrand",
    "EnableSms": false, // Hoặc true nếu dùng TestPhoneNumbers
    "TestPhoneNumbers": ["0123456789", "0987654321", "0912345678"]
  }
}
```

---

## 🔍 Debug: Xem OTP trong Console

Dù bật hay tắt SMS, OTP luôn được log ra console/terminal:

```bash
# Chạy backend
cd BusBookingAPI/BusBookingAPI
dotnet run

# Xem log:
info: BusBookingAPI.Services.OtpService[0]
      OTP cho 0123456789: 456789 (Hết hạn: 2024-01-15 10:05:00)

info: BusBookingAPI.Services.SmsService[0]
      [TEST PHONE] SMS OTP to 0123456789: 456789
```

Bạn có thể copy OTP từ log để nhập vào app!

---

## 💡 Mẹo hay

### Tự động điền OTP khi develop (Flutter)

Nếu muốn tự động điền OTP từ TEST MODE, bỏ comment dòng này:

**File: `lib/screens/auth/login_screen.dart` (dòng 116)**

```dart
// Bỏ comment dòng này khi develop:
_password2faOtpController.text = otpCode;
```

⚠️ **Nhớ comment lại trước khi deploy production!**

---

## ❓ FAQ

### Q: Tại sao không tắt hẳn OTP khi develop?

**A:** Vì cần test đầy đủ luồng bảo mật. Dùng TEST MODE vẫn giữ được logic OTP.

### Q: Có cách nào nhận SMS thật trên emulator không?

**A:** Không. Emulator không có SIM thật nên không nhận được SMS từ nhà mạng.

### Q: TestPhoneNumbers có tốn phí SMS không?

**A:** Không. Số trong TestPhoneNumbers KHÔNG gửi SMS thật.

### Q: Có thể thêm nhiều số vào TestPhoneNumbers không?

**A:** Có, thêm bao nhiêu cũng được. Mỗi số trên 1 dòng.

---

## 🚀 Tổng kết

**Giải pháp tốt nhất**: Dùng **TestPhoneNumbers** (Giải pháp 1)

- ✅ Linh hoạt nhất
- ✅ Phù hợp cho cả dev và production
- ✅ Không tốn phí SMS khi test
- ✅ Vẫn test được SMS thật cho số khác

Chúc bạn thành công! 🎉
