# 🏷️ Hướng dẫn Brandname eSMS

## ❗ Lỗi gặp phải

```
"Gửi SMS thất bại: Sai template Brandname CSKH"
```

### Nguyên nhân:

1. ❌ Brandname không tồn tại hoặc chưa được duyệt
2. ❌ SmsType không phù hợp với Brandname
3. ❌ Nội dung tin nhắn không đúng template

## ✅ Đã sửa

### 1. Thay đổi cấu hình SMS:

```csharp
SmsType = 4      // 4 = OTP (trước là 2)
IsUnicode = 0    // Không dấu (tránh lỗi encoding)
```

### 2. Nội dung tin nhắn không dấu:

```
Ma xac thuc OTP cua ban la: 123456. Ma nay co hieu luc trong 5 phut.
```

### 3. Đổi Brandname thành "Notify"

## 🎯 Brandname có thể dùng

### **Brandname công khai miễn phí:**

| Brandname     | Loại     | Ghi chú                        |
| ------------- | -------- | ------------------------------ |
| `Notify`      | OTP/CSKH | ⭐ Khuyến nghị - Phổ biến nhất |
| `Baotrixemay` | CSKH     | Có thể cần đăng ký riêng       |
| `ESMS`        | Hệ thống | Dùng được nhưng ít tin cậy     |

### **Cách kiểm tra Brandname của bạn:**

1. Đăng nhập vào https://esms.vn
2. Vào menu **"Quản lý API eSMS"** hoặc **"Brandname"**
3. Xem danh sách Brandname đã đăng ký
4. Copy tên chính xác (phân biệt chữ hoa/thường)

## 🧪 Cách test từng bước

### **Bước 1: Test với TEST MODE (Không gửi SMS thật)**

Đảm bảo config như sau:

**File: `appsettings.json`**

```json
{
  "eSMS": {
    "ApiKey": "your_api_key",
    "SecretKey": "your_secret_key",
    "BrandName": "Notify",
    "EnableSms": false,  ← Tắt SMS thật
    "TestPhoneNumbers": ["0123456789"]
  }
}
```

**Test:**

```bash
# 1. Restart backend
dotnet run

# 2. Đăng nhập với SĐT: 0123456789
# 3. Xem OTP trong log backend
```

✅ Nếu thấy OTP trong log → Config backend OK

---

### **Bước 2: Test SMS thật với Brandname công khai**

**File: `appsettings.json`**

```json
{
  "eSMS": {
    "ApiKey": "your_api_key",
    "SecretKey": "your_secret_key",
    "BrandName": "Notify",  ← Dùng brandname công khai
    "EnableSms": true,       ← Bật SMS thật
    "TestPhoneNumbers": ["0123456789"]  ← Giữ số test
  }
}
```

**Test:**

```bash
# 1. Restart backend
dotnet run

# 2. Test với số test trước (không tốn tiền)
#    Đăng nhập: 0123456789 → Xem OTP trong log

# 3. Test với SĐT thật của bạn
#    Đăng nhập: 0912345678 → Nhận SMS
```

---

### **Bước 3: Nếu vẫn lỗi - Thử các SmsType khác**

eSMS có các loại SmsType:

| SmsType | Loại      | Khi nào dùng                         |
| ------- | --------- | ------------------------------------ |
| `1`     | CSKH      | Tin nhắn chăm sóc khách hàng         |
| `2`     | Quảng cáo | Marketing                            |
| `4`     | OTP       | ⭐ Mã xác thực (khuyến nghị cho OTP) |
| `8`     | Brandname | Dùng brandname riêng đã đăng ký      |

**Thử SmsType = 8 nếu bạn có Brandname riêng:**

**File: `SmsService.cs` (dòng 59)**

```csharp
SmsType = 8  // Thử đổi từ 4 sang 8
```

---

## 🔍 Debug: Log chi tiết

Để xem request/response từ eSMS, check log backend:

```bash
info: BusBookingAPI.Services.SmsService[0]
      eSMS Response: {"CodeResult":"104","ErrorMessage":"Sai template Brandname CSKH"}
```

### Các mã lỗi phổ biến:

| CodeResult | Lỗi                    | Giải pháp                          |
| ---------- | ---------------------- | ---------------------------------- |
| `100`      | Thành công ✅          | SMS đã gửi                         |
| `104`      | Sai template Brandname | Đổi Brandname hoặc SmsType         |
| `99`       | Lỗi xác thực           | Sai API Key/Secret Key             |
| `101`      | Hết tiền               | Nạp thêm tiền vào tài khoản        |
| `118`      | Brandname chưa active  | Chờ duyệt hoặc dùng brandname khác |

---

## 📱 Liên hệ Support eSMS

Nếu vẫn gặp vấn đề, liên hệ eSMS support:

- **Hotline**: 1900 3427
- **Email**: support@esms.vn
- **Website**: https://esms.vn/lien-he

**Câu hỏi nên hỏi:**

1. "Brandname nào tôi có thể dùng ngay với API Key: 85338965512EDC9FAF8A4AB99255AD?"
2. "SmsType nào phù hợp để gửi OTP?"
3. "Tại sao tôi gặp lỗi 'Sai template Brandname CSKH'?"

---

## 🎯 Khuyến nghị cuối cùng

### **Cho Development:**

```json
{
  "BrandName": "Notify",
  "EnableSms": false, // Test mode
  "SmsType": 4 // OTP type
}
```

### **Cho Production:**

**Nếu có Brandname riêng:**

```json
{
  "BrandName": "YourBrandName", // Tên đã đăng ký
  "EnableSms": true,
  "SmsType": 8 // Brandname riêng
}
```

**Nếu dùng Brandname công khai:**

```json
{
  "BrandName": "Notify",
  "EnableSms": true,
  "SmsType": 4 // OTP
}
```

---

## 🚀 Tóm tắt các bước

1. ✅ **Đã sửa code**: SmsType = 4, IsUnicode = 0
2. ✅ **Đã đổi Brandname**: "Notify"
3. ✅ **Tắt SMS thật**: EnableSms = false (test trước)
4. 🧪 **Test TEST MODE**: Đăng nhập với 0123456789
5. 🧪 **Test SMS thật**: Bật EnableSms = true, test với SĐT thật
6. 📞 **Nếu vẫn lỗi**: Gọi support eSMS để hỏi Brandname

Chúc bạn thành công! 🎉
