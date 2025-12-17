import 'trip.dart';

class Booking {
  final int bookingId;
  final int tripId;
  final String userId; // Backend returns GUID string
  final String seatNumber;
  final String holderName;
  final String phone;
  final String status;
  final String paymentStatus;
  final String qrToken;
  final DateTime bookingTime;
  final DateTime? expiresAt;
  final Trip? trip;
  final String? pickupPoint; // Nơi đón: "Dọc tuyến đường" hoặc "Bến xe miền đông"

  Booking({
    required this.bookingId,
    required this.tripId,
    required this.userId,
    required this.seatNumber,
    required this.holderName,
    required this.phone,
    required this.status,
    required this.paymentStatus,
    required this.qrToken,
    required this.bookingTime,
    this.expiresAt,
    this.trip,
    this.pickupPoint,
  });

  factory Booking.fromJson(Map<String, dynamic> json) {
    return Booking(
      bookingId: json['id'] ?? json['bookingId'] ?? 0,
      tripId: json['tripId'] ?? 0,
      userId: (json['userId'] ?? '').toString(),
      seatNumber: json['seatNumber'],
      holderName: json['holderName'],
      phone: json['phone'],
      status: json['status'],
      paymentStatus: json['paymentStatus'],
      qrToken: json['qrToken'],
      bookingTime: DateTime.parse(json['bookingTime']),
      expiresAt: json['expiresAt'] != null ? DateTime.parse(json['expiresAt']) : null,
      trip: json['trip'] != null ? Trip.fromJson(json['trip']) : null,
      pickupPoint: json['pickupPoint'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'bookingId': bookingId,
      'tripId': tripId,
      'userId': userId,
      'seatNumber': seatNumber,
      'holderName': holderName,
      'phone': phone,
      'status': status,
      'paymentStatus': paymentStatus,
      'qrToken': qrToken,
      'bookingTime': bookingTime.toIso8601String(),
      'expiresAt': expiresAt?.toIso8601String(),
      'trip': trip?.toJson(),
      'pickupPoint': pickupPoint,
    };
  }

  Booking copyWith({
    int? bookingId,
    int? tripId,
    String? userId,
    String? seatNumber,
    String? holderName,
    String? phone,
    String? status,
    String? paymentStatus,
    String? qrToken,
    DateTime? bookingTime,
    DateTime? expiresAt,
    Trip? trip,
    String? pickupPoint,
  }) {
    return Booking(
      bookingId: bookingId ?? this.bookingId,
      tripId: tripId ?? this.tripId,
      userId: userId ?? this.userId,
      seatNumber: seatNumber ?? this.seatNumber,
      holderName: holderName ?? this.holderName,
      phone: phone ?? this.phone,
      status: status ?? this.status,
      paymentStatus: paymentStatus ?? this.paymentStatus,
      qrToken: qrToken ?? this.qrToken,
      bookingTime: bookingTime ?? this.bookingTime,
      expiresAt: expiresAt ?? this.expiresAt,
      trip: trip ?? this.trip,
      pickupPoint: pickupPoint ?? this.pickupPoint,
    );
  }

  bool get isReserved => status == BookingStatus.reserved;
  bool get isCancelled => status == BookingStatus.cancelled;
  bool get isCheckedIn => status == BookingStatus.checkedIn;
  bool get isPaid => paymentStatus == PaymentStatus.paid;
  bool get isUnpaid => paymentStatus == PaymentStatus.unpaid;
  bool get isExpired => expiresAt != null && DateTime.now().isAfter(expiresAt!);
}

class BookingStatus {
  static const String reserved = 'Reserved';
  static const String cancelled = 'Cancelled';
  static const String checkedIn = 'CheckedIn';
}

class PaymentStatus {
  static const String unpaid = 'Unpaid';
  static const String paid = 'Paid';
}
