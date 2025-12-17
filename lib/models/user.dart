class User {
  final String userId; // GUID string from backend
  final String fullName;
  final String phone;
  final String email;
  final String role;
  final bool isActive;
  final DateTime createdAt;

  User({
    required this.userId,
    required this.fullName,
    required this.phone,
    required this.email,
    required this.role,
    this.isActive = true,
    required this.createdAt,
  });

  factory User.fromJson(Map<String, dynamic> json) {
    final String userId = (json['id'] ?? json['userId'] ?? '').toString();
    final String? createdAtStr = json['createdAt'] as String?;

    return User(
      userId: userId,
      fullName: json['fullName'] ?? '',
      phone: json['phone'] ?? '',
      email: json['email'] ?? '',
      role: json['role'] ?? '',
      isActive: json['isActive'] ?? true,
      createdAt:
          createdAtStr != null ? DateTime.parse(createdAtStr) : DateTime.now(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'userId': userId,
      'fullName': fullName,
      'phone': phone,
      'email': email,
      'role': role,
      'isActive': isActive,
      'createdAt': createdAt.toIso8601String(),
    };
  }

  User copyWith({
    String? userId,
    String? fullName,
    String? phone,
    String? email,
    String? role,
    bool? isActive,
    DateTime? createdAt,
  }) {
    return User(
      userId: userId ?? this.userId,
      fullName: fullName ?? this.fullName,
      phone: phone ?? this.phone,
      email: email ?? this.email,
      role: role ?? this.role,
      isActive: isActive ?? this.isActive,
      createdAt: createdAt ?? this.createdAt,
    );
  }
}

class UserRole {
  static const String customer = 'Customer';
  static const String admin = 'Admin';
}
