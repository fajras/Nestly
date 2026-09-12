import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:pdf/pdf.dart' show PdfColors;
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/model/app_user_row.dart';
import 'package:flutter_application_nestly/providers/admin_pdf_service.dart';
import 'package:flutter_application_nestly/providers/api_response_helper.dart';
import 'package:flutter_application_nestly/layouts/nestly_widgets.dart';

class AdminDashboardService {
  Future<List<AppUserRow>> getUsers() async {
    try {
      final data = await _fetchAllPages('/AppUser?RoleId=1');

      return data.map<AppUserRow>((e) => AppUserRow.fromJson(e)).toList();
    } catch (_) {
      throw Exception("Unable to retrieve users.");
    }
  }

  Future<List<dynamic>> _fetchAllPages(String url) async {
    List all = [];
    int page = 1;
    bool hasMore = true;

    while (hasMore) {
      final separator = url.contains('?') ? '&' : '?';

      final res = await ApiClient.get(
        '$url${separator}Page=$page&PageSize=100',
      );

      if (res.statusCode != 200) {
        throw Exception('Failed to load data');
      }

      final decoded = jsonDecode(res.body);

      final items = ApiResponseHelper.extractList(res.body);
      final totalCount = decoded['totalCount'] as int? ?? 0;

      all.addAll(items);

      if (items.isEmpty) {
        hasMore = false;
      } else {
        hasMore = all.length < totalCount;
      }

      page++;
    }

    return all;
  }

  Future<List> getFeedingLogs(int babyId) async {
    return _fetchAllPages('/api/feedinglog?BabyId=$babyId');
  }

  Future<List> getMilestones(int babyId) async {
    return _fetchAllPages('/api/milestone?BabyId=$babyId');
  }

  Future<List> getCalendarEvents(int babyId) async {
    return _fetchAllPages('/api/calendarevent?BabyId=$babyId');
  }

  Future<List> getMedication(int parentProfileId) async {
    return _fetchAllPages(
      '/api/medicationplan?ParentProfileId=$parentProfileId',
    );
  }

  Future<List> getSymptoms(int userId) async {
    return _fetchAllPages('/api/symptomdiary?ParentProfileId=$userId');
  }

  Future<List> getMeals(int babyId) async {
    return _fetchAllPages('/api/mealplan?BabyId=$babyId');
  }

  Future<List> getHealth(int babyId) async {
    return _fetchAllPages('/api/HealthEntry?BabyId=$babyId');
  }

  Future<List> getDiapers(int babyId) async {
    return _fetchAllPages('/api/diaperlog?BabyId=$babyId');
  }

  Future<List> getSleep(int babyId) async {
    return _fetchAllPages('/api/sleeplog?BabyId=$babyId');
  }

  Future<List> getGrowth(int babyId) async {
    return _fetchAllPages('/api/babygrowth?BabyId=$babyId');
  }

  Future<List> getQuestions(int parentProfileId) async {
    return _fetchAllPages('/api/qaquestion/user/$parentProfileId');
  }

