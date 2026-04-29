import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/model/app_user_row.dart';
import 'package:flutter_application_nestly/providers/admin_pdf_service.dart';

class AdminDashboardService {
  Future<List<AppUserRow>> getUsers() async {
    try {
      final res = await ApiClient.get('/AppUser?RoleId=1');

      if (res.statusCode != 200) {
        throw Exception("Failed to load users.");
      }

      final List data = jsonDecode(res.body);
      return data.map((e) => AppUserRow.fromJson(e)).toList();
    } catch (_) {
      throw Exception("Unable to retrieve users.");
    }
  }

  Future<List> getFeedingLogs(int babyId) async {
    final res = await ApiClient.get('/api/feedinglog?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load feeding logs');
    }

    return jsonDecode(res.body);
  }

  Future<List> getMilestones(int babyId) async {
    final res = await ApiClient.get('/api/milestone?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load milestones');
    }

    return jsonDecode(res.body);
  }

  Future<List> getCalendarEvents(int babyId) async {
    final res = await ApiClient.get('/api/calendarevent?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load calendar events');
    }

    return jsonDecode(res.body);
  }

  Future<List> getMedication(int parentProfileId) async {
    final res = await ApiClient.get(
      '/api/medicationplan?ParentProfileId=$parentProfileId',
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load medication');
    }

    return jsonDecode(res.body);
  }

  Future<List> getSymptoms(int userId) async {
    final res = await ApiClient.get('/api/symptomdiary/parent/$userId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load symptoms');
    }

    return jsonDecode(res.body);
  }

  Future<List> getMeals(int babyId) async {
    final res = await ApiClient.get('/api/mealplan?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load meals');
    }

    return jsonDecode(res.body);
  }

  Future<List> getHealth(int babyId) async {
    final res = await ApiClient.get('/api/HealthEntry?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load health entries');
    }

    return jsonDecode(res.body);
  }

  Future<List> getDiapers(int babyId) async {
    final res = await ApiClient.get('/api/diaperlog?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load diapers');
    }

    return jsonDecode(res.body);
  }

  Future<List> getSleep(int babyId) async {
    final res = await ApiClient.get('/api/sleeplog?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load sleep logs');
    }

    return jsonDecode(res.body);
  }

  Future<List> getGrowth(int babyId) async {
    final res = await ApiClient.get('/api/babygrowth?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load growth');
    }

    return jsonDecode(res.body);
  }

  Future<List> getQuestions(int userId) async {
    final res = await ApiClient.get('/api/qaquestion/my?AskedById=$userId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load questions');
    }

    return jsonDecode(res.body);
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

class UserDetailsScreen extends StatefulWidget {
  final AppUserRow? user;

  const UserDetailsScreen({super.key, this.user});

  @override
  State<UserDetailsScreen> createState() => _UserDetailsScreenState();
}

class _UserDetailsScreenState extends State<UserDetailsScreen> with RouteAware {
  final _service = AdminDashboardService();

