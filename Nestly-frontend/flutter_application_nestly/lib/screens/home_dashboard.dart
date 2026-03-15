import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/providers/notification_signalr_service.dart';
import 'package:flutter_application_nestly/providers/notification_state.dart';
import 'package:flutter_application_nestly/screens/advice_center_screen.dart';
import 'package:flutter_application_nestly/screens/baby_growth_screen.dart';
import 'package:flutter_application_nestly/screens/baby_profile_create_screen.dart';
import 'package:flutter_application_nestly/screens/baby_time_home_screen.dart';
import 'package:flutter_application_nestly/screens/blog_module_screen.dart';
import 'package:flutter_application_nestly/screens/chat_home_screen.dart';
import 'package:flutter_application_nestly/screens/notifications_screen.dart';
import 'package:flutter_application_nestly/screens/qa_module_screen.dart';
import 'package:flutter_application_nestly/screens/symptom_diary_screen.dart';
import 'package:flutter_application_nestly/screens/therapy_module_screen.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

class HomeDashboardScreen extends StatefulWidget {
  const HomeDashboardScreen({super.key, required this.parentProfileId});

  final int parentProfileId;
  @override
  State<HomeDashboardScreen> createState() => _HomeDashboardScreenState();
}

class _HomeDashboardScreenState extends State<HomeDashboardScreen> {
  int? _gestationalWeek;
  int? _daysRemaining;
  DateTime? _lmpDate;
  DateTime? _dueDate;

  bool _loading = true;
  bool _error = false;
  final NotificationSignalRService _signalRService =
      NotificationSignalRService();
  bool _checkingBaby = false;
  bool _hasBaby = false;
  int? _babyId;
  String? _babyName;
  String? _gender;
  @override
  void initState() {
    super.initState();
    _loadPregnancyStatus();
    _loadBabyStatus();
    notificationState.loadUnreadCount();
    _initSignalR();
  }

  @override
  void dispose() {
    _signalRService.disconnect();
    super.dispose();
  }

  Future<void> _initSignalR() async {
    try {
      final token = await AuthStorage.getToken();
      if (token == null) return;

      final decoded = JwtDecoder.decode(token);
      final userId =
          decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

      await _signalRService.connect(
        userId.toString(),
        token,
        onNotification: () {
          notificationState.increment();
        },
      );
    } catch (e) {
      debugPrint('SignalR error: $e');
    }
  }

