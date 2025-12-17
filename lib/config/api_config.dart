import 'package:flutter/foundation.dart';

class ApiConfig {
  // Thay đổi URL này thành URL API thực tế của bạn (production)
  static const String baseUrl = 'https://your-api-domain.com/api';
  
  // Địa chỉ khi chạy local
  static const String localhostUrlWeb = 'http://127.0.0.1:5264/api';
  static const String localhostUrlDesktop = 'http://localhost:5264/api';
  static const String localhostUrlAndroidEmulator = 'http://10.0.2.2:5264/api';
  // Địa chỉ IP của máy tính trong mạng LAN (cho điện thoại thật)
  static const String localhostUrlPhysicalDevice = 'http://10.110.33.77:5264/api';

  // Tự động chọn base URL theo nền tảng (web / android emulator / desktop)
  static String resolveBaseUrl() {
    if (kIsWeb) return localhostUrlWeb;
    switch (defaultTargetPlatform) {
      case TargetPlatform.android:
        // THAY ĐỔI: Sử dụng IP của máy tính thay vì 10.0.2.2
        // Nếu chạy trên emulator, đổi lại thành localhostUrlAndroidEmulator
        return localhostUrlPhysicalDevice;
      case TargetPlatform.iOS:
      case TargetPlatform.macOS:
      case TargetPlatform.windows:
      case TargetPlatform.linux:
      case TargetPlatform.fuchsia:
        return localhostUrlDesktop;
    }
  }
  
  // Timeout cho các request
  static const Duration timeout = Duration(seconds: 30);
  
  // Headers mặc định
  static const Map<String, String> defaultHeaders = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  };
  
  // Các endpoint chính
  static const String loginEndpoint = '/auth/login';
  static const String registerEndpoint = '/auth/register';
  static const String searchTripsEndpoint = '/trips/search';
  static const String tripDetailsEndpoint = '/trips';
  static const String tripSeatsEndpoint = '/trips/seats';
  static const String createBookingEndpoint = '/bookings/create';
  static const String bookingDetailsEndpoint = '/bookings';
  static const String userBookingsEndpoint = '/bookings/user';
  static const String cancelBookingEndpoint = '/bookings';
  static const String checkinEndpoint = '/checkin';
  static const String verifyBookingEndpoint = '/checkin/verify';
  static const String adminTripsEndpoint = '/admin/trips';
  static const String adminBookingsEndpoint = '/admin/bookings';
  static const String routesEndpoint = '/routes';
  
  // Mock data cho testing (khi chưa có backend)
  static const bool useMockData = false;
  
  // Mock responses
  static Map<String, dynamic> getMockLoginResponse() {
    return {
      'success': true,
      'user': {
        'userId': 1,
        'fullName': 'Test User',
        'phone': '0123456789',
        'email': 'test@example.com',
        'role': 'Customer',
        'createdAt': '2024-01-01T00:00:00Z'
      },
      'token': 'mock_jwt_token_123'
    };
  }
  
  static Map<String, dynamic> getMockSearchResponse() {
    return {
      'success': true,
      'trips': [
        {
          'tripId': 1,
          'routeId': 1,
          'busName': 'Xe Khách Phương Trang',
          'startTime': '2024-01-15T08:00:00Z',
          'totalSeats': 40,
          'status': 'Active',
          'route': {
            'routeId': 1,
            'departure': 'Hà Nội',
            'destination': 'Hồ Chí Minh'
          }
        },
        {
          'tripId': 2,
          'routeId': 1,
          'busName': 'Xe Khách Hoàng Long',
          'startTime': '2024-01-15T14:00:00Z',
          'totalSeats': 40,
          'status': 'Active',
          'route': {
            'routeId': 1,
            'departure': 'Hà Nội',
            'destination': 'Hồ Chí Minh'
          }
        }
      ]
    };
  }
  
  static Map<String, dynamic> getMockSeatsResponse() {
    return {
      'success': true,
      'seats': List.generate(40, (index) {
        final row = String.fromCharCode(65 + (index ~/ 4)); // A-J
        final seatNum = (index % 4) + 1;
        return {
          'seatId': index + 1,
          'tripId': 1,
          'seatNumber': '$row$seatNum',
          'isBooked': index % 5 == 0 // Mock: mỗi ghế thứ 5 bị đặt
        };
      })
    };
  }
  
  static Map<String, dynamic> getMockBookingResponse() {
    return {
      'success': true,
      'booking': {
        'bookingId': 1,
        'tripId': 1,
        'userId': 1,
        'seatNumber': 'A1',
        'holderName': 'Test User',
        'phone': '0123456789',
        'status': 'Reserved',
        'paymentStatus': 'Unpaid',
        'qrToken': 'BOOK-1-mock-uuid-123',
        'bookingTime': DateTime.now().toIso8601String(),
        'expiresAt': DateTime.now().add(Duration(hours: 24)).toIso8601String(),
        'trip': {
          'tripId': 1,
          'busName': 'Xe Khách Phương Trang',
          'startTime': '2024-01-15T08:00:00Z',
          'route': {
            'departure': 'Hà Nội',
            'destination': 'Hồ Chí Minh'
          }
        }
      }
    };
  }
}