  // The doctor-facing user list only carries the parent's AppUser/ParentProfile
  // ids, not a baby id - and BabyProfile isn't filterable by parent server-side,
  // so resolve it by pulling the (doctor-only) full baby list and matching
  // ParentProfileId client-side. Returns the most recently born baby if the
  // parent has more than one, or null if the parent has none yet.
  Future<int?> getPrimaryBabyId(int parentProfileId) async {
    final babies = await _fetchAllPages('/api/BabyProfile?PageSize=200');

    final matches = babies
        .where(
          (b) => (b['parentProfileId'] as num?)?.toInt() == parentProfileId,
        )
        .toList();

    if (matches.isEmpty) return null;

    matches.sort(
      (a, b) => (b['birthDate'] as String).compareTo(a['birthDate'] as String),
    );

    return (matches.first['id'] as num).toInt();
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
  bool _reportBusy = false;
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

  int _requireParentProfileId() {
    final id = _selectedUser?.parentProfileId;
    if (id == null) {
      throw Exception('Korisnica nema profil roditelja');
    }
    return id;
  }

  Future<int> _requireBabyId() async {
    final babyId = await _service.getPrimaryBabyId(_requireParentProfileId());
    if (babyId == null) {
      throw Exception('Korisnica još nema unesenu bebu');
    }
    return babyId;
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

  String _formatDiaperState(dynamic raw) {
    switch (raw?.toString().toLowerCase()) {
      case 'mokra':
        return 'Mokra';
      case 'stolica':
        return 'Stolica';
      case 'kombinovano':
        return 'Kombinovano';
      default:
        return 'Mokra';
    }
  }

  List<DetailItem> _mapDiapers(List data) {
    return data.map<DetailItem>((e) {
      return DetailItem(
        title: e['changeDate'].toString().split('T').first,
        subtitle: _formatDiaperState(e['diaperState']),
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
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Pregled korisnice',
                    style: TextStyle(
                      fontSize: 26,
                      fontWeight: FontWeight.w800,
                      color: AppColors.seed,
                    ),
                  ),

                  const SizedBox(height: AppSpacing.lg),

                  if (_selectedUser != null)
                    Container(
                      decoration: BoxDecoration(
                        color: AppColors.card,
                        borderRadius: BorderRadius.circular(AppRadius.xl),
                        border: const Border(
                          left: BorderSide(color: AppColors.seed, width: 4),
                        ),
                        boxShadow: [
                          BoxShadow(
                            color: Colors.black.withOpacity(.04),
                            blurRadius: 10,
                            offset: const Offset(0, 4),
                          ),
                        ],
                      ),
                      child: Padding(
                        padding: const EdgeInsets.all(AppSpacing.lg),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Row(
                              children: [
                                NestlyAvatar(name: _selectedUser!.fullName),
                                const SizedBox(width: AppSpacing.md),
                                Text(
                                  _selectedUser!.fullName,
                                  style: const TextStyle(
                                    fontSize: 18,
                                    fontWeight: FontWeight.w700,
                                  ),
                                ),
                              ],
                            ),
                            Container(
                              padding: const EdgeInsets.symmetric(
                                horizontal: 12,
                                vertical: 6,
                              ),
                              decoration: BoxDecoration(
                                color: AppColors.seed.withOpacity(.12),
                                borderRadius: BorderRadius.circular(20),
                              ),
                              child: Text(
                                _selectedUser!.pregnancyInfo,
                                style: const TextStyle(
                                  fontSize: 14,
                                  fontWeight: FontWeight.w700,
                                  color: AppColors.seed,
                                ),
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
                    physics: const NeverScrollableScrollPhysics(),
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
                                request: () async =>
                                    _service.getMeals(await _requireBabyId()),
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
                                request: () async => _service.getHealth(
                                  await _requireBabyId(),
                                ),
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
                                request: () async => _service.getDiapers(
                                  await _requireBabyId(),
                                ),
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
                                request: () async =>
                                    _service.getSleep(await _requireBabyId()),
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
                                request: () async => _service.getGrowth(
                                  await _requireBabyId(),
                                ),
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
                                request: () async => _service.getFeedingLogs(
                                  await _requireBabyId(),
                                ),
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
                                request: () async => _service.getMilestones(
                                  await _requireBabyId(),
                                ),
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
                                request: () async => _service.getCalendarEvents(
                                  await _requireBabyId(),
                                ),
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
                                request: () => _service.getMedication(
                                  _requireParentProfileId(),
                                ),
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
                                request: () => _service.getSymptoms(
                                  _requireParentProfileId(),
                                ),
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
                                request: () => _service.getQuestions(
                                  _requireParentProfileId(),
                                ),
                                mapper: _mapQuestions,
                              ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 12),

                  Wrap(
                    spacing: 16,
                    runSpacing: 16,
                    children: [
                      _ReportActionCard(
                        title: 'Izvještaj o majci',
                        subtitle: 'Terapija, simptomi i pitanja',
                        icon: Icons.pregnant_woman,
                        accentColor: AppColors.seed,
                        enabled: _selectedUser != null && !_reportBusy,

                        onDownload: () async {
                          if (_reportBusy) return;
                          setState(() => _reportBusy = true);

                          try {
                            final parentProfileId = _requireParentProfileId();

                            final therapy = await _service.getMedication(
                              parentProfileId,
                            );

                            final symptoms = await _service.getSymptoms(
                              parentProfileId,
                            );

                            final questions = await _service.getQuestions(
                              parentProfileId,
                            );

                            await _pdfService.saveReportPdf(
                              reportTitle: 'Izvještaj o majci',
                              fileNamePrefix: 'mother_report',
                              userName: _selectedUser!.fullName,
                              charts: _buildMotherCharts(symptoms),
                              tables: _buildMotherTables(therapy, questions),
                            );

                            if (!mounted) return;

                            NestlyToast.success(
                              context,
                              "PDF izvještaj je uspješno preuzet.",
                              accentColor: AppColors.seed,
                            );
                          } catch (e) {
                            debugPrint('MOTHER REPORT DOWNLOAD ERROR: $e');
                            if (!mounted) return;
                            NestlyToast.error(
                              context,
                              'Greška pri preuzimanju izvještaja: $e',
                            );
                          } finally {
                            if (mounted) setState(() => _reportBusy = false);
                          }
                        },

                        onPrint: () async {
                          if (_reportBusy) return;
                          setState(() => _reportBusy = true);

                          try {
                            final parentProfileId = _requireParentProfileId();

                            final therapy = await _service.getMedication(
                              parentProfileId,
                            );

                            final symptoms = await _service.getSymptoms(
                              parentProfileId,
                            );

                            final questions = await _service.getQuestions(
                              parentProfileId,
                            );

                            await _pdfService.printReportPdf(
                              reportTitle: 'Izvještaj o majci',
                              userName: _selectedUser!.fullName,
                              charts: _buildMotherCharts(symptoms),
                              tables: _buildMotherTables(therapy, questions),
                            );
                          } catch (e) {
                            debugPrint('MOTHER REPORT PRINT ERROR: $e');
                            if (!mounted) return;
                            NestlyToast.error(
                              context,
                              'Greška pri štampanju izvještaja: $e',
                            );
                          } finally {
                            if (mounted) setState(() => _reportBusy = false);
                          }
                        },
                      ),

                      _ReportActionCard(
                        title: 'Izvještaj o bebi',
                        subtitle: 'Zdravlje, rast i aktivnosti',
                        icon: Icons.child_care,
                        enabled: _selectedUser != null && !_reportBusy,

                        onDownload: () async {
                          if (_reportBusy) return;
                          setState(() => _reportBusy = true);

                          try {
                            final babyId = await _requireBabyId();

                            final meals = await _service.getMeals(babyId);
                            final health = await _service.getHealth(babyId);
                            final diapers = await _service.getDiapers(babyId);
                            final sleep = await _service.getSleep(babyId);
                            final growth = await _service.getGrowth(babyId);
                            final feeding = await _service.getFeedingLogs(
                              babyId,
                            );
                            final milestones = await _service.getMilestones(
                              babyId,
                            );
                            final calendar = await _service.getCalendarEvents(
                              babyId,
                            );

                            await _pdfService.saveReportPdf(
                              reportTitle: 'Izvještaj o bebi',
                              fileNamePrefix: 'baby_report',
                              userName: _selectedUser!.fullName,
                              charts: _buildBabyCharts(growth, sleep, health, feeding, diapers),
                              tables: _buildBabyTables(
                                meals,
                                health,
                                diapers,
                                sleep,
                                feeding,
                                milestones,
                                calendar,
                              ),
                            );

                            if (!mounted) return;

                            NestlyToast.success(
                              context,
                              "PDF izvještaj je uspješno preuzet.",
                              accentColor: AppColors.seed,
                            );
                          } catch (e) {
                            debugPrint('BABY REPORT DOWNLOAD ERROR: $e');
                            if (!mounted) return;
                            NestlyToast.error(
                              context,
                              'Greška pri preuzimanju izvještaja: $e',
                            );
                          } finally {
                            if (mounted) setState(() => _reportBusy = false);
                          }
                        },

                        onPrint: () async {
                          if (_reportBusy) return;
                          setState(() => _reportBusy = true);

                          try {
                            final babyId = await _requireBabyId();

                            final meals = await _service.getMeals(babyId);
                            final health = await _service.getHealth(babyId);
                            final diapers = await _service.getDiapers(babyId);
                            final sleep = await _service.getSleep(babyId);
                            final growth = await _service.getGrowth(babyId);
                            final feeding = await _service.getFeedingLogs(
                              babyId,
                            );
                            final milestones = await _service.getMilestones(
                              babyId,
                            );
                            final calendar = await _service.getCalendarEvents(
                              babyId,
                            );

                            await _pdfService.printReportPdf(
                              reportTitle: 'Izvještaj o bebi',
                              userName: _selectedUser!.fullName,
                              charts: _buildBabyCharts(growth, sleep, health, feeding, diapers),
                              tables: _buildBabyTables(
                                meals,
                                health,
                                diapers,
                                sleep,
                                feeding,
                                milestones,
                                calendar,
                              ),
                            );
                          } catch (e) {
                            debugPrint('BABY REPORT PRINT ERROR: $e');
                            if (!mounted) return;
                            NestlyToast.error(
                              context,
                              'Greška pri štampanju izvještaja: $e',
                            );
                          } finally {
                            if (mounted) setState(() => _reportBusy = false);
                          }
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

                  ListView.separated(
                    shrinkWrap: true,
                    physics: const NeverScrollableScrollPhysics(),
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
                ],
              ),
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

class _ReportActionCard extends StatelessWidget {
  final String title;
  final String subtitle;
  final IconData icon;
  final Color accentColor;

  final VoidCallback onDownload;
  final VoidCallback onPrint;

  final bool enabled;

  const _ReportActionCard({
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.onDownload,
    required this.onPrint,
    required this.enabled,
    this.accentColor = AppColors.seed,
  });

  @override
  Widget build(BuildContext context) {
    return Opacity(
      opacity: enabled ? 1 : 0.5,

      child: Container(
        width: 320,

        padding: const EdgeInsets.all(AppSpacing.lg),

        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(AppRadius.lg),

          border: Border.all(color: accentColor.withOpacity(.2)),

          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(.03),
              blurRadius: 10,
              offset: const Offset(0, 4),
            ),
          ],
        ),

        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,

          children: [
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(10),

                  decoration: BoxDecoration(
                    color: accentColor.withOpacity(.1),
                    borderRadius: BorderRadius.circular(12),
                  ),

                  child: Icon(icon, color: accentColor),
                ),

                const SizedBox(width: 12),

                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,

                    children: [
                      Text(
                        title,

                        style: const TextStyle(
                          fontWeight: FontWeight.w700,
                          fontSize: 16,
                        ),
                      ),

                      const SizedBox(height: 4),

                      Text(
                        subtitle,

                        style: const TextStyle(
                          color: AppColors.textSecondary,
                          fontSize: 13,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),

            const SizedBox(height: 20),

            Row(
              children: [
                Expanded(
                  child: OutlinedButton.icon(
                    onPressed: enabled ? onDownload : null,

                    icon: const Icon(Icons.download),

                    label: const Text('Preuzmi'),
                  ),
                ),

                const SizedBox(width: 12),

                Expanded(
                  child: ElevatedButton.icon(
                    onPressed: enabled ? onPrint : null,

                    style: ElevatedButton.styleFrom(
                      backgroundColor: AppColors.seed,
                      foregroundColor: Colors.white,
                    ),

                    icon: const Icon(Icons.print),

                    label: const Text('Print'),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

// ---------------------------------------------------------------------------
// PDF report builders - convert raw API rows into ReportChart (measurable
// parameters over time) and ReportTable (everything else) for a report
// that's actually readable, instead of one card per row.
// ---------------------------------------------------------------------------

String _shortDate(dynamic iso) {
  if (iso == null) return '-';
  return iso.toString().split('T').first;
}

/// Compact dd.MM. label for chart X-axes - the full yyyy-MM-dd date used in
/// tables is too wide and overlaps once a chart has more than a couple of
/// points.
String _chartDate(dynamic iso) {
  final full = _shortDate(iso);
  final parts = full.split('-');
  if (parts.length != 3) return full;
  return '${parts[2]}.${parts[1]}.';
}

List<ReportTable> _buildMotherTables(List therapy, List questions) {
  return [
    ReportTable(
      title: 'Terapija',
      headers: const ['Lijek', 'Doza', 'Period'],
      rows: [
        for (final e in therapy)
          [
            (e['medicineName'] ?? '-').toString(),
            (e['dose'] ?? '-').toString(),
            '${_shortDate(e['startDate'])} - ${_shortDate(e['endDate'])}',
          ],
      ],
    ),
    ReportTable(
      title: 'Pitanja',
      headers: const ['Datum', 'Pitanje', 'Odgovor'],
      rows: [
        for (final e in questions)
          [
            _shortDate(e['createdAt']),
            (e['questionText'] ?? '-').toString(),
            e['isAnswered'] == true
                ? (e['latestAnswerText']?.toString() ?? '-')
                : 'Na čekanju',
          ],
      ],
    ),
  ];
}

// Charts show at most this many most-recent date points - beyond that the
// x-axis labels start overlapping regardless of font size/angle, and a
// month is plenty to see a meaningful trend.
const _maxChartPoints = 30;

List<T> _lastN<T>(List<T> sortedAscending, int n) {
  if (sortedAscending.length <= n) return sortedAscending;
  return sortedAscending.sublist(sortedAscending.length - n);
}

List<ReportChart> _buildMotherCharts(List symptoms) {
  final sorted = _lastN(
    List.of(symptoms)
      ..sort(
        (a, b) => (a['date'] ?? '').toString().compareTo((b['date'] ?? '').toString()),
      ),
    _maxChartPoints,
  );

  ReportChartSeries? series(String key, String legend) {
    final pts = <ChartPoint>[];
    for (final e in sorted) {
      final v = e[key];
      if (v == null) continue;
      pts.add(ChartPoint(_chartDate(e['date']), (v as num).toDouble()));
    }
    return pts.isEmpty ? null : ReportChartSeries(legend: legend, points: pts);
  }

  final allSeries = [
    series('nausea', 'Mučnina'),
    series('fatigue', 'Umor'),
    series('headache', 'Glavobolja'),
    series('heartburn', 'Žgaravica'),
    series('legSwelling', 'Oticanje nogu'),
  ].whereType<ReportChartSeries>().toList();

  if (allSeries.isEmpty) return [];

  return [
    ReportChart(
      title: 'Simptomi kroz vrijeme',
      unit: 'skala 0-5',
      series: allSeries,
      yMin: 0,
      yMax: 5,
      integerTicks: true,
    ),
  ];
}

List<ReportChart> _buildBabyCharts(
  List growth,
  List sleep,
  List health,
  List feeding,
  List diapers,
) {
  final sortedGrowth = List.of(growth)
    ..sort(
      (a, b) => ((a['weekNumber'] as num?) ?? 0)
          .compareTo((b['weekNumber'] as num?) ?? 0),
    );
  final sortedSleep = List.of(sleep)
    ..sort(
      (a, b) => (a['sleepDate'] ?? '').toString().compareTo(
            (b['sleepDate'] ?? '').toString(),
          ),
    );
  final sortedHealth = List.of(health)
    ..sort(
      (a, b) => (a['entryDate'] ?? '').toString().compareTo(
            (b['entryDate'] ?? '').toString(),
          ),
    );

  final charts = <ReportChart>[];

  final weight = [
    for (final e in sortedGrowth)
      if (e['weightKg'] != null)
        ChartPoint('Sed. ${e['weekNumber']}', (e['weightKg'] as num).toDouble()),
  ];
  if (weight.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Težina',
        unit: 'kg',
        series: [ReportChartSeries(legend: 'Težina', points: weight)],
      ),
    );
  }

  final height = [
    for (final e in sortedGrowth)
      if (e['heightCm'] != null)
        ChartPoint('Sed. ${e['weekNumber']}', (e['heightCm'] as num).toDouble()),
  ];
  if (height.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Visina',
        unit: 'cm',
        series: [
          ReportChartSeries(
            legend: 'Visina',
            points: height,
            color: PdfColors.orange,
          ),
        ],
      ),
    );
  }

  final head = [
    for (final e in sortedGrowth)
      if (e['headCircumferenceCm'] != null)
        ChartPoint(
          'Sed. ${e['weekNumber']}',
          (e['headCircumferenceCm'] as num).toDouble(),
        ),
  ];
  if (head.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Obim glave',
        unit: 'cm',
        series: [
          ReportChartSeries(
            legend: 'Obim glave',
            points: head,
            color: PdfColors.purple,
          ),
        ],
      ),
    );
  }

  // One point per day per sleep type (total hours that day) rather than one
  // point per individual sleep log entry - several naps/entries on the same
  // day would otherwise plot as separate, hard-to-read points. Split into
  // night sleep vs daytime naps (by start hour) so the breakdown reads
  // directly off the axis as two lines, the same way symptoms do, instead
  // of needing to eyeball a stacked/grouped bar.
  final nightMinutesByDay = <String, double>{};
  final napMinutesByDay = <String, double>{};
  for (final e in sortedSleep) {
    if (e['durationMinutes'] == null) continue;
    final day = _shortDate(e['sleepDate']);
    final startHour = int.tryParse(
          (e['startTime'] ?? '').toString().split(':').first,
        ) ??
        0;
    final isNight = startHour >= 18 || startHour < 6;
    final minutes = (e['durationMinutes'] as num).toDouble();
    if (isNight) {
      nightMinutesByDay[day] = (nightMinutesByDay[day] ?? 0) + minutes;
    } else {
      napMinutesByDay[day] = (napMinutesByDay[day] ?? 0) + minutes;
    }
  }
  final sleepDays = _lastN(
    ({...nightMinutesByDay.keys, ...napMinutesByDay.keys}.toList()..sort()),
    _maxChartPoints,
  );
  if (sleepDays.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Trajanje sna (po danu)',
        unit: 'h',
        yMin: 0,
        integerTicks: true,
        series: [
          ReportChartSeries(
            legend: 'San noću',
            color: PdfColors.indigo,
            points: [
              for (final d in sleepDays)
                ChartPoint(_chartDate(d), (nightMinutesByDay[d] ?? 0) / 60),
            ],
          ),
          ReportChartSeries(
            legend: 'Dnevni odmor',
            color: PdfColors.amber,
            points: [
              for (final d in sleepDays)
                ChartPoint(_chartDate(d), (napMinutesByDay[d] ?? 0) / 60),
            ],
          ),
        ],
      ),
    );
  }

  final tempPts = _lastN(
    [
      for (final e in sortedHealth)
        if (e['temperatureC'] != null)
          ChartPoint(_chartDate(e['entryDate']), (e['temperatureC'] as num).toDouble()),
    ],
    _maxChartPoints,
  );
  if (tempPts.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Temperatura',
        unit: '°C',
        series: [
          ReportChartSeries(
            legend: 'Temperatura',
            points: tempPts,
            color: PdfColors.red,
          ),
        ],
      ),
    );
  }

  // Feeding totalled per day, liquid (ml) vs solid food (g) as two lines -
  // the two use different units so they can't be summed into one number,
  // but plotted like the symptom chart the exact daily amount of each
  // reads straight off the axis instead of needing to compare bar heights.
  final mlByDay = <String, double>{};
  final gByDay = <String, double>{};
  for (final e in feeding) {
    final amount = (e['amountMl'] as num?)?.toDouble();
    if (amount == null || amount <= 0) continue;
    final day = _shortDate(e['feedDate']);
    if ((e['amountUnit'] ?? 'ml').toString().toLowerCase() == 'g') {
      gByDay[day] = (gByDay[day] ?? 0) + amount;
    } else {
      mlByDay[day] = (mlByDay[day] ?? 0) + amount;
    }
  }
  final feedingDays = _lastN(
    ({...mlByDay.keys, ...gByDay.keys}.toList()..sort()),
    _maxChartPoints,
  );
  if (feedingDays.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Hranjenje (po danu)',
        unit: 'ml / g',
        yMin: 0,
        tickStep: 100,
        series: [
          ReportChartSeries(
            legend: 'Tečna hrana (ml)',
            color: PdfColors.blue,
            points: [
              for (final d in feedingDays) ChartPoint(_chartDate(d), mlByDay[d] ?? 0),
            ],
          ),
          ReportChartSeries(
            legend: 'Čvrsta hrana (g)',
            color: PdfColors.pink,
            points: [
              for (final d in feedingDays) ChartPoint(_chartDate(d), gByDay[d] ?? 0),
            ],
          ),
        ],
      ),
    );
  }

  // One line per state (Mokra/Stolica/Kombinovano), same style as the
  // symptom chart - a stacked bar showed the total at a glance but made the
  // per-state counts something you had to estimate visually; a line per
  // state reads the exact daily count straight off the axis.
  final diaperCountsByDay = <String, Map<String, int>>{};
  for (final e in diapers) {
    final day = _shortDate(e['changeDate']);
    final state = (e['diaperState'] ?? '').toString().toLowerCase();
    final counts = diaperCountsByDay.putIfAbsent(
      day,
      () => {'mokra': 0, 'stolica': 0, 'kombinovano': 0},
    );
    if (counts.containsKey(state)) counts[state] = counts[state]! + 1;
  }
  final diaperDays = _lastN(diaperCountsByDay.keys.toList()..sort(), _maxChartPoints);
  if (diaperDays.isNotEmpty) {
    charts.add(
      ReportChart(
        title: 'Pelene (po danu)',
        unit: 'broj promjena',
        yMin: 0,
        integerTicks: true,
        series: [
          ReportChartSeries(
            legend: 'Mokra',
            color: PdfColors.blue,
            points: [
              for (final d in diaperDays)
                ChartPoint(_chartDate(d), (diaperCountsByDay[d]!['mokra'] ?? 0).toDouble()),
            ],
          ),
          ReportChartSeries(
            legend: 'Stolica',
            color: PdfColors.brown,
            points: [
              for (final d in diaperDays)
                ChartPoint(_chartDate(d), (diaperCountsByDay[d]!['stolica'] ?? 0).toDouble()),
            ],
          ),
          ReportChartSeries(
            legend: 'Kombinovano',
            color: PdfColors.orange,
            points: [
              for (final d in diaperDays)
                ChartPoint(
                  _chartDate(d),
                  (diaperCountsByDay[d]!['kombinovano'] ?? 0).toDouble(),
                ),
            ],
          ),
        ],
      ),
    );
  }

  return charts;
}

// One row per food, not one row per time it was tried - a rating describes
// the baby's current reaction to that food, so only the most recent try
// counts, matching how the app itself treats a re-tried food (update the
// same rating, not a new history entry).
ReportTable _buildMealRatingsTable(List meals) {
  final latestByFood = <String, dynamic>{};
  for (final e in meals) {
    final food = (e['foodName'] ?? '-').toString();
    final triedAt = (e['triedAt'] ?? '').toString();
    final existing = latestByFood[food];
    if (existing == null || triedAt.compareTo((existing['triedAt'] ?? '').toString()) > 0) {
      latestByFood[food] = e;
    }
  }
  final entries = latestByFood.entries.toList()
    ..sort((a, b) => a.key.compareTo(b.key));

  return ReportTable(
    title: 'Hrana (ocjene)',
    headers: const ['Namirnica', 'Zadnja ocjena', 'Datum'],
    rows: [
      for (final e in entries)
        [
          e.key,
          '${e.value['rating'] ?? '-'}/5',
          _shortDate(e.value['triedAt']),
        ],
    ],
  );
}

List<ReportTable> _buildBabyTables(
  List meals,
  List health,
  List diapers,
  List sleep,
  List feeding,
  List milestones,
  List calendar,
) {
  return [
    ReportTable(
      title: 'Zdravlje',
      headers: const ['Datum', 'Temperatura', 'Lijekovi', 'Posjeta doktoru'],
      rows: [
        for (final e in health)
          [
            _shortDate(e['entryDate']),
            e['temperatureC'] != null ? '${e['temperatureC']} °C' : '-',
            (e['medicines'] ?? '-').toString(),
            (e['doctorVisit'] ?? '-').toString(),
          ],
      ],
    ),
    _buildMealRatingsTable(meals),
    ReportTable(
      title: 'Dostignuća',
      headers: const ['Datum', 'Dostignuće', 'Napomena'],
      rows: [
        for (final e in milestones)
          [
            _shortDate(e['achievedDate']),
            (e['title'] ?? '-').toString(),
            (e['notes'] ?? '-').toString(),
          ],
      ],
    ),
    ReportTable(
      title: 'Događaji',
      headers: const ['Datum', 'Naslov', 'Opis'],
      rows: [
        for (final e in calendar)
          [
            _shortDate(e['startAt']),
            (e['title'] ?? '-').toString(),
            (e['description'] ?? '-').toString(),
          ],
      ],
    ),
  ];
}