  List<AppUserRow> _users = [];
  List<AppUserRow> _filtered = [];
  List<DetailItem> _details = [];
  bool _loadingDetails = false;
  final _pdfService = AdminPdfService();
  AppUserRow? _selectedUser;
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    final route = ModalRoute.of(context);
    if (route is PageRoute) {
      routeObserver.subscribe(this, route);
    }
  }

  @override
  void dispose() {
    routeObserver.unsubscribe(this);
    super.dispose();
  }

  @override
  void didPopNext() {
    _resetScreen();
  }

  void _resetScreen() {
    if (!mounted) return;
    setState(() {
      _selectedUser = null;
      _details.clear();
      _loadingDetails = false;
      _filtered = _users;
    });
  }

  Future<void> _load() async {
    try {
      final users = await _service.getUsers();
      if (!mounted) return;

      setState(() {
        _users = users;
        _filtered = users;
        _selectedUser = null;
      });
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(
        context,
        'Trenutno nije moguće učitati korisnice. Pokušajte ponovo.',
      );
    } finally {
      if (!mounted) return;
      setState(() => _loading = false);
    }
  }

  Future<void> _loadDetails({
    required String module,
    required Future<List> Function() request,
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
      _loadingDetails = true;
      _details.clear();
    });

    try {
      final data = await request();

      if (!mounted) return;

      setState(() {
        _details = mapper(data);
      });
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju $module');
    } finally {
      if (!mounted) return;
      setState(() => _loadingDetails = false);
    }
  }

  List<DetailItem> _mapMedication(List data) {
    return data.map<DetailItem>((e) {
      final start = e['startDate']?.toString().split('T').first ?? '-';
      final end = e['endDate']?.toString().split('T').first ?? '-';

      return DetailItem(
        title: e['medicineName'] ?? '-',
        subtitle: '$start – $end',
        meta: 'Doza: ${e['dose'] ?? '-'}',
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
        title: e['foodName'] ?? '-',
        subtitle: e['triedAt']?.toString().split('T').first ?? '-',
        meta: 'Ocjena: ${e['rating'] ?? '-'}',
      );
    }).toList();
  }

  List<DetailItem> _mapQuestions(List data) {
    return data.map<DetailItem>((e) {
      final answered = e['isAnswered'] == true;

      return DetailItem(
        title: e['questionText'] ?? '',
        subtitle: e['createdAt']?.toString().split('T').first ?? '',
        meta: answered
            ? 'Odgovor: ${e['latestAnswerText'] ?? ''}'
            : 'Na čekanju',
      );
    }).toList();
  }

  void _onSearch(String value) {
    final q = value.trim().toLowerCase();
    if (q.isEmpty) {
      setState(() => _filtered = _users);
      return;
    }
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
        Expanded(
          flex: 5,
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const Text(
                  'Pregled korisnice',
                  style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
                ),

                const SizedBox(height: AppSpacing.lg),

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
                if (_selectedUser == null)
                  Padding(
                    padding: const EdgeInsets.only(bottom: AppSpacing.md),
                    child: Text(
                      'Odaberite korisnicu da biste vidjeli detalje',
                      style: TextStyle(
                        color: AppColors.textSecondary,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ),

                GridView.count(
                  crossAxisCount: 4,
                  shrinkWrap: true,
                  crossAxisSpacing: 12,
                  mainAxisSpacing: 12,
                  childAspectRatio: 1,
                  children: [
                    _ModuleCard(
                      icon: Icons.restaurant,
                      label: 'Hrana',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Hrana',
                              request: () =>
                                  _service.getMeals(_selectedUser!.id),
                              mapper: _mapMeals,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.health_and_safety,
                      label: 'Zdravlje',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Zdravlje',
                              request: () =>
                                  _service.getHealth(_selectedUser!.id),
                              mapper: _mapHealth,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.baby_changing_station,
                      label: 'Pelene',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Pelene',
                              request: () =>
                                  _service.getDiapers(_selectedUser!.id),
                              mapper: _mapDiapers,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.bedtime,
                      label: 'San bebe',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'San bebe',
                              request: () =>
                                  _service.getSleep(_selectedUser!.id),
                              mapper: _mapSleep,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.monitor_weight,
                      label: 'Rast bebe',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Rast bebe',
                              request: () =>
                                  _service.getGrowth(_selectedUser!.id),
                              mapper: _mapGrowth,
                            ),
                    ),

                    _ModuleCard(
                      icon: Icons.baby_changing_station_outlined,
                      label: 'Hranjenje',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Hranjenje',
                              request: () =>
                                  _service.getFeedingLogs(_selectedUser!.id),
                              mapper: _mapFeeding,
                            ),
                    ),

                    _ModuleCard(
                      icon: Icons.flag,
                      label: 'Dostignuća',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Milestones',
                              request: () =>
                                  _service.getMilestones(_selectedUser!.id),
                              mapper: _mapMilestones,
                            ),
                    ),

                    _ModuleCard(
                      icon: Icons.event,
                      label: 'Događaji',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Događaji',
                              request: () =>
                                  _service.getCalendarEvents(_selectedUser!.id),
                              mapper: _mapCalendar,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.medication,
                      label: 'Terapija',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Terapija',
                              request: () =>
                                  _service.getMedication(_selectedUser!.id),
                              mapper: _mapMedication,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.sick,
                      label: 'Simptomi',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Simptomi',
                              request: () =>
                                  _service.getSymptoms(_selectedUser!.id),
                              mapper: _mapSymptoms,
                            ),
                    ),
                    _ModuleCard(
                      icon: Icons.question_answer,
                      label: 'Pitanja',
                      onTap: _selectedUser == null
                          ? null
                          : () => _loadDetails(
                              module: 'Pitanja',
                              request: () =>
                                  _service.getQuestions(_selectedUser!.id),
                              mapper: _mapQuestions,
                            ),
                    ),
                  ],
                ),
                const SizedBox(height: 12),

                Row(
                  children: [
                    ElevatedButton.icon(
                      icon: const Icon(Icons.picture_as_pdf),
                      label: const Text("PDF Mama"),
                      onPressed: _selectedUser == null
                          ? null
                          : () async {
                              final therapy = await _service.getMedication(
                                _selectedUser!.id,
                              );
                              final symptoms = await _service.getSymptoms(
                                _selectedUser!.id,
                              );
                              final questions = await _service.getQuestions(
                                _selectedUser!.id,
                              );

                              final file = await _pdfService.generateMotherPdf(
                                userName: _selectedUser!.fullName,
                                therapy: _mapMedication(therapy),
                                symptoms: _mapSymptoms(symptoms),
                                questions: _mapQuestions(questions),
                              );

                              NestlyToast.success(
                                context,
                                "PDF je uspješno generisan.",
                                accentColor: AppColors.seed,
                              );
                            },
                    ),

                    const SizedBox(width: 10),

                    ElevatedButton.icon(
                      icon: const Icon(Icons.picture_as_pdf),
                      label: const Text("PDF Beba"),
                      onPressed: _selectedUser == null
                          ? null
                          : () async {
                              final meals = await _service.getMeals(
                                _selectedUser!.id,
                              );
                              final health = await _service.getHealth(
                                _selectedUser!.id,
                              );
                              final diapers = await _service.getDiapers(
                                _selectedUser!.id,
                              );
                              final sleep = await _service.getSleep(
                                _selectedUser!.id,
                              );
                              final growth = await _service.getGrowth(
                                _selectedUser!.id,
                              );
                              final feeding = await _service.getFeedingLogs(
                                _selectedUser!.id,
                              );
                              final milestones = await _service.getMilestones(
                                _selectedUser!.id,
                              );
                              final calendar = await _service.getCalendarEvents(
                                _selectedUser!.id,
                              );

                              final file = await _pdfService.generateBabyPdf(
                                userName: _selectedUser!.fullName,
                                meals: _mapMeals(meals),
                                health: _mapHealth(health),
                                diapers: _mapDiapers(diapers),
                                sleep: _mapSleep(sleep),
                                growth: _mapGrowth(growth),
                                feeding: _mapFeeding(feeding),
                                milestones: _mapMilestones(milestones),
                                calendar: _mapCalendar(calendar),
                              );

                              NestlyToast.success(
                                context,
                                "PDF generisan: ${file.path}",
                                accentColor: AppColors.seed,
                              );
                            },
                    ),
                  ],
                ),
                const SizedBox(height: AppSpacing.lg),

                TextField(
                  onChanged: _onSearch,
                  decoration: const InputDecoration(
                    prefixIcon: Icon(Icons.search),
                    hintText: 'Pretraga korisnica',
                  ),
                ),

                const SizedBox(height: AppSpacing.lg),

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
                                  fontWeight: FontWeight.w700,
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

