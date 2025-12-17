import 'package:flutter/material.dart';
import '../models/route.dart' as route_model;
import '../models/trip.dart';
import '../models/booking.dart';
import '../models/checkin_log.dart';
import '../services/api_service.dart';
import '../services/auth_service.dart';

class AdminProvider extends ChangeNotifier {
  List<Trip> _todayTrips = [];
  List<Booking> _bookings = [];
  List<route_model.Route> _routes = [];
  bool _isLoading = false;
  String? _error;

  // Getters
  List<Trip> get todayTrips => _todayTrips;
  List<Booking> get bookings => _bookings;
  List<route_model.Route> get routes => _routes;
  bool get isLoading => _isLoading;
  String? get error => _error;

  // Load trip templates (unique trips by hour/route/bus)
  Future<void> loadTodayTrips() async {
    _setLoading(true);
    _clearError();
    
    try {
      _todayTrips = await ApiService.getTripTemplates();
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Load bookings
  Future<void> loadBookings({int? tripId, String? status}) async {
    _setLoading(true);
    _clearError();
    
    try {
      _bookings = await ApiService.getAdminBookings(
        tripId: tripId,
        status: status,
      );
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Load routes
  Future<void> loadRoutes() async {
    _setLoading(true);
    _clearError();
    
    try {
      _routes = await ApiService.getRoutes();
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Create trip
  Future<Trip?> createTrip({
    required int routeId,
    required String busName,
    String? driverName,
    required DateTime startTime,
    required int totalSeats,
  }) async {
    _setLoading(true);
    _clearError();
    
    try {
      final trip = await ApiService.createTrip(
        routeId: routeId,
        busName: busName,
        driverName: driverName,
        startTime: startTime,
        totalSeats: totalSeats,
      );
      
      _todayTrips.add(trip);
      notifyListeners();
      return trip;
    } catch (e) {
      _setError(e.toString());
      return null;
    } finally {
      _setLoading(false);
    }
  }

  // Update trip
  Future<bool> updateTrip({
    required int tripId,
    required int routeId,
    required String busName,
    String? driverName,
    required DateTime startTime,
    required int totalSeats,
  }) async {
    _setLoading(true);
    _clearError();
    
    try {
      final trip = await ApiService.updateTrip(
        tripId: tripId,
        routeId: routeId,
        busName: busName,
        driverName: driverName,
        startTime: startTime,
        totalSeats: totalSeats,
      );
      
      // Update in local list
      final index = _todayTrips.indexWhere((t) => t.tripId == tripId);
      if (index != -1) {
        _todayTrips[index] = trip;
      }
      
      notifyListeners();
      return true;
    } catch (e) {
      _setError(e.toString());
      return false;
    } finally {
      _setLoading(false);
    }
  }

  // Pause trip
  Future<void> pauseTrip(int tripId) async {
    _setLoading(true);
    _clearError();
    
    try {
      await ApiService.pauseTrip(tripId);
      
      // Update in local list
      final index = _todayTrips.indexWhere((t) => t.tripId == tripId);
      if (index != -1) {
        _todayTrips[index] = _todayTrips[index].copyWith(
          status: TripStatus.paused,
        );
      }
      
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Resume trip
  Future<void> resumeTrip(int tripId) async {
    _setLoading(true);
    _clearError();
    
    try {
      await ApiService.resumeTrip(tripId);
      
      // Update in local list
      final index = _todayTrips.indexWhere((t) => t.tripId == tripId);
      if (index != -1) {
        _todayTrips[index] = _todayTrips[index].copyWith(
          status: TripStatus.active,
        );
      }
      
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Delete trip (hard delete)
  Future<void> deleteTrip(int tripId) async {
    _setLoading(true);
    _clearError();
    
    try {
      await ApiService.deleteTrip(tripId);
      
      // Remove from local list
      _todayTrips.removeWhere((t) => t.tripId == tripId);
      
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Check-in booking (unified method for both QR scan and manual checkin)
  Future<bool> checkinBooking({
    required int bookingId,
    required String qrToken,
  }) async {
    _setLoading(true);
    _clearError();
    
    try {
      final user = await AuthService.getCurrentUser();
      if (user == null) {
        _setError('Chưa đăng nhập');
        return false;
      }

      final checkinLog = await ApiService.checkinBooking(
        qrToken: qrToken,
        staffId: user.userId,
        checkinPoint: 'Mobile App',
      );
      
      // Update booking status in local list
      final bookingIndex = _bookings.indexWhere((b) => b.bookingId == bookingId);
      if (bookingIndex != -1) {
        _bookings[bookingIndex] = _bookings[bookingIndex].copyWith(
          status: BookingStatus.checkedIn,
        );
      }
      
      notifyListeners();
      return checkinLog != null;
    } catch (e) {
      _setError(e.toString());
      return false;
    } finally {
      _setLoading(false);
    }
  }

  // Verify booking by QR token
  Future<Booking?> verifyBooking(String qrToken) async {
    _setLoading(true);
    _clearError();
    
    try {
      final booking = await ApiService.verifyBooking(qrToken);
      notifyListeners();
      return booking;
    } catch (e) {
      _setError(e.toString());
      return null;
    } finally {
      _setLoading(false);
    }
  }

  // Helper methods
  void _setLoading(bool loading) {
    _isLoading = loading;
    notifyListeners();
  }

  void _setError(String error) {
    _error = error;
    notifyListeners();
  }

  void _clearError() {
    _error = null;
  }
}
