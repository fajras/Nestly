class AppUserRow {
  final int id;
  final String email;
  final String firstName;
  final String lastName;
  final String parentStatus;
  final int? pregnancyTrimester;
  final int? babyAgeMonths;
  final int? roleId;

  AppUserRow({
    required this.id,
    required this.email,
    required this.firstName,
    required this.lastName,
    required this.parentStatus,
    this.pregnancyTrimester,
    this.babyAgeMonths,
    this.roleId,
  });

  factory AppUserRow.fromJson(Map<String, dynamic> json) {
    return AppUserRow(
      id: (json['id'] as num?)?.toInt() ?? 0,
      email: json['email'] ?? '',
      firstName: json['firstName'] ?? '',
      lastName: json['lastName'] ?? '',
      parentStatus: json['parentStatus'] ?? '',
      pregnancyTrimester: (json['pregnancyTrimester'] as num?)?.toInt(),
      babyAgeMonths: (json['babyAgeMonths'] as num?)?.toInt(),
      roleId: (json['roleId'] as num?)?.toInt(),
    );
  }

  String get fullName => '$firstName $lastName';

  String get pregnancyInfo {
    if (parentStatus == 'PREGNANT' && pregnancyTrimester != null) {
      return '${pregnancyTrimester! * 13}. sedmica';
    }
    return '-';
  }

  String get babyInfo {
    if (parentStatus == 'PARENT' && babyAgeMonths != null) {
      return '$babyAgeMonths mj';
    }
    return '-';
  }

  String get roleLabel {
    if (roleId == 1) return 'Roditelj';
    if (roleId == 2) return 'Doktor';
    return 'Korisnik';
  }
}
