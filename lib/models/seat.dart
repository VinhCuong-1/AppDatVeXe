class Seat {
  final int seatId;
  final int tripId;
  final String seatNumber;
  final bool isBooked;

  Seat({
    required this.seatId,
    required this.tripId,
    required this.seatNumber,
    required this.isBooked,
  });

  factory Seat.fromJson(Map<String, dynamic> json) {
    return Seat(
      seatId: json['id'] ?? json['seatId'] ?? 0,
      tripId: json['tripId'] ?? 0,
      seatNumber: json['seatNumber'] ?? '',
      isBooked: json['isBooked'] ?? false,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'seatId': seatId,
      'tripId': tripId,
      'seatNumber': seatNumber,
      'isBooked': isBooked,
    };
  }

  Seat copyWith({
    int? seatId,
    int? tripId,
    String? seatNumber,
    bool? isBooked,
  }) {
    return Seat(
      seatId: seatId ?? this.seatId,
      tripId: tripId ?? this.tripId,
      seatNumber: seatNumber ?? this.seatNumber,
      isBooked: isBooked ?? this.isBooked,
    );
  }

  bool get isAvailable => !isBooked;
}
