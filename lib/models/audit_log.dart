class AuditLog {
  final int logId;
  final int userId;
  final String action;
  final DateTime createdAt;
  final String description;

  AuditLog({
    required this.logId,
    required this.userId,
    required this.action,
    required this.createdAt,
    required this.description,
  });

  factory AuditLog.fromJson(Map<String, dynamic> json) {
    return AuditLog(
      logId: json['logId'],
      userId: json['userId'],
      action: json['action'],
      createdAt: DateTime.parse(json['createdAt']),
      description: json['description'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'logId': logId,
      'userId': userId,
      'action': action,
      'createdAt': createdAt.toIso8601String(),
      'description': description,
    };
  }
}

class AuditAction {
  static const String createBooking = 'CreateBooking';
  static const String cancelBooking = 'CancelBooking';
  static const String checkin = 'Checkin';
  static const String createTrip = 'CreateTrip';
  static const String updateTrip = 'UpdateTrip';
  static const String cancelTrip = 'CancelTrip';
}
