class Route {
  final int routeId;
  final String departure;
  final String destination;

  Route({
    required this.routeId,
    required this.departure,
    required this.destination,
  });

  factory Route.fromJson(Map<String, dynamic> json) {
    return Route(
      routeId: json['id'] ?? json['routeId'] ?? 0,
      departure: json['departure'] ?? json['Departure'] ?? 'Unknown',
      destination: json['destination'] ?? json['Destination'] ?? 'Unknown',
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'routeId': routeId,
      'departure': departure,
      'destination': destination,
    };
  }

  Route copyWith({
    int? routeId,
    String? departure,
    String? destination,
  }) {
    return Route(
      routeId: routeId ?? this.routeId,
      departure: departure ?? this.departure,
      destination: destination ?? this.destination,
    );
  }

  String get displayName => '$departure - $destination';
}
