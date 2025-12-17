-- Script tạo database cho ứng dụng đặt vé xe
-- Chạy script này trên SQL Server để tạo database và các bảng

-- Tạo database
CREATE DATABASE BusBookingDB;
GO

USE BusBookingDB;
GO

-- Bảng Users
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Customer',
    PasswordHash NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Bảng Routes
CREATE TABLE Routes (
    RouteId INT IDENTITY(1,1) PRIMARY KEY,
    Departure NVARCHAR(100) NOT NULL,
    Destination NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Bảng Trips (Chuyến xe cụ thể)
CREATE TABLE Trips (
    TripId INT IDENTITY(1,1) PRIMARY KEY,
    RouteId INT NOT NULL,
    BusName NVARCHAR(100) NOT NULL,
    StartTime DATETIME NOT NULL,
    TotalSeats INT NOT NULL DEFAULT 40,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (RouteId) REFERENCES Routes(RouteId)
);
GO

-- Bảng Seats
CREATE TABLE Seats (
    SeatId INT IDENTITY(1,1) PRIMARY KEY,
    TripId INT NOT NULL,
    SeatNumber NVARCHAR(10) NOT NULL,
    IsBooked BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (TripId) REFERENCES Trips(TripId),
    UNIQUE(TripId, SeatNumber)
);
GO

-- Bảng Bookings
CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    TripId INT NOT NULL,
    UserId INT NOT NULL,
    SeatNumber NVARCHAR(10) NOT NULL,
    HolderName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Reserved',
    PaymentStatus NVARCHAR(20) NOT NULL DEFAULT 'Unpaid',
    QRToken NVARCHAR(200) NOT NULL UNIQUE,
    BookingTime DATETIME NOT NULL DEFAULT GETDATE(),
    ExpiresAt DATETIME NULL,
    FOREIGN KEY (TripId) REFERENCES Trips(TripId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- Bảng CheckinLogs
CREATE TABLE CheckinLogs (
    CheckinId INT IDENTITY(1,1) PRIMARY KEY,
    BookingId INT NOT NULL,
    StaffId INT NOT NULL,
    CheckinPoint NVARCHAR(50) NOT NULL,
    CheckinTime DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
    FOREIGN KEY (StaffId) REFERENCES Users(UserId)
);
GO

-- Bảng AuditLogs
CREATE TABLE AuditLogs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Description NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- Tạo indexes để tối ưu performance
CREATE INDEX IX_Users_Phone ON Users(Phone);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Trips_RouteId ON Trips(RouteId);
CREATE INDEX IX_Trips_StartTime ON Trips(StartTime);
CREATE INDEX IX_Seats_TripId ON Seats(TripId);
CREATE INDEX IX_Bookings_TripId ON Bookings(TripId);
CREATE INDEX IX_Bookings_UserId ON Bookings(UserId);
CREATE INDEX IX_Bookings_QRToken ON Bookings(QRToken);
CREATE INDEX IX_Bookings_Status ON Bookings(Status);
CREATE INDEX IX_CheckinLogs_BookingId ON CheckinLogs(BookingId);
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
CREATE INDEX IX_AuditLogs_CreatedAt ON AuditLogs(CreatedAt);
GO

-- Insert dữ liệu mẫu
-- Routes
INSERT INTO Routes (Departure, Destination) VALUES
('Hà Nội', 'Hồ Chí Minh'),
('Hà Nội', 'Đà Nẵng'),
('Hà Nội', 'Hải Phòng'),
('Hồ Chí Minh', 'Đà Lạt'),
('Hồ Chí Minh', 'Cần Thơ'),
('Đà Nẵng', 'Huế'),
('Hà Nội', 'Nghệ An');
GO

-- Users (admin và customer mẫu)
INSERT INTO Users (FullName, Phone, Email, Role, PasswordHash) VALUES
('Admin User', '0123456789', 'admin@example.com', 'Admin', 'hashed_password_123'),
('Staff User', '0987654321', 'staff@example.com', 'Staff', 'hashed_password_456'),
('Customer 1', '0111111111', 'customer1@example.com', 'Customer', 'hashed_password_789'),
('Customer 2', '0222222222', 'customer2@example.com', 'Customer', 'hashed_password_101');
GO

-- Trips mẫu
INSERT INTO Trips (RouteId, BusName, StartTime, TotalSeats) VALUES
(1, 'Xe Khách Phương Trang', '2024-01-15 08:00:00', 40),
(1, 'Xe Khách Hoàng Long', '2024-01-15 14:00:00', 40),
(1, 'Xe Khách Mai Linh', '2024-01-15 20:00:00', 40),
(2, 'Xe Khách Phương Trang', '2024-01-15 09:00:00', 40),
(2, 'Xe Khách Hoàng Long', '2024-01-15 15:00:00', 40),
(3, 'Xe Khách Mai Linh', '2024-01-15 10:00:00', 40);
GO

-- Tạo seats cho các trips
DECLARE @TripId INT;
DECLARE @SeatNumber NVARCHAR(10);
DECLARE @RowLetter CHAR(1);
DECLARE @SeatNum INT;

DECLARE trip_cursor CURSOR FOR 
SELECT TripId FROM Trips;

OPEN trip_cursor;
FETCH NEXT FROM trip_cursor INTO @TripId;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Tạo ghế từ A1 đến J4 (40 ghế)
    SET @RowLetter = 'A';
    WHILE @RowLetter <= 'J'
    BEGIN
        SET @SeatNum = 1;
        WHILE @SeatNum <= 4
        BEGIN
            SET @SeatNumber = @RowLetter + CAST(@SeatNum AS NVARCHAR(10));
            INSERT INTO Seats (TripId, SeatNumber) VALUES (@TripId, @SeatNumber);
            SET @SeatNum = @SeatNum + 1;
        END;
        SET @RowLetter = CHAR(ASCII(@RowLetter) + 1);
    END;
    
    FETCH NEXT FROM trip_cursor INTO @TripId;
END;

CLOSE trip_cursor;
DEALLOCATE trip_cursor;
GO

-- Stored Procedures
-- Procedure để tạo booking
CREATE PROCEDURE sp_CreateBooking
    @TripId INT,
    @UserId INT,
    @SeatNumber NVARCHAR(10),
    @HolderName NVARCHAR(100),
    @Phone NVARCHAR(20),
    @QRToken NVARCHAR(200),
    @BookingId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Kiểm tra ghế có trống không
        IF EXISTS (
            SELECT 1 FROM Bookings 
            WHERE TripId = @TripId 
            AND SeatNumber = @SeatNumber 
            AND Status IN ('Reserved', 'CheckedIn')
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Ghế đã được đặt', 16, 1);
            RETURN;
        END;
        
        -- Tạo booking
        INSERT INTO Bookings (TripId, UserId, SeatNumber, HolderName, Phone, QRToken, ExpiresAt)
        VALUES (@TripId, @UserId, @SeatNumber, @HolderName, @Phone, @QRToken, DATEADD(HOUR, 24, GETDATE()));
        
        SET @BookingId = SCOPE_IDENTITY();
        
        -- Cập nhật seat status
        UPDATE Seats SET IsBooked = 1 WHERE TripId = @TripId AND SeatNumber = @SeatNumber;
        
        -- Ghi audit log
        INSERT INTO AuditLogs (UserId, Action, Description)
        VALUES (@UserId, 'CreateBooking', 'Tạo vé mới - BookingId: ' + CAST(@BookingId AS NVARCHAR(10)));
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedure để check-in
CREATE PROCEDURE sp_CheckinBooking
    @QRToken NVARCHAR(200),
    @StaffId INT,
    @CheckinPoint NVARCHAR(50),
    @CheckinId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        DECLARE @BookingId INT;
        DECLARE @UserId INT;
        
        -- Lấy thông tin booking
        SELECT @BookingId = BookingId, @UserId = UserId
        FROM Bookings 
        WHERE QRToken = @QRToken AND Status = 'Reserved';
        
        IF @BookingId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Không tìm thấy vé hoặc vé không hợp lệ', 16, 1);
            RETURN;
        END;
        
        -- Cập nhật status booking
        UPDATE Bookings SET Status = 'CheckedIn' WHERE BookingId = @BookingId;
        
        -- Tạo checkin log
        INSERT INTO CheckinLogs (BookingId, StaffId, CheckinPoint)
        VALUES (@BookingId, @StaffId, @CheckinPoint);
        
        SET @CheckinId = SCOPE_IDENTITY();
        
        -- Ghi audit log
        INSERT INTO AuditLogs (UserId, Action, Description)
        VALUES (@StaffId, 'Checkin', 'Check-in vé - BookingId: ' + CAST(@BookingId AS NVARCHAR(10)));
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedure để hủy booking
CREATE PROCEDURE sp_CancelBooking
    @BookingId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        DECLARE @TripId INT;
        DECLARE @SeatNumber NVARCHAR(10);
        
        -- Lấy thông tin booking
        SELECT @TripId = TripId, @SeatNumber = SeatNumber
        FROM Bookings 
        WHERE BookingId = @BookingId AND UserId = @UserId AND Status = 'Reserved';
        
        IF @TripId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Không tìm thấy vé hoặc vé không thể hủy', 16, 1);
            RETURN;
        END;
        
        -- Cập nhật status booking
        UPDATE Bookings SET Status = 'Cancelled' WHERE BookingId = @BookingId;
        
        -- Cập nhật seat status
        UPDATE Seats SET IsBooked = 0 WHERE TripId = @TripId AND SeatNumber = @SeatNumber;
        
        -- Ghi audit log
        INSERT INTO AuditLogs (UserId, Action, Description)
        VALUES (@UserId, 'CancelBooking', 'Hủy vé - BookingId: ' + CAST(@BookingId AS NVARCHAR(10)));
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

PRINT 'Database BusBookingDB đã được tạo thành công!';
PRINT 'Đã tạo ' + CAST((SELECT COUNT(*) FROM Routes) AS NVARCHAR(10)) + ' tuyến đường mẫu';
PRINT 'Đã tạo ' + CAST((SELECT COUNT(*) FROM Trips) AS NVARCHAR(10)) + ' chuyến xe mẫu';
PRINT 'Đã tạo ' + CAST((SELECT COUNT(*) FROM Seats) AS NVARCHAR(10)) + ' ghế xe';
PRINT 'Đã tạo ' + CAST((SELECT COUNT(*) FROM Users) AS NVARCHAR(10)) + ' người dùng mẫu';
