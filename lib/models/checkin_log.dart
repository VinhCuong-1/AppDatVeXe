class CheckinLog {
  final int checkinId;
  final int bookingId;
  final String staffId; // Changed to String to match backend
  final String checkinPoint;
  final DateTime checkinTime;

  CheckinLog({
    required this.checkinId,
    required this.bookingId,
    required this.staffId,
    required this.checkinPoint,
    required this.checkinTime,
  });

  factory CheckinLog.fromJson(Map<String, dynamic> json) {
    return CheckinLog(
      checkinId: json['id'] ?? json['checkinId'], // Backend uses 'id'
      bookingId: json['bookingId'],
      staffId: json['staffId'].toString(), // Convert to String
      checkinPoint: json['checkinPoint'],
      checkinTime: DateTime.parse(json['checkinTime']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': checkinId,
      'bookingId': bookingId,
      'staffId': staffId,
      'checkinPoint': checkinPoint,
      'checkinTime': checkinTime.toIso8601String(),
    };
  }
}

class CheckinPoint {
  static const String onBus = 'OnBus';
  static const String atStation = 'AtStation';
}
