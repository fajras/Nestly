import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/model/app_user_row.dart';

/// =======================
/// API
/// =======================

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _headers(String token) => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  'Authorization': 'Bearer $token',
};

/// =======================
/// SERVICE
/// =======================

class AdminDashboardService {
  Future<List<AppUserRow>> getUsers(String token) async {
    final res = await http.get(
      Uri.parse('$_apiBase/AppUser'),
      headers: _headers(token),
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load users');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => AppUserRow.fromJson(e)).toList();
  }
}

class MedicationPlanRow {
  final int id;
  final String medicineName;
  final String dose;
  final DateTime startDate;
  final DateTime endDate;

  MedicationPlanRow({
    required this.id,
    required this.medicineName,
    required this.dose,
    required this.startDate,
    required this.endDate,
  });

  factory MedicationPlanRow.fromJson(Map<String, dynamic> json) {
    return MedicationPlanRow(
      id: json['id'],
      medicineName: json['medicineName'],
      dose: json['dose'],
      startDate: DateTime.parse(json['startDate']),
      endDate: DateTime.parse(json['endDate']),
    );
  }
}

/// =======================
/// SCREEN
/// =======================

class UserDetailsScreen extends StatefulWidget {
  final String token;
  final AppUserRow? user;

  const UserDetailsScreen({super.key, required this.token, this.user});

  @override
  State<UserDetailsScreen> createState() => _UserDetailsScreenState();
}

class _UserDetailsScreenState extends State<UserDetailsScreen> {
  final _service = AdminDashboardService();

  List<AppUserRow> _users = [];
  List<AppUserRow> _filtered = [];
  List<DetailItem> _details = [];
  bool _loadingDetails = false;
  String _activeModule = '';

  AppUserRow? _selectedUser;

  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _loadDetails({
    required String module,
    required String url,
    required List<DetailItem> Function(List data) mapper,
  }) async {
    if (_selectedUser == null) {
      NestlyToast.info(
        context,
        'Molimo označite korisnicu prije pregleda "$module"',
      );
      return;
    }

    setState(() {
      _activeModule = module;
      _loadingDetails = true;
      _details.clear();
    });

    try {
      final res = await http.get(
        Uri.parse(url),
        headers: _headers(widget.token),
      );

      if (res.statusCode != 200) throw Exception();

      final List data = jsonDecode(res.body);

      setState(() {
        _details = mapper(data);
      });
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju $module');
    } finally {
      setState(() => _loadingDetails = false);
    }
  }

