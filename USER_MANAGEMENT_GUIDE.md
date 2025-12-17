# Hướng dẫn Quản lý Người dùng

## 📱 Tính năng mới trong Admin Dashboard

Đã thêm tính năng **Quản lý Người dùng** với đầy đủ các chức năng CRUD (Create, Read, Update, Delete).

---

## 🎯 Chức năng

### 1. **Xem danh sách người dùng**

- Hiển thị tất cả người dùng trong hệ thống
- Thông tin hiển thị:
  - Họ tên
  - Số điện thoại
  - Email
  - Vai trò (Customer/Admin/Staff)
  - Trạng thái (Active/Inactive)

### 2. **Tìm kiếm người dùng**

- Tìm theo tên
- Tìm theo số điện thoại
- Tìm theo email
- Kết quả tìm kiếm real-time

### 3. **Thêm người dùng mới**

- Nhập họ tên
- Nhập email (optional)
- Nhập số điện thoại (required, unique)
- Nhập mật khẩu (tối thiểu 6 ký tự)
- Chọn vai trò:
  - **Customer**: Khách hàng thông thường
  - **Admin**: Quản trị viên (full access)

### 4. **Chỉnh sửa người dùng**

- Cập nhật thông tin cá nhân
- Thay đổi vai trò
- Đổi mật khẩu (để trống nếu không muốn đổi)
- Thay đổi trạng thái (Active/Inactive)

### 5. **Xóa người dùng**

- **Người dùng có lịch sử đặt vé**: Tự động chuyển sang trạng thái "Inactive" thay vì xóa hoàn toàn
- **Người dùng chưa đặt vé**: Xóa hoàn toàn khỏi hệ thống

---

## 🖥️ Backend API

### Endpoints

#### 1. GET `/api/users`

**Lấy danh sách tất cả người dùng**

- **Authorization**: Admin only
- **Response**: List of UserDto

```json
[
  {
    "userId": "guid-string",
    "fullName": "Nguyễn Văn A",
    "email": "user@example.com",
    "phone": "0123456789",
    "role": "Customer",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

#### 2. GET `/api/users/{id}`

**Lấy thông tin chi tiết một người dùng**

- **Authorization**: Admin only
- **Response**: UserDto

#### 3. POST `/api/users`

**Tạo người dùng mới**

- **Authorization**: Admin only
- **Request Body**:

```json
{
  "fullName": "Nguyễn Văn A",
  "email": "user@example.com",
  "phone": "0123456789",
  "password": "password123",
  "role": "Customer"
}
```

#### 4. PUT `/api/users/{id}`

**Cập nhật thông tin người dùng**

- **Authorization**: Admin only
- **Request Body**:

```json
{
  "fullName": "Nguyễn Văn A",
  "email": "user@example.com",
  "phone": "0123456789",
  "password": "newpassword123", // Optional
  "role": "Customer"
}
```

#### 5. DELETE `/api/users/{id}`

**Xóa người dùng**

- **Authorization**: Admin only
- **Logic**:
  - Nếu user có bookings: Deactivate (set `isActive = false`)
  - Nếu user chưa có bookings: Hard delete

#### 6. PUT `/api/users/{id}/toggle-status`

**Bật/tắt trạng thái người dùng**

- **Authorization**: Admin only
- **Response**:

```json
{
  "message": "Đã kích hoạt/vô hiệu hóa người dùng",
  "isActive": true
}
```

---

## 📱 Flutter Screen

### File Structure

```
lib/screens/admin/
├── admin_dashboard_screen.dart    # Dashboard chính
├── user_management_screen.dart     # Màn hình quản lý users (MỚI)
├── admin_trips_screen.dart
└── admin_bookings_screen.dart
```

### Truy cập

1. Đăng nhập với tài khoản Admin
2. Vào tab "Tài khoản"
3. Chọn "Admin Dashboard"
4. Nhấn vào "Quản lý người dùng" trong phần "Thao tác nhanh"

---

## 🔒 Bảo mật

### Authorization

- **Tất cả endpoints** yêu cầu role `Admin`
- Kiểm tra qua `[Authorize(Roles = "Admin")]`
- Token JWT phải hợp lệ

### Validation

- **Phone**: Unique, required
- **Email**: Unique (nếu có), valid format
- **Password**: Minimum 6 ký tự
- **Role**: Phải là một trong `Customer`, `Admin`

### Business Logic

- Không cho phép xóa user nếu có lịch sử booking
- Tự động hash password với BCrypt
- Kiểm tra trùng lặp phone/email trước khi create/update

---

## 🎨 UI Features

### Màu sắc theo vai trò

- **Admin**: 🔴 Đỏ (Red)
- **Customer**: 🟢 Xanh lá (Green)

### Icons

- Admin: `admin_panel_settings`
- Customer: `person`

### User Card Layout

```
┌─────────────────────────────────┐
│ [Icon] Nguyễn Văn A        [⋮] │
│        📱 0123456789            │
│        📧 user@example.com      │
│        [Khách hàng]             │
└─────────────────────────────────┘
```

---

## 🧪 Testing

### Test Cases

1. **Thêm user mới**

   - ✅ Thêm với đầy đủ thông tin
   - ✅ Thêm không có email (optional)
   - ❌ Thêm với phone trùng lặp
   - ❌ Thêm với password < 6 ký tự

2. **Sửa user**

   - ✅ Cập nhật thông tin không đổi password
   - ✅ Cập nhật và đổi password
   - ✅ Thay đổi role
   - ❌ Đổi phone trùng với user khác

3. **Xóa user**

   - ✅ Xóa user chưa có booking → Hard delete
   - ✅ Xóa user có booking → Soft delete (isActive = false)

4. **Tìm kiếm**
   - ✅ Tìm theo tên
   - ✅ Tìm theo phone
   - ✅ Tìm theo email
   - ✅ Real-time search

---

## 🚀 Deployment

### Backend

```bash
cd BusBookingAPI/BusBookingAPI
dotnet build
dotnet run
```

### Frontend

```bash
flutter pub get
flutter run
```

---

## 📝 Notes

- User model đã được cập nhật với trường `isActive`
- API Service đã có đầy đủ methods cho user management
- Screen tự động refresh sau mỗi thao tác CRUD
- Hỗ trợ pull-to-refresh

---

## 🐛 Troubleshooting

### Lỗi "Unauthorized"

- Kiểm tra token JWT còn hạn
- Đảm bảo user hiện tại có role `Admin`

### Lỗi "Phone đã tồn tại"

- Phone number phải unique
- Kiểm tra database xem phone đã tồn tại chưa

### Lỗi khi build backend

```bash
dotnet clean
dotnet restore
dotnet build
```

---

## 📞 Support

Nếu gặp vấn đề, kiểm tra:

1. Backend logs: `BusBookingAPI/BusBookingAPI/bin/Debug/net8.0/`
2. Flutter logs: Console output khi chạy `flutter run`
3. Database: Kiểm tra bảng `Users` trong SQL Server
