# Hướng dẫn chạy Backend API

## Yêu cầu hệ thống

- .NET 9.0 SDK
- SQL Server (LocalDB, Express, hoặc Full)
- Visual Studio 2022 hoặc VS Code

## Cài đặt

### 1. Cài đặt .NET 9.0 SDK

Tải và cài đặt .NET 9.0 SDK từ: https://dotnet.microsoft.com/download/dotnet/9.0

### 2. Cài đặt SQL Server

**Tùy chọn 1: SQL Server LocalDB (Khuyến nghị cho development)**

```bash
# Tải SQL Server Express LocalDB
# https://www.microsoft.com/en-us/sql-server/sql-server-downloads
```

**Tùy chọn 2: SQL Server Express**

```bash
# Tải SQL Server Express
# https://www.microsoft.com/en-us/sql-server/sql-server-downloads
```

**Tùy chọn 3: Docker (Nếu có Docker)**

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

## Chạy ứng dụng

### 1. Mở terminal trong thư mục BusBookingAPI/BusBookingAPI

```bash
cd BusBookingAPI/BusBookingAPI
```

### 2. Khôi phục packages

```bash
dotnet restore
```

### 3. Cập nhật connection string (nếu cần)

Mở file `appsettings.json` và cập nhật connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BusBookingDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**Cho SQL Server Express:**

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=BusBookingDB;Trusted_Connection=true;TrustServerCertificate=true;"
```

**Cho Docker:**

```json
"DefaultConnection": "Server=localhost,1433;Database=BusBookingDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;"
```

### 4. Chạy ứng dụng

cd BusBookingAPI/BusBookingAPI
dotnet restore
dotnet run

Ứng dụng sẽ chạy tại: `https://localhost:7000` hoặc `http://localhost:5000`

## Kiểm tra API

### 1. Health Check

```bash
curl https://localhost:7000/api/health
```

### 2. Swagger UI

Mở trình duyệt và truy cập: `https://localhost:7000/swagger`

## Cấu trúc API

### Authentication Endpoints

- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/register` - Đăng ký
- `GET /api/auth/me` - Lấy thông tin user hiện tại

### Trip Endpoints

- `POST /api/trips/search` - Tìm kiếm chuyến xe
- `GET /api/trips/{id}` - Lấy chi tiết chuyến xe
- `GET /api/trips/{id}/seats` - Lấy danh sách ghế

### Booking Endpoints

- `POST /api/bookings/create` - Tạo booking
- `GET /api/bookings/{id}` - Lấy chi tiết booking
- `GET /api/bookings/user/{userId}` - Lấy booking của user
- `DELETE /api/bookings/{id}` - Hủy booking

### Check-in Endpoints

- `POST /api/checkin` - Check-in vé
- `GET /api/checkin/verify?token={qrToken}` - Xác thực vé

## Tài khoản mặc định

Sau khi chạy lần đầu, hệ thống sẽ tự động tạo tài khoản admin:

- **Username:** admin
- **Password:** Admin123!
- **Email:** admin@example.com
- **Phone:** 0123456789

## Troubleshooting

### Lỗi kết nối database

1. Kiểm tra SQL Server đã chạy chưa
2. Kiểm tra connection string
3. Kiểm tra firewall

### Lỗi SSL Certificate

```bash
# Chạy với lệnh này để bỏ qua SSL
dotnet run --environment Development
```

### Lỗi CORS

Kiểm tra cấu hình CORS trong `Program.cs` và `appsettings.json`

## Cấu hình cho Flutter App

Trong Flutter app, cập nhật `lib/config/api_config.dart`:

```dart
static const String baseUrl = 'https://localhost:7000/api';
// hoặc
static const String baseUrl = 'http://localhost:5000/api';
```

## Database Schema

API sẽ tự động tạo database và các bảng khi chạy lần đầu. Các bảng chính:

- **AspNetUsers** - Users (ASP.NET Identity)
- **AspNetRoles** - Roles (ASP.NET Identity)
- **Routes** - Tuyến đường
- **Trips** - Chuyến xe
- **Seats** - Ghế ngồi
- **Bookings** - Đặt vé
- **CheckinLogs** - Log check-in
- **AuditLogs** - Log audit

## Security

- JWT Authentication
- Password hashing với ASP.NET Identity
- CORS configuration
- Input validation
- SQL injection protection với Entity Framework

## Monitoring

- Logging với ILogger
- Health check endpoint
- Swagger UI cho testing
- Audit logs cho tracking