  List<DetailItem> _mapMedication(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: e['medicineName'],
        subtitle:
            '${e['startDate'].toString().split('T').first} – ${e['endDate'].toString().split('T').first}',
        meta: e['dose'],
      );
    }).toList();
  }

  List<DetailItem> _mapSymptoms(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: 'Datum: ${e['date'].toString().split('T').first}',
        subtitle:
            'Mučnina: ${e['nausea'] ?? '-'}  Umor: ${e['fatigue'] ?? '-'}',
        meta: 'Glavobolja: ${e['headache'] ?? '-'}',
      );
    }).toList();
  }

  List<DetailItem> _mapGrowth(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: 'Sedmica ${e['weekNumber']}',
        subtitle:
            'Težina: ${e['weightKg'] ?? '-'} kg  Visina: ${e['heightCm'] ?? '-'} cm',
        meta: 'Obim glave: ${e['headCircumferenceCm'] ?? '-'} cm',
      );
    }).toList();
  }

  List<DetailItem> _mapSleep(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: e['sleepDate'].toString().split('T').first,
        subtitle: '${e['startTime']} – ${e['endTime']}',
        meta: '${e['durationMinutes']} min',
      );
    }).toList();
  }

  List<DetailItem> _mapDiapers(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: e['changeDate'].toString().split('T').first,
        subtitle: e['diaperState'],
        meta: e['notes'] ?? '',
      );
    }).toList();
  }

  List<DetailItem> _mapMeals(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: e['foodType']['name'],
        subtitle: e['triedAt'].toString().split('T').first,
        meta: 'Ocjena: ${e['rating'] ?? '-'}',
      );
    }).toList();
  }

  List<DetailItem> _mapQuestions(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: e['questionText'],
        subtitle: e['createdAt'].toString().split('T').first,
        meta: e['isAnswered']
            ? 'Odgovor: ${e['answeredByName'] ?? ''}'
            : 'Na čekanju',
      );
    }).toList();
  }

  Future<void> _load() async {
    try {
      final users = await _service.getUsers(widget.token);
      setState(() {
        _users = users;
        _filtered = users;
        _selectedUser = users.isNotEmpty ? users.first : null;
      });
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju korisnica');
    } finally {
      setState(() => _loading = false);
    }
  }

  void _onSearch(String value) {
    final q = value.toLowerCase();
    setState(() {
      _filtered = _users.where((u) {
        return u.firstName.toLowerCase().contains(q) ||
            u.lastName.toLowerCase().contains(q) ||
            u.email.toLowerCase().contains(q);
      }).toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    if (_loading) {
      return const Center(child: CircularProgressIndicator());
    }

    return Row(
      children: [
        /// =======================
        /// LEFT SIDE
        /// =======================
        Expanded(
          flex: 5,
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const Text(
                  'Pregled korisnice',
                  style: TextStyle(fontSize: 26, fontWeight: FontWeight.w800),
                ),

                const SizedBox(height: AppSpacing.lg),

                /// USER HEADER
                if (_selectedUser != null)
                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(AppSpacing.lg),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Text(
                            _selectedUser!.fullName,
                            style: const TextStyle(
                              fontSize: 18,
                              fontWeight: FontWeight.w700,
                            ),
                          ),
                          Text(
                            _selectedUser!.pregnancyInfo,
                            style: const TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.w600,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),

                const SizedBox(height: AppSpacing.lg),

                /// MODULE CARDS
                GridView.count(
                  crossAxisCount: 4,
                  shrinkWrap: true,
                  crossAxisSpacing: 12,
                  mainAxisSpacing: 12,
                  childAspectRatio: 1.3,
                  children: [
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Terapija',
                      onTap: () => _loadDetails(
                        module: 'Terapija',
                        url:
                            '$_apiBase/api/medicationplan?UserId=${_selectedUser!.id}',
                        mapper: _mapMedication,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Simptomi',
                      onTap: () => _loadDetails(
                        module: 'Simptomi',
                        url:
                            '$_apiBase/api/symptomdiary/parent/${_selectedUser!.id}',
                        mapper: _mapSymptoms,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Hrana',
                      onTap: () => _loadDetails(
                        module: 'Hrana',
                        url:
                            '$_apiBase/api/mealplan?BabyId=${_selectedUser!.id}',
                        mapper: _mapMeals,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Zdravlje',
                      onTap: () => _loadDetails(
                        module: 'Zdravlje',
                        url:
                            '$_apiBase/api/medicationplan?UserId=${_selectedUser!.id}',
                        mapper: _mapMedication,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Pelene',
                      onTap: () => _loadDetails(
                        module: 'Pelene',
                        url:
                            '$_apiBase/api/diaperlog?BabyId=${_selectedUser!.id}',
                        mapper: _mapDiapers,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'San bebe',
                      onTap: () => _loadDetails(
                        module: 'San bebe',
                        url:
                            '$_apiBase/api/sleeplog?BabyId=${_selectedUser!.id}',
                        mapper: _mapSleep,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Rast bebe',
                      onTap: () => _loadDetails(
                        module: 'Rast bebe',
                        url:
                            '$_apiBase/api/babygrowth?BabyId=${_selectedUser!.id}',
                        mapper: _mapGrowth,
                      ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Pitanja',
                      onTap: () => _loadDetails(
                        module: 'Pitanja',
                        url:
                            '$_apiBase/api/qaquestion/my?AskedByUserId=${_selectedUser!.id}',
                        mapper: _mapQuestions,
                      ),
                    ),
                  ],
                ),

                const SizedBox(height: AppSpacing.lg),

                /// SEARCH
                TextField(
                  onChanged: _onSearch,
                  decoration: const InputDecoration(
                    prefixIcon: Icon(Icons.search),
                    hintText: 'Pretraga korisnica',
                  ),
                ),

                const SizedBox(height: AppSpacing.lg),

                /// USERS LIST
                Expanded(
                  child: ListView.separated(
                    itemCount: _filtered.length,
                    separatorBuilder: (_, __) =>
                        const SizedBox(height: AppSpacing.md),
                    itemBuilder: (_, i) {
                      final u = _filtered[i];
                      return InkWell(
                        onTap: () {
                          setState(() => _selectedUser = u);
                        },
                        child: Card(
                          child: Padding(
                            padding: const EdgeInsets.all(AppSpacing.lg),
                            child: Row(
                              children: [
                                CircleAvatar(
                                  child: Text(
                                    u.firstName.isNotEmpty
                                        ? u.firstName[0]
                                        : '?',
                                  ),
                                ),
                                const SizedBox(width: AppSpacing.lg),
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      u.fullName,
                                      style: const TextStyle(
                                        fontWeight: FontWeight.w700,
                                      ),
                                    ),
                                    Text(
                                      u.email,
                                      style: const TextStyle(
                                        color: AppColors.textSecondary,
                                      ),
                                    ),
                                  ],
                                ),
                              ],
                            ),
                          ),
                        ),
                      );
                    },
                  ),
                ),
              ],
            ),
          ),
        ),

        /// =======================
        /// RIGHT SIDE
        /// =======================
        Expanded(
          flex: 4,
          child: Container(
            color: AppColors.seed.withOpacity(.03),
            child: _loadingDetails
                ? const Center(child: CircularProgressIndicator())
                : _details.isEmpty
                ? const Center(
                    child: Text(
                      'Nema podataka',
                      style: TextStyle(
                        fontSize: 16,
                        color: AppColors.textSecondary,
                      ),
                    ),
                  )
                : ListView.separated(
                    padding: const EdgeInsets.all(AppSpacing.lg),
                    itemCount: _details.length,
                    separatorBuilder: (_, __) =>
                        const SizedBox(height: AppSpacing.md),
                    itemBuilder: (_, i) {
                      final d = _details[i];

                      return Card(
                        child: Padding(
                          padding: const EdgeInsets.all(AppSpacing.lg),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                d.title,
                                style: const TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.w800,
                                ),
                              ),
                              const SizedBox(height: 6),
                              Text(
                                d.subtitle,
                                style: const TextStyle(
                                  color: AppColors.textSecondary,
                                ),
                              ),
                              const SizedBox(height: 10),
                              Align(
                                alignment: Alignment.centerLeft,
                                child: Container(
                                  padding: const EdgeInsets.symmetric(
                                    horizontal: 10,
                                    vertical: 6,
                                  ),
                                  decoration: BoxDecoration(
                                    color: AppColors.seed.withOpacity(.12),
                                    borderRadius: BorderRadius.circular(20),
                                  ),
                                  child: Text(
                                    d.meta,
                                    style: const TextStyle(
                                      fontWeight: FontWeight.w600,
                                      color: AppColors.seed,
                                    ),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      );
                    },
                  ),
          ),
        ),
      ],
    );
  }
}

/// =======================
/// MODULE CARD
/// =======================

class _ModuleCard extends StatelessWidget {
  final IconData icon;
  final String label;
  final VoidCallback? onTap;

  const _ModuleCard({required this.icon, required this.label, this.onTap});

  @override
  Widget build(BuildContext context) {
    final enabled = onTap != null;

    return Opacity(
      opacity: enabled ? 1 : 0.5,
      child: Card(
        child: InkWell(
          borderRadius: BorderRadius.circular(AppRadius.lg),
          onTap: onTap,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(icon, size: 32, color: AppColors.seed),
              const SizedBox(height: 8),
              Text(label, style: const TextStyle(fontWeight: FontWeight.w600)),
            ],
          ),
        ),
      ),
    );
  }
}

class DetailItem {
  final String title;
  final String subtitle;
  final String meta;

  DetailItem({required this.title, required this.subtitle, required this.meta});
}
