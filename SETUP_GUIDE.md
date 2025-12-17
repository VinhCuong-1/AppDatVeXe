# Hướng dẫn Setup và Chạy Ứng dụng Đặt Vé Xe

## 📋 Yêu cầu hệ thống

- **Flutter SDK**: 3.9.2 hoặc cao hơn
- **Dart SDK**: 3.0.0 hoặc cao hơn
- **Android Studio** hoặc **VS Code** với Flutter extension
- **Git** để clone repository

## 🚀 Cài đặt và chạy ứng dụng

### Bước 1: Clone và cài đặt dependencies

```bash
# Clone repository (nếu có)
git clone <repository-url>
cd app_dat_ve_xe

# Hoặc nếu đã có project, chạy:
flutter pub get
```

### Bước 2: Cấu hình API

Mở file `lib/config/api_config.dart` và cấu hình:

```dart
class ApiConfig {
  // Thay đổi URL này thành URL API thực tế của bạn
  static const String baseUrl = 'https://your-api-domain.com/api';

  // Hoặc sử dụng localhost khi test
  // static const String baseUrl = 'http://localhost:5000/api';

  // Bật/tắt mock data (true = sử dụng mock data, false = gọi API thật)
  static const bool useMockData = true;
}
```

### Bước 3: Chạy ứng dụng

```bash
# Chạy trên Android
flutter run

# Chạy trên iOS (chỉ trên macOS)
flutter run -d ios

# Chạy trên web
flutter run -d web

# Chạy trên desktop
flutter run -d windows
flutter run -d macos
flutter run -d linux
```

## 🗄️ Setup Database (SQL Server)

### Bước 1: Cài đặt SQL Server

1. Tải và cài đặt **SQL Server** hoặc **SQL Server Express**
2. Cài đặt **SQL Server Management Studio (SSMS)**

### Bước 2: Tạo Database

1. Mở **SQL Server Management Studio**
2. Kết nối đến SQL Server instance
3. Mở file `database_setup.sql` đã tạo
4. Chạy script để tạo database và các bảng

### Bước 3: Kiểm tra dữ liệu

```sql
-- Kiểm tra dữ liệu đã được tạo
USE BusBookingDB;
SELECT COUNT(*) as RouteCount FROM Routes;
SELECT COUNT(*) as TripCount FROM Trips;
SELECT COUNT(*) as SeatCount FROM Seats;
SELECT COUNT(*) as UserCount FROM Users;
```

## 🔧 Setup Backend API

### Option 1: ASP.NET Core Web API

1. **Tạo project**:

   ```bash
   dotnet new webapi -n BusBookingAPI
   cd BusBookingAPI
   ```

2. **Cài đặt packages**:

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
   dotnet add package BCrypt.Net-Next
   ```

3. **Cấu hình connection string** trong `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=BusBookingDB;Trusted_Connection=true;"
     },
     "JWT": {
       "SecretKey": "your-secret-key-here",
       "ExpireHours": 24
     }
   }
   ```

4. **Chạy API**:
   ```bash
   dotnet run
   ```

### Option 2: Node.js + Express

1. **Tạo project**:

   ```bash
   mkdir bus-booking-api
   cd bus-booking-api
   npm init -y
   ```

2. **Cài đặt packages**:

   ```bash
   npm install express cors helmet morgan
   npm install mssql jsonwebtoken bcryptjs
   npm install -D nodemon @types/node
   ```

3. **Tạo file `server.js`**:

   ```javascript
   const express = require("express");
   const cors = require("cors");

   const app = express();
   const PORT = process.env.PORT || 5000;

   app.use(cors());
   app.use(express.json());

   app.get("/api/health", (req, res) => {
     res.json({ message: "API is running!" });
   });

   app.listen(PORT, () => {
     console.log(`Server running on port ${PORT}`);
   });
   ```

4. **Chạy API**:
   ```bash
   npm start
   ```

## 📱 Test ứng dụng

### Với Mock Data (Mặc định)

1. **Đăng nhập**: Sử dụng bất kỳ số điện thoại nào (ví dụ: `0123456789`)
2. **Mật khẩu**: Nhập bất kỳ mật khẩu nào (ví dụ: `123456`)
3. **Tìm chuyến**: Nhập điểm đi/đến bất kỳ
4. **Đặt vé**: Chọn ghế và nhập thông tin

### Với API thật

1. **Tắt mock data**: Đặt `useMockData = false` trong `api_config.dart`
2. **Cập nhật URL**: Đặt URL API thật trong `baseUrl`
3. **Chạy backend**: Đảm bảo API server đang chạy
4. **Test**: Thực hiện các thao tác như bình thường

## 🔍 Debug và Troubleshooting

### Lỗi thường gặp

1. **"No devices found"**:

   ```bash
   # Kiểm tra devices
   flutter devices

   # Khởi động emulator
   flutter emulators --launch <emulator_name>
   ```

2. **"Package not found"**:

   ```bash
   # Cài đặt lại dependencies
   flutter clean
   flutter pub get
   ```

3. **"API connection failed"**:

   - Kiểm tra URL API trong `api_config.dart`
   - Đảm bảo backend server đang chạy
   - Kiểm tra network connection

4. **"Database connection failed"**:
   - Kiểm tra SQL Server đang chạy
   - Kiểm tra connection string
   - Kiểm tra firewall settings

### Debug mode

```bash
# Chạy với debug mode
flutter run --debug

# Chạy với verbose logging
flutter run -v
```

## 📊 Monitoring và Logs

### Flutter Logs

```bash
# Xem logs real-time
flutter logs

# Xem logs của specific device
flutter logs -d <device_id>
```

### Database Monitoring

```sql
-- Kiểm tra performance
SELECT
    t.name AS TableName,
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.tables t ON s.object_id = t.object_id
INNER JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE t.name IN ('Users', 'Trips', 'Bookings', 'Seats')
ORDER BY s.user_seeks + s.user_scans + s.user_lookups DESC;
```

## 🚀 Deployment

### Flutter App

1. **Build APK**:

   ```bash
   flutter build apk --release
   ```

2. **Build iOS**:

   ```bash
   flutter build ios --release
   ```

3. **Build Web**:
   ```bash
   flutter build web --release
   ```

### Backend API

1. **Docker**:

   ```dockerfile
   FROM mcr.microsoft.com/dotnet/aspnet:8.0
   COPY . /app
   WORKDIR /app
   EXPOSE 80
   ENTRYPOINT ["dotnet", "BusBookingAPI.dll"]
   ```

2. **Azure/AWS/GCP**: Deploy theo hướng dẫn của từng platform

## 📞 Hỗ trợ

Nếu gặp vấn đề:

1. **Kiểm tra logs** để xem lỗi cụ thể
2. **Đọc documentation** của Flutter và các packages
3. **Tạo issue** trong repository
4. **Liên hệ team** phát triển

## 📝 Ghi chú

- **Mock data**: Được sử dụng để test UI/UX khi chưa có backend
- **Database**: Cần được setup trước khi chạy với API thật
- **API**: Có thể sử dụng bất kỳ công nghệ nào (ASP.NET Core, Node.js, Python, etc.)
- **Security**: Nhớ cấu hình HTTPS và authentication khi deploy production

---

**Chúc bạn thành công! 🎉**
