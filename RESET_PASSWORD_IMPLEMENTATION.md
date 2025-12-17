# 🔐 Reset Password Implementation Guide

## ✅ **ĐÃ HOÀN THÀNH**

### **1. Frontend (Flutter)**

#### **Files đã tạo/cập nhật:**

- ✅ `lib/screens/auth/forgot_password_screen.dart` - Màn hình quên mật khẩu
- ✅ `lib/screens/auth/login_screen.dart` - Thêm link "Quên mật khẩu?"
- ✅ `lib/services/api_service.dart` - Thêm method `resetPassword()`

#### **Flow:**

1. User nhấn "Quên mật khẩu?" trên màn hình login
2. Nhập số điện thoại → Gửi OTP
3. Nhập OTP (auto-fill trong TEST MODE)
4. Nhập mật khẩu mới + xác nhận mật khẩu
5. Submit → Verify OTP + Reset password
6. Success → Quay về màn hình login

---

### **2. Backend (ASP.NET Core)**

#### **Files đã tạo/cập nhật:**

- ✅ `BusBookingAPI/BusBookingAPI/Controllers/AuthController.cs`
  - Thêm endpoint `POST /api/auth/reset-password`
- ✅ `BusBookingAPI/BusBookingAPI/Services/AuthService.cs`
  - Thêm interface method: `Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request)`
  - Thêm implementation: Verify OTP → Reset password using `UserManager`
- ✅ `BusBookingAPI/BusBookingAPI/Models/DTOs.cs`
  - Thêm class `ResetPasswordRequest`:
    ```csharp
    public class ResetPasswordRequest
    {
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
    ```

#### **API Endpoint:**

```http
POST /api/auth/reset-password
Content-Type: application/json

{
  "phone": "0123456789",
  "otp": "123456",
  "newPassword": "NewPassword123"
}
```

#### **Response:**

```json
{
  "success": true,
  "message": "Đặt lại mật khẩu thành công! Vui lòng đăng nhập lại.",
  "data": null,
  "errors": null
}
```

---

## 🔧 **Backend Logic**

### **ResetPasswordAsync Method:**

1. **Tìm user** bằng số điện thoại
2. **Verify OTP**:
   - Tìm OTP record trong database
   - Check: OTP hợp lệ, chưa hết hạn, chưa sử dụng
3. **Mark OTP as used** để tránh tái sử dụng
4. **Reset password**:
   - Generate password reset token
   - Use `UserManager.ResetPasswordAsync()` để đổi mật khẩu
5. **Return success/error response**

### **Security Features:**

- ✅ OTP verification bắt buộc
- ✅ OTP chỉ dùng được 1 lần
- ✅ OTP có thời gian hết hạn (5 phút)
- ✅ Password validation (minimum 6 characters)
- ✅ Use ASP.NET Identity `UserManager` for secure password hashing

---

## 📱 **Testing**

### **Test Flow:**

#### **1. Start Backend:**

```bash
cd BusBookingAPI/BusBookingAPI
dotnet run
```

#### **2. Run Flutter App:**

```bash
flutter run
```

#### **3. Test Steps:**

1. ✅ Mở app → Nhấn "Quên mật khẩu?"
2. ✅ Nhập SĐT: `0123456789`
3. ✅ Nhấn "Gửi mã OTP"
4. ✅ OTP tự động điền (TEST MODE)
5. ✅ Nhập mật khẩu mới: `NewPassword123`
6. ✅ Nhập lại mật khẩu: `NewPassword123`
7. ✅ Nhấn "Đặt lại mật khẩu"
8. ✅ Xem dialog thành công
9. ✅ Quay về login → Đăng nhập với mật khẩu mới

---

## 🎨 **UI Features**

### **Login Screen:**

- Logo gradient đẹp
- Tên công ty "NHÀ XE NGŨ AN" nổi bật
- Link "Quên mật khẩu?" màu xanh

### **Forgot Password Screen:**

- Icon lock_reset
- Form 4 bước rõ ràng
- Countdown timer cho OTP
- Show/hide password toggle
- Success dialog với icon check_circle

---

## 📝 **Code Examples**

### **Flutter - Call API:**

```dart
final response = await ApiService.resetPassword(
  phone: '0123456789',
  otp: '123456',
  newPassword: 'NewPassword123',
);

if (response['success'] == true) {
  // Show success, navigate to login
} else {
  // Show error
}
```

### **Backend - Verify OTP:**

```csharp
var otpRecord = await _context.OtpCodes
    .Where(o => o.Phone == request.Phone && o.Code == request.Otp)
    .OrderByDescending(o => o.CreatedAt)
    .FirstOrDefaultAsync();

if (otpRecord == null || otpRecord.ExpiresAt < DateTime.UtcNow || otpRecord.IsUsed)
{
    return new ApiResponse<object>
    {
        Success = false,
        Message = "Mã OTP không hợp lệ hoặc đã hết hạn"
    };
}
```

---

## 🚀 **Deployment Notes**

### **Production Checklist:**

- [ ] Đổi TEST MODE thành Production (gửi SMS thật)
- [ ] Tăng độ phức tạp password requirement
- [ ] Add rate limiting cho endpoint reset password
- [ ] Add reCAPTCHA để chống spam
- [ ] Log tất cả password reset attempts
- [ ] Gửi email/SMS thông báo khi password thay đổi

---

## 📊 **Database Schema**

### **OtpCodes Table:**

```sql
CREATE TABLE OtpCodes (
    Id INT PRIMARY KEY IDENTITY,
    Phone NVARCHAR(15) NOT NULL,
    Code NVARCHAR(6) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ExpiresAt DATETIME2 NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0
);
```

---

## ✅ **Summary**

**Feature:** ✅ HOÀN THÀNH  
**Frontend:** ✅ Flutter UI + API integration  
**Backend:** ✅ ASP.NET Core API endpoint  
**Security:** ✅ OTP verification + Password hashing  
**Testing:** ✅ Đã test thành công

**Next Steps:**

- Test trên production environment
- Add email notification
- Enhance security (rate limiting, reCAPTCHA)