  Future<void> _loadPregnancyStatus() async {
    setState(() {
      _loading = true;
      _error = false;
    });

    try {
      final resp = await ApiClient.get(
        '/api/Pregnancy/status?parentProfileId=${widget.parentProfileId}',
      );

      if (!mounted) return;

      if (resp.statusCode == 200) {
        final data = jsonDecode(resp.body) as Map<String, dynamic>;

        setState(() {
          _gestationalWeek = data['gestationalWeek'];
          _daysRemaining = data['daysRemaining'];

          _lmpDate = data['lmpDate'] != null
              ? DateTime.parse(data['lmpDate'])
              : null;

          _dueDate = data['dueDate'] != null
              ? DateTime.parse(data['dueDate'])
              : null;
        });
      } else {
        setState(() => _error = true);
      }
    } catch (_) {
      if (!mounted) return;
      setState(() => _error = true);
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  Future<void> _loadBabyStatus() async {
    setState(() => _checkingBaby = true);

    try {
      final resp = await ApiClient.get(
        '/api/BabyProfile/latest-by-parent/${widget.parentProfileId}',
      );

      if (!mounted) return;

      if (resp.statusCode == 200) {
        final data = jsonDecode(resp.body) as Map<String, dynamic>;

        setState(() {
          _hasBaby = true;
          _babyId = data['id'];
          _babyName = data['babyName'];
          _gender = data['gender'];
        });
      } else {
        setState(() => _hasBaby = false);
      }
    } catch (_) {
      if (!mounted) return;
      setState(() => _hasBaby = false);
    } finally {
      if (mounted) setState(() => _checkingBaby = false);
    }
  }

  int get _week => (_gestationalWeek ?? 1).clamp(1, 40);

  double get _progress {
    if (_daysRemaining == null) return 0;
    const total = 280;
    final done = (total - _daysRemaining!).clamp(0, total);
    return done / total;
  }

  String get _subtitle {
    if (_error) return 'Nije moguće učitati podatke o trudnoći.';
    if (_gestationalWeek == null || _daysRemaining == null) {
      return 'Nema podataka o trudnoći. Dodajte informacije u profilu.';
    }
    if (_daysRemaining! > 0) {
      return 'Preostalo ${_daysRemaining!} dana';
    }
    return 'Termin je prošao';
  }

  void _open(BuildContext context, Widget page) {
    Navigator.of(context).push(MaterialPageRoute(builder: (_) => page));
  }

  void _openBabyTime() {
    if (_babyId == null) return;

    _open(
      context,
      BabyTimeHomeScreen(
        babyName: _babyName ?? 'Vaša beba',
        babyId: _babyId!,
        parentProfileId: widget.parentProfileId,
        gender: _normalizeGender(_gender),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        actions: [
          Stack(
            children: [
              IconButton(
                icon: const Icon(Icons.notifications_rounded),
                onPressed: () async {
                  await Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (_) => const NotificationsScreen(),
                    ),
                  );

                  notificationState.loadUnreadCount();
                },
              ),
              AnimatedBuilder(
                animation: notificationState,
                builder: (_, __) {
                  final count = notificationState.unreadCount;

                  if (count == 0) return const SizedBox();

                  return Positioned(
                    right: 10,
                    top: 10,
                    child: Container(
                      padding: const EdgeInsets.all(4),
                      decoration: const BoxDecoration(
                        color: AppColors.roseDark,
                        shape: BoxShape.circle,
                      ),
                      constraints: const BoxConstraints(
                        minWidth: 18,
                        minHeight: 18,
                      ),
                      child: Text(
                        count > 9 ? '9+' : count.toString(),
                        style: const TextStyle(
                          color: Colors.white,
                          fontSize: 11,
                          fontWeight: FontWeight.bold,
                        ),
                        textAlign: TextAlign.center,
                      ),
                    ),
                  );
                },
              ),
            ],
          ),
        ],
      ),
      body: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onHorizontalDragEnd: (details) {
          if (details.primaryVelocity != null &&
              details.primaryVelocity! < -300) {
            if (_checkingBaby || !_hasBaby) return;
            _openBabyTime();
          }
        },
        child: SafeArea(
          child: Center(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: ConstrainedBox(
                constraints: const BoxConstraints(maxWidth: 520),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    _HeaderSimple(
                      title: 'Sedmica $_week',
                      subtitle: _subtitle,
                      progress: _progress,
                      loading: _loading,
                      week: _week,
                    ),
                    const SizedBox(height: AppSpacing.xl),
                    _menu(
                      icon: Icons.local_florist_rounded,
                      label: 'Veličina ploda',
                      onTap: () =>
                          _open(context, BabyGrowthScreen(week: _week)),
                    ),
                    _menu(
                      icon: Icons.fact_check_rounded,
                      label: 'Dnevnik simptoma',
                      onTap: () => _open(
                        context,
                        SymptomDiaryScreen(
                          parentProfileId: widget.parentProfileId,
                        ),
                      ),
                    ),
                    _menu(
                      icon: Icons.lightbulb_outline_rounded,
                      label: 'Savjetni centar',
                      onTap: () => _open(
                        context,
                        AdviceCenterScreen(gestationalWeek: _week),
                      ),
                    ),
                    _menu(
                      icon: Icons.article_outlined,
                      label: 'Blog',
                      onTap: () => _open(
                        context,
                        BlogScreen(parentProfileId: widget.parentProfileId),
                      ),
                    ),
                    _menu(
                      icon: Icons.help_outline_rounded,
                      label: 'Pitanja',
                      onTap: () => _open(
                        context,
                        MyQuestionsScreen(
                          parentProfileId: widget.parentProfileId,
                        ),
                      ),
                    ),
                    _menu(
                      icon: Icons.medical_services_outlined,
                      label: 'Terapija',
                      onTap: () => _open(
                        context,
                        TherapyCalendarScreen(
                          parentProfileId: widget.parentProfileId,
                        ),
                      ),
                    ),
                    _menu(
                      icon: Icons.chat_bubble_outline_rounded,
                      label: 'Chat',
                      onTap: () => _open(
                        context,
                        ChatHomeScreen(currentUserId: widget.parentProfileId),
                      ),
                    ),
                    _menu(
                      icon: Icons.logout_rounded,
                      label: 'Odjavi se',
                      onTap: () async {
                        await _signalRService.disconnect();
                        notificationState.reset();
                        await AuthStorage.clear();

                        if (!context.mounted) return;

                        Navigator.of(context).pushAndRemoveUntil(
                          MaterialPageRoute(
                            builder: (_) => const LoginScreen(),
                          ),
                          (_) => false,
                        );
                      },
                    ),

                    const SizedBox(height: AppSpacing.xl),
                    _hasBaby
                        ? _GradientPinkCard(
                            icon: Icons.child_friendly_rounded,
                            label: 'Vrijeme je za bebu',
                            onTap: _openBabyTime,
                          )
                        : _GradientPinkCard(
                            icon: Icons.favorite_rounded,
                            label: 'Beba je rođena',
                            onTap: () async {
                              await Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (_) => BabyProfileCreateScreen(
                                    parentProfileId: widget.parentProfileId,
                                  ),
                                ),
                              );

                              await _loadBabyStatus();
                            },
                          ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _menu({
    required IconData icon,
    required String label,
    required VoidCallback onTap,
  }) => Padding(
    padding: const EdgeInsets.only(bottom: AppSpacing.md),
    child: _GradientPinkCard(icon: icon, label: label, onTap: onTap),
  );
}

class _HeaderSimple extends StatelessWidget {
  const _HeaderSimple({
    required this.title,
    required this.subtitle,
    required this.progress,
    required this.loading,
    required this.week,
  });

  final String title;
  final String subtitle;
  final double progress;
  final bool loading;
  final int week;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: AppColors.card,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: loading
            ? const Center(child: CircularProgressIndicator())
            : Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                      fontWeight: FontWeight.w700,
                      color: AppColors.roseDark,
                    ),
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  Text(
                    subtitle,
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: AppColors.textSecondary,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  LinearProgressIndicator(
                    value: progress.clamp(0, 1),
                    minHeight: 10,
                    color: AppColors.roseDark,
                    backgroundColor: AppColors.babyPink.withOpacity(.3),
                  ),
                  const SizedBox(height: 6),
                  Text(
                    '${week.clamp(1, 40)} / 40 sedmica • ${(progress * 100).round()}%',
                    style: Theme.of(context).textTheme.bodySmall?.copyWith(
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
      ),
    );
  }
}

class _GradientPinkCard extends StatelessWidget {
  const _GradientPinkCard({
    required this.icon,
    required this.label,
    required this.onTap,
  });

  final IconData icon;
  final String label;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppRadius.lg),
        child: Ink(
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              Icon(icon, color: AppColors.roseDark, size: 28),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Text(
                  label,
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
                  ),
                ),
              ),
              const Icon(
                Icons.chevron_right_rounded,
                color: AppColors.roseDark,
              ),
            ],
          ),
        ),
      ),
    );
  }
}

String _normalizeGender(String? value) {
  if (value == null) return 'unknown';

  final g = value.toLowerCase();

  if (g == 'female') return 'female';
  if (g == 'male') return 'male';

  return 'unknown';
}
