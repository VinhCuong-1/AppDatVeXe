import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/models.dart';
import '../config/api_config.dart';
import './auth_service.dart';

class ApiService {
  static final String baseUrl = ApiConfig.resolveBaseUrl();
  static const Duration timeout = ApiConfig.timeout;

  // Helper method to handle HTTP requests
  static Future<dynamic> _makeRequest(
    String method,
    String endpoint, {
    Map<String, dynamic>? body,
    Map<String, String>? headers,
  }) async {
    final url = Uri.parse('$baseUrl$endpoint');
    final defaultHeaders = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };

    // Attach bearer token if available
    try {
      final token = await AuthService.getToken();
      if (token != null && token.isNotEmpty) {
        defaultHeaders['Authorization'] = 'Bearer $token';
      }
    } catch (_) {
      // ignore token retrieval errors
    }
    
    if (headers != null) {
      defaultHeaders.addAll(headers);
    }

    http.Response response;
    
    try {
      switch (method.toUpperCase()) {
        case 'GET':
          response = await http.get(url, headers: defaultHeaders).timeout(timeout);
          break;
        case 'POST':
          response = await http.post(
            url,
            headers: defaultHeaders,
            body: body != null ? jsonEncode(body) : null,
          ).timeout(timeout);
          break;
        case 'PUT':
          response = await http.put(
            url,
            headers: defaultHeaders,
            body: body != null ? jsonEncode(body) : null,
          ).timeout(timeout);
          break;
        case 'DELETE':
          response = await http.delete(url, headers: defaultHeaders).timeout(timeout);
          break;
        default:
          throw Exception('Unsupported HTTP method: $method');
      }

      // Debug logging
      print('API Response - Status: ${response.statusCode}');
      print('API Response - Body: ${response.body}');
      print('API Response - Headers: ${response.headers}');
      
      // If server returned an error status with empty body, throw with status
      if (response.body.isEmpty) {
        if (response.statusCode >= 400) {
          throw ApiException(
            message: response.statusCode == 401 ? 'Unauthorized' : 'Empty response from server',
            statusCode: response.statusCode,
          );
        }
        // For successful empty responses, return an empty map
        return {};
      }
      
      final responseData = jsonDecode(response.body);
      
      if (response.statusCode >= 200 && response.statusCode < 300) {
        return responseData;
      } else {
        throw ApiException(
          message: responseData['message'] ?? 'API Error',
          statusCode: response.statusCode,
        );
      }
    } catch (e) {
      if (e is ApiException) rethrow;
      throw ApiException(
        message: 'Network error: ${e.toString()}',
        statusCode: 0,
      );
    }
  }

  // Authentication APIs
  static Future<User> login(String phone, String password) async {
    if (ApiConfig.useMockData) {
      // Simulate network delay
      await Future.delayed(Duration(seconds: 1));
      final mockResponse = ApiConfig.getMockLoginResponse();
      return User.fromJson(mockResponse['user']);
    }
    
    final response = await _makeRequest('POST', '/auth/login', body: {
      'phone': phone,
      'password': password,
    });
    // Some backends return top-level { success, message, user, token }
    final userJson = response['user'] ?? response['data'] ?? response;
    final token = response['token'] ?? response['Token'];
    final user = User.fromJson(Map<String, dynamic>.from(userJson));
    try {
      await AuthService.saveUser(user);
      if (token is String && token.isNotEmpty) {
        await AuthService.saveToken(token);
      }
    } catch (_) {}
    return user;
  }

  // OTP Authentication
  static Future<Map<String, dynamic>> sendOtp(String phone) async {
    final response = await _makeRequest('POST', '/otp/send', body: {
      'phone': phone,
    });
    return response;
  }

  static Future<User> verifyOtp(String phone, String otp) async {
    final response = await _makeRequest('POST', '/otp/verify', body: {
      'phone': phone,
      'otp': otp,
    });
    
    final userJson = response['user'] ?? response['data']?['user'] ?? response;
    final token = response['token'] ?? response['data']?['token'];
    final user = User.fromJson(Map<String, dynamic>.from(userJson));
    
    try {
      await AuthService.saveUser(user);
      if (token is String && token.isNotEmpty) {
        await AuthService.saveToken(token);
      }
    } catch (_) {}
    return user;
  }

  // Reset Password
  static Future<Map<String, dynamic>> resetPassword({
    required String phone,
    required String otp,
    required String newPassword,
  }) async {
    // Gọi trực tiếp endpoint /auth/reset-password
    final response = await _makeRequest('POST', '/auth/reset-password', body: {
      'phone': phone,
      'otp': otp,
      'newPassword': newPassword,
    });
    return response;
  }

  static Future<User> register({
    required String fullName,
    required String phone,
    required String email,
    required String password,
  }) async {
    if (ApiConfig.useMockData) {
      // Simulate network delay
      await Future.delayed(Duration(seconds: 1));
      final mockResponse = ApiConfig.getMockLoginResponse();
      final userData = Map<String, dynamic>.from(mockResponse['user']);
      userData['fullName'] = fullName;
      userData['phone'] = phone;
      userData['email'] = email;
      return User.fromJson(userData);
    }
    
    final response = await _makeRequest('POST', '/auth/register', body: {
      'fullName': fullName,
      'phone': phone,
      'email': email,
      'password': password,
    });
    final userJson = response['user'] ?? response['data'] ?? response;
    final token = response['token'] ?? response['Token'];
    final user = User.fromJson(Map<String, dynamic>.from(userJson));
    try {
      await AuthService.saveUser(user);
      if (token is String && token.isNotEmpty) {
        await AuthService.saveToken(token);
      }
    } catch (_) {}
    return user;
  }

  static Future<void> logout() async {
    await AuthService.clearAuth();
  }

  // Trip search APIs
  static Future<List<Trip>> searchTrips({
    required String from,
    required String to,
    required DateTime date,
  }) async {
    if (ApiConfig.useMockData) {
      // Simulate network delay
      await Future.delayed(Duration(seconds: 1));
      final mockResponse = ApiConfig.getMockSearchResponse();
      return (mockResponse['trips'] as List)
          .map((trip) => Trip.fromJson(trip))
          .toList();
    }
    
    final response = await _makeRequest('POST', '/trips/search', body: {
      'from': from,
      'to': to,
      'date': date.toIso8601String(),
    });
    
    // Debug logging
    print('API Response: $response');
    
    // Backend may return { data: [...] } or { trips: [...] }
    final rawList = (response['trips'] ?? response['data']) as List?;
    print('Raw list: $rawList');
    
    if (rawList == null) {
      print('No trips found in response');
      return [];
    }
    
    final trips = rawList.map((trip) {
      print('Parsing trip: $trip');
      return Trip.fromJson(Map<String, dynamic>.from(trip));
    }).toList();
    
    print('Parsed ${trips.length} trips');
    
    // Remove duplicates based on trip ID
    final uniqueTrips = <int, Trip>{};
    for (var trip in trips) {
      uniqueTrips[trip.tripId] = trip;
    }
    
    final result = uniqueTrips.values.toList();
    print('After removing duplicates: ${result.length} trips');
    return result;
  }

  static Future<Trip> getTripDetails(int tripId) async {
    final response = await _makeRequest('GET', '/trips/$tripId');
    
    // Debug logging
    print('GetTripDetails response: $response');
    
    // Backend returns { success: true, message: "...", data: { ... trip data ... } }
    final tripData = response['data'];
    if (tripData == null) {
      throw Exception('Trip not found');
    }
    
    return Trip.fromJson(tripData);
  }

  static Future<List<Seat>> getTripSeats(int tripId) async {
    if (ApiConfig.useMockData) {
      // Simulate network delay
      await Future.delayed(Duration(seconds: 1));
      final mockResponse = ApiConfig.getMockSeatsResponse();
      return (mockResponse['seats'] as List)
          .map((seat) => Seat.fromJson(seat))
          .toList();
    }
    
    final response = await _makeRequest('GET', '/trips/$tripId/seats');
    
    // Debug logging
    print('GetTripSeats response: $response');
    
    // Backend returns { success: true, message: "...", data: [ ... seats ... ] }
    final seatsData = response['data'] as List?;
    if (seatsData == null) {
      return [];
    }
    
    return seatsData.map((seat) => Seat.fromJson(seat)).toList();
  }

  // Booking APIs
  static Future<Booking> createBooking({
    required int tripId,
    required int userId,
    required String seatNumber,
    required String holderName,
    required String phone,
    String? pickupPoint,
  }) async {
    if (ApiConfig.useMockData) {
      // Simulate network delay
      await Future.delayed(Duration(seconds: 2));
      final mockResponse = ApiConfig.getMockBookingResponse();
      final bookingData = Map<String, dynamic>.from(mockResponse['booking']);
      bookingData['tripId'] = tripId;
      bookingData['userId'] = userId;
      bookingData['seatNumber'] = seatNumber;
      bookingData['holderName'] = holderName;
      bookingData['phone'] = phone;
      bookingData['pickupPoint'] = pickupPoint;
      bookingData['qrToken'] = 'BOOK-${DateTime.now().millisecondsSinceEpoch}-$userId';
      return Booking.fromJson(bookingData);
    }
    
    final body = {
      'tripId': tripId,
      'seatNumber': seatNumber,
      'holderName': holderName,
      'phone': phone,
    };
    
    if (pickupPoint != null) {
      body['pickupPoint'] = pickupPoint;
    }
    
    final response = await _makeRequest('POST', '/bookings/create', body: body);
    final bookingJson = response['data'] ?? response['booking'] ?? response;
    return Booking.fromJson(Map<String, dynamic>.from(bookingJson));
  }

  static Future<Booking> getBooking(int bookingId) async {
    final response = await _makeRequest('GET', '/bookings/$bookingId');
    final bookingJson = response['data'] ?? response['booking'] ?? response;
    return Booking.fromJson(Map<String, dynamic>.from(bookingJson));
  }

  static Future<List<Booking>> getUserBookings(String userId) async {
    final response = await _makeRequest('GET', '/bookings/user/$userId');
    final list = (response['data'] ?? response['bookings'] ?? []) as List;
    return list.map((booking) => Booking.fromJson(Map<String, dynamic>.from(booking))).toList();
  }

  static Future<void> cancelBooking(int bookingId) async {
    await _makeRequest('DELETE', '/bookings/$bookingId');
  }

  // Admin APIs
  static Future<List<Booking>> searchBookingsByPhone(String phone) async {
    final response = await _makeRequest('GET', '/admin/bookings?phone=$phone');
    final list = (response['data'] ?? response['bookings'] ?? []) as List;
    return list.map((booking) => Booking.fromJson(Map<String, dynamic>.from(booking))).toList();
  }

  // Check-in APIs
  static Future<CheckinLog> checkinBooking({
    required String qrToken,
    required String staffId,
    required String checkinPoint,
  }) async {
    final response = await _makeRequest('POST', '/checkin', body: {
      'qrToken': qrToken,
      'staffId': staffId,
      'checkinPoint': checkinPoint,
    });
    
    // Backend returns { success: true, message: "...", data: { ... checkinLog ... } }
    final checkinLogData = response['data'] ?? response['checkinLog'] ?? response;
    return CheckinLog.fromJson(checkinLogData);
  }

  static Future<Booking> verifyBooking(String qrToken) async {
    final response = await _makeRequest('GET', '/checkin/verify?token=$qrToken');
    final bookingData = response['data'] ?? response['booking'] ?? response;
    return Booking.fromJson(bookingData);
  }

  // Admin APIs
  // Get unique trip templates (no date, just hour/route/bus patterns)
  static Future<List<Trip>> getTripTemplates() async {
    final response = await _makeRequest('GET', '/admin/trips');
    final tripsData = response['data'] ?? response['trips'] ?? [];
    return (tripsData as List).map((trip) => Trip.fromJson(trip)).toList();
  }

  // Get today's trips (for user search)
  static Future<List<Trip>> getTodayTrips() async {
    final response = await _makeRequest('GET', '/admin/trips');
    final tripsData = response['data'] ?? response['trips'] ?? [];
    final allTrips = (tripsData as List)
        .map((trip) => Trip.fromJson(trip))
        .toList();
    
    // Filter trips for today and tomorrow
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final tomorrow = today.add(const Duration(days: 2)); // Next 2 days
    
    return allTrips.where((trip) {
      final tripDate = DateTime(trip.startTime.year, trip.startTime.month, trip.startTime.day);
      return tripDate.isAfter(today.subtract(const Duration(days: 1))) && 
             tripDate.isBefore(tomorrow);
    }).toList();
  }

  static Future<List<Trip>> getAllTrips() async {
    final response = await _makeRequest('GET', '/admin/trips');
    final tripsData = response['data'] ?? response['trips'] ?? [];
    return (tripsData as List)
        .map((trip) => Trip.fromJson(trip))
        .toList();
  }

  static Future<List<Booking>> getAdminBookings({
    int? tripId,
    String? status,
  }) async {
    final queryParams = <String, String>{};
    if (tripId != null) queryParams['tripId'] = tripId.toString();
    if (status != null) queryParams['status'] = status;
    
    final queryString = queryParams.isNotEmpty 
        ? '?${queryParams.entries.map((e) => '${e.key}=${e.value}').join('&')}'
        : '';
    
    final response = await _makeRequest('GET', '/admin/bookings$queryString');
    final bookingsData = response['data'] ?? response['bookings'] ?? [];
    return (bookingsData as List)
        .map((booking) => Booking.fromJson(booking))
        .toList();
  }

  static Future<Trip> createTrip({
    required int routeId,
    required String busName,
    String? driverName,
    required DateTime startTime,
    required int totalSeats,
  }) async {
    final body = {
      'routeId': routeId,
      'busName': busName,
      'startTime': startTime.toIso8601String(),
      'totalSeats': totalSeats,
    };
    
    if (driverName != null && driverName.isNotEmpty) {
      body['driverName'] = driverName;
    }
    
    final response = await _makeRequest('POST', '/admin/trips', body: body);
    final tripData = response['data'] ?? response['trip'] ?? response;
    return Trip.fromJson(tripData);
  }

  // Update trip
  static Future<Trip> updateTrip({
    required int tripId,
    required int routeId,
    required String busName,
    String? driverName,
    required DateTime startTime,
    required int totalSeats,
  }) async {
    final body = {
      'routeId': routeId,
      'busName': busName,
      'driverName': driverName,
      'startTime': startTime.toIso8601String(),
      'totalSeats': totalSeats,
    };
    
    final response = await _makeRequest('PUT', '/admin/trips/$tripId', body: body);
    final tripJson = response['data'] ?? response['trip'] ?? response;
    return Trip.fromJson(Map<String, dynamic>.from(tripJson));
  }

  // Pause trip
  static Future<void> pauseTrip(int tripId) async {
    await _makeRequest('PUT', '/admin/trips/$tripId/pause');
  }

  // Resume trip
  static Future<void> resumeTrip(int tripId) async {
    await _makeRequest('PUT', '/admin/trips/$tripId/resume');
  }

  // Delete trip (hard delete)
  static Future<void> deleteTrip(int tripId) async {
    await _makeRequest('DELETE', '/admin/trips/$tripId/delete');
  }

  // Routes APIs
  static Future<List<Route>> getRoutes() async {
    final response = await _makeRequest('GET', '/admin/trips'); // Get trips to extract unique routes
    final tripsData = response['data'] ?? response['trips'] ?? [];
    final trips = (tripsData as List).map((trip) => Trip.fromJson(trip)).toList();
    
    // Extract unique routes from trips
    final routesMap = <int, Route>{};
    for (var trip in trips) {
      if (trip.route != null) {
        routesMap[trip.route!.routeId] = trip.route!;
      }
    }
    
    return routesMap.values.toList();
  }

  // User Management APIs
  static Future<List<User>> getAllUsers() async {
    try {
      final response = await _makeRequest('GET', '/users');
      // Backend returns a list directly
      if (response is List) {
        return (response as List).map((user) => User.fromJson(user as Map<String, dynamic>)).toList();
      }
      // Or wrapped in data field
      final usersData = response['data'] ?? response;
      if (usersData is List) {
        return (usersData as List).map((user) => User.fromJson(user as Map<String, dynamic>)).toList();
      }
      return [];
    } catch (e) {
      throw Exception('Không thể tải danh sách người dùng: $e');
    }
  }

  static Future<User> getUserById(String userId) async {
    try {
      final response = await _makeRequest('GET', '/users/$userId');
      return User.fromJson(response);
    } catch (e) {
      throw Exception('Không thể tải thông tin người dùng: $e');
    }
  }

  static Future<void> createUser(
    String fullName,
    String email,
    String phone,
    String password,
    String role,
  ) async {
    try {
      await _makeRequest('POST', '/users', body: {
        'fullName': fullName,
        'email': email,
        'phone': phone,
        'password': password,
        'role': role,
      });
    } catch (e) {
      throw Exception('Không thể tạo người dùng: $e');
    }
  }

  static Future<void> updateUser(
    String userId,
    String fullName,
    String email,
    String phone,
    String? password,
    String role,
  ) async {
    try {
      final body = {
        'fullName': fullName,
        'email': email,
        'phone': phone,
        'role': role,
      };
      
      if (password != null && password.isNotEmpty) {
        body['password'] = password;
      }
      
      await _makeRequest('PUT', '/users/$userId', body: body);
    } catch (e) {
      throw Exception('Không thể cập nhật người dùng: $e');
    }
  }

  static Future<void> deleteUser(String userId) async {
    try {
      await _makeRequest('DELETE', '/users/$userId');
    } catch (e) {
      throw Exception('Không thể xóa người dùng: $e');
    }
  }

  static Future<void> toggleUserStatus(String userId) async {
    try {
      await _makeRequest('PUT', '/users/$userId/toggle-status');
    } catch (e) {
      throw Exception('Không thể thay đổi trạng thái người dùng: $e');
    }
  }
}

class ApiException implements Exception {
  final String message;
  final int statusCode;

  ApiException({required this.message, required this.statusCode});

  @override
  String toString() => 'ApiException: $message (Status: $statusCode)';
}
