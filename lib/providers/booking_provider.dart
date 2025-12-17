import 'package:flutter/material.dart';
import '../models/models.dart';
import '../models/time_filter.dart';
import '../services/api_service.dart';
import '../services/auth_service.dart';

class BookingProvider extends ChangeNotifier {
  List<Trip> _searchResults = [];
  List<Seat> _availableSeats = [];
  Trip? _selectedTrip;
  Seat? _selectedSeat;
  List<Booking> _userBookings = [];
  bool _isLoading = false;
  String? _error;

  // Getters
  List<Trip> get searchResults => _searchResults;
  List<Seat> get availableSeats => _availableSeats;
  Trip? get selectedTrip => _selectedTrip;
  Seat? get selectedSeat => _selectedSeat;
  List<Booking> get userBookings => _userBookings;
  bool get isLoading => _isLoading;
  String? get error => _error;

  // Search trips
  Future<void> searchTrips({
    required String from,
    required String to,
    required DateTime date,
    TimeFilter? timeFilter,
  }) async {
    _setLoading(true);
    _clearError();
    
    try {
      _searchResults = await ApiService.searchTrips(
        from: from,
        to: to,
        date: date,
      );
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Get trip details and seats
  Future<void> selectTrip(int tripId) async {
    _setLoading(true);
    _clearError();
    
    try {
      print('BookingProvider.selectTrip - Getting trip details for ID: $tripId');
      _selectedTrip = await ApiService.getTripDetails(tripId);
      print('BookingProvider.selectTrip - Trip details loaded: ${_selectedTrip?.busName}');
      
      print('BookingProvider.selectTrip - Getting seats for trip ID: $tripId');
      _availableSeats = await ApiService.getTripSeats(tripId);
      print('BookingProvider.selectTrip - Seats loaded: ${_availableSeats.length} seats');
      
      notifyListeners();
    } catch (e) {
      print('BookingProvider.selectTrip - Error: $e');
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Select seat
  void selectSeat(Seat seat) {
    if (seat.isAvailable) {
      _selectedSeat = seat;
      notifyListeners();
    }
  }

  // Create booking
  Future<Booking?> createBooking({
    required String holderName,
    required String phone,
    String? pickupPoint,
  }) async {
    if (_selectedTrip == null || _selectedSeat == null) {
      _setError('Vui lòng chọn chuyến và ghế');
      return null;
    }

    _setLoading(true);
    _clearError();
    
    try {
      // Get current user ID from AuthService
      final user = await AuthService.getCurrentUser();
      if (user == null) {
        _setError('Vui lòng đăng nhập');
        return null;
      }

      final booking = await ApiService.createBooking(
        tripId: _selectedTrip!.tripId,
        userId: int.tryParse(user.userId) ?? 0,
        seatNumber: _selectedSeat!.seatNumber,
        holderName: holderName,
        phone: phone,
        pickupPoint: pickupPoint,
      );

      // Add to user bookings
      _userBookings.insert(0, booking);
      
      // Update seat status
      final seatIndex = _availableSeats.indexWhere(
        (s) => s.seatId == _selectedSeat!.seatId,
      );
      if (seatIndex != -1) {
        _availableSeats[seatIndex] = _availableSeats[seatIndex].copyWith(
          isBooked: true,
        );
      }

      notifyListeners();
      return booking;
    } catch (e) {
      _setError(e.toString());
      return null;
    } finally {
      _setLoading(false);
    }
  }

  // Get user bookings
  Future<void> loadUserBookings() async {
    final user = await AuthService.getCurrentUser();
    if (user == null) return;

    _setLoading(true);
    _clearError();
    
    try {
      _userBookings = await ApiService.getUserBookings(user.userId);
      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Cancel booking
  Future<void> cancelBooking(int bookingId) async {
    _setLoading(true);
    _clearError();
    
    try {
      await ApiService.cancelBooking(bookingId);
      
      // Update local booking status
      final bookingIndex = _userBookings.indexWhere(
        (b) => b.bookingId == bookingId,
      );
      if (bookingIndex != -1) {
        _userBookings[bookingIndex] = _userBookings[bookingIndex].copyWith(
          status: BookingStatus.cancelled,
        );
      }

      notifyListeners();
    } catch (e) {
      _setError(e.toString());
    } finally {
      _setLoading(false);
    }
  }

  // Clear selections
  void clearSelections() {
    _selectedTrip = null;
    _selectedSeat = null;
    _availableSeats = [];
    notifyListeners();
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