List<DetailItem> _mapHealth(List data) {
  return data.map<DetailItem>((e) {
    return DetailItem(
      title: e['entryDate'].toString().split('T').first,
      subtitle: 'Temperatura: ${e['temperatureC'] ?? '-'} °C',
      meta:
          'Lijekovi: ${e['medicines'] ?? '-'}  |  Posjeta doktoru: ${e['doctorVisit'] ?? '-'}',
    );
  }).toList();
}

List<DetailItem> _mapFeeding(List data) {
  return data.map<DetailItem>((e) {
    final date = e['feedDate']?.toString().split('T').first ?? '-';

    return DetailItem(
      title: date,
      subtitle: '${e['foodTypeName'] ?? '-'} u ${e['feedTime']}',
      meta: 'Količina: ${e['amountMl'] ?? '-'} ml',
    );
  }).toList();
}

List<DetailItem> _mapMilestones(List data) {
  return data.map<DetailItem>((e) {
    final date = e['achievedDate']?.toString().split('T').first ?? '-';

    return DetailItem(
      title: e['title'] ?? '-',
      subtitle: 'Postignuto: $date',
      meta: e['notes'] ?? '',
    );
  }).toList();
}

List<DetailItem> _mapCalendar(List data) {
  return data.map<DetailItem>((e) {
    final date = e['startAt']?.toString().split('T').first ?? '-';

    return DetailItem(
      title: e['title'] ?? '-',
      subtitle: date,
      meta: e['description'] ?? '',
    );
  }).toList();
}
