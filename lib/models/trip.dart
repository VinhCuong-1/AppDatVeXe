import 'route.dart';

class Trip {
  final int tripId;
  final int routeId;
  final String busName;
  final String? driverName; // Tên tài xế
  final DateTime startTime;
  final int totalSeats;
  final int availableSeats; // Số ghế còn trống
  final String status;
  final Route? route;

  Trip({
    required this.tripId,
    required this.routeId,
    required this.busName,
    this.driverName,
    required this.startTime,
    required this.totalSeats,
    required this.availableSeats,
    required this.status,
    this.route,
  });

  factory Trip.fromJson(Map<String, dynamic> json) {
    return Trip(
      tripId: json['id'] ?? json['tripId'] ?? 0,
      routeId: json['routeId'] ?? json['RouteId'] ?? 0,
      busName: json['busName'] ?? json['BusName'] ?? 'Unknown Bus',
      driverName: json['driverName'] ?? json['DriverName'],
      startTime: DateTime.parse(json['startTime'] ?? json['StartTime'] ?? DateTime.now().toIso8601String()),
      totalSeats: json['totalSeats'] ?? json['TotalSeats'] ?? 0,
      availableSeats: json['availableSeats'] ?? json['AvailableSeats'] ?? json['totalSeats'] ?? json['TotalSeats'] ?? 0,
      status: json['status'] ?? json['Status'] ?? 'Unknown',
      route: json['route'] != null ? Route.fromJson(json['route']) : (json['Route'] != null ? Route.fromJson(json['Route']) : null),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'tripId': tripId,
      'routeId': routeId,
      'busName': busName,
      'driverName': driverName,
      'startTime': startTime.toIso8601String(),
      'totalSeats': totalSeats,
      'availableSeats': availableSeats,
      'status': status,
      'route': route?.toJson(),
    };
  }

  Trip copyWith({
    int? tripId,
    int? routeId,
    String? busName,
    String? driverName,
    DateTime? startTime,
    int? totalSeats,
    int? availableSeats,
    String? status,
    Route? route,
  }) {
    return Trip(
      tripId: tripId ?? this.tripId,
      routeId: routeId ?? this.routeId,
      busName: busName ?? this.busName,
      driverName: driverName ?? this.driverName,
      startTime: startTime ?? this.startTime,
      totalSeats: totalSeats ?? this.totalSeats,
      availableSeats: availableSeats ?? this.availableSeats,
      status: status ?? this.status,
      route: route ?? this.route,
    );
  }

  bool get isActive => status == TripStatus.active;
  bool get isPaused => status == TripStatus.paused;
  bool get isCancelled => status == TripStatus.cancelled;
}

class TripStatus {
  static const String active = 'Active';
  static const String paused = 'Paused';
  static const String cancelled = 'Cancelled';
}
