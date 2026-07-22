import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_widgets.dart';
import 'package:flutter_application_nestly/providers/notification_signalr_service.dart';
import 'package:flutter_application_nestly/providers/notification_state.dart';
import 'package:flutter_application_nestly/screens/advice_center_screen.dart';
import 'package:flutter_application_nestly/screens/baby_growth_screen.dart';
import 'package:flutter_application_nestly/screens/baby_profile_create_screen.dart';
import 'package:flutter_application_nestly/screens/baby_time_home_screen.dart';
import 'package:flutter_application_nestly/screens/blog_module_screen.dart';
import 'package:flutter_application_nestly/screens/chat_home_screen.dart';
import 'package:flutter_application_nestly/screens/edit_profile_screen.dart';
import 'package:flutter_application_nestly/screens/notifications_screen.dart';
import 'package:flutter_application_nestly/screens/qa_module_screen.dart';
import 'package:flutter_application_nestly/screens/symptom_diary_screen.dart';
import 'package:flutter_application_nestly/screens/therapy_module_screen.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

class HomeDashboardScreen extends StatefulWidget {
  const HomeDashboardScreen({super.key});

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
    _refreshBadge();
  }

  void _refreshBadge() async {
    await notificationState.loadUnreadCount();
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
      await _signalRService.connect(
        token,
        onNotification: () async {
          await notificationState.loadUnreadCount();
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
      final resp = await ApiClient.get('/api/Pregnancy/my-status');

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
      final resp = await ApiClient.get('/api/BabyProfile/my-latest');

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
        gender: _normalizeGender(_gender),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
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
                    right: 8,
                    top: 8,
                    child: Container(
                      padding: count > 0
                          ? const EdgeInsets.symmetric(
                              horizontal: 6,
                              vertical: 2,
                            )
                          : const EdgeInsets.all(6),
                      decoration: BoxDecoration(
                        color: AppColors.roseDark,
                        borderRadius: BorderRadius.circular(12),
                      ),
                      constraints: const BoxConstraints(
                        minWidth: 18,
                        minHeight: 18,
                      ),
                      child: Center(
                        child: Text(
                          count > 9 ? '9+' : count.toString(),
                          style: const TextStyle(
                            color: Colors.white,
                            fontSize: 10,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                      ),
                    ),
                  );
                },
              ),
            ],
          ),
          IconButton(
            icon: const Icon(Icons.account_circle_rounded),
            onPressed: () async {
              final token = await AuthStorage.getToken();
              if (token == null) return;
              Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => EditProfileScreen()),
              );
            },
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
                    _hasBaby
                        ? _CtaBanner(
                            icon: Icons.child_friendly_rounded,
                            label: 'Vrijeme je za bebu',
                            subtitle:
                                'Praćenje rasta, ishrane, sna i još mnogo toga',
                            gradient: AppGradients.brandCool,
                            onTap: _openBabyTime,
                          )
                        : _CtaBanner(
                            icon: Icons.favorite_rounded,
                            label: 'Beba je rođena',
                            subtitle:
                                'Kreirajte profil bebe i nastavite putovanje',
                            gradient: AppGradients.brandCool,
                            onTap: () async {
                              await Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (_) => BabyProfileCreateScreen(),
                                ),
                              );

                              await _loadBabyStatus();
                            },
                          ),
                    const SizedBox(height: AppSpacing.xl),
                    const NestlySectionHeader(title: 'Vaš meni'),
                    const SizedBox(height: AppSpacing.sm),
                    _buildMenuGrid(context),
                    const SizedBox(height: AppSpacing.lg),
                    _logoutRow(context),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildMenuGrid(BuildContext context) {
    final items = <_MenuItem>[
      _MenuItem(
        Icons.local_florist_rounded,
        'Veličina ploda',
        () => _open(context, BabyGrowthScreen(week: _week)),
      ),
      _MenuItem(
        Icons.fact_check_rounded,
        'Dnevnik simptoma',
        () => _open(context, SymptomDiaryScreen()),
      ),
      _MenuItem(
        Icons.lightbulb_outline_rounded,
        'Savjetni centar',
        () => _open(context, AdviceCenterScreen(gestationalWeek: _week)),
      ),
      _MenuItem(
        Icons.article_outlined,
        'Blog',
        () => _open(context, const BlogScreen()),
      ),
      _MenuItem(
        Icons.help_outline_rounded,
        'Pitanja',
        () => _open(context, MyQuestionsScreen()),
      ),
      _MenuItem(
        Icons.medical_services_outlined,
        'Terapija',
        () => _open(context, TherapyCalendarScreen()),
      ),
      _MenuItem(
        Icons.chat_bubble_outline_rounded,
        'Chat',
        () => _open(context, ChatHomeScreen()),
      ),
    ];

    return Column(
      // Simple list, one row under another, all in the brand rose color.
      children: [
        for (var i = 0; i < items.length; i++)
          Padding(
            padding: EdgeInsets.only(
              bottom: i == items.length - 1 ? 0 : AppSpacing.sm,
            ),
            child: NestlyMenuRow(
              icon: items[i].icon,
              label: items[i].label,
              color: AppColors.roseDark,
              onTap: items[i].onTap,
            ),
          ),
      ],
    );
  }

  Widget _logoutRow(BuildContext context) {
    return TextButton.icon(
      onPressed: () async {
        await _signalRService.disconnect();
        notificationState.reset();
        await AuthStorage.clear();

        if (!context.mounted) return;

        Navigator.of(context).pushAndRemoveUntil(
          MaterialPageRoute(builder: (_) => const LoginScreen()),
          (_) => false,
        );
      },
      style: TextButton.styleFrom(
        foregroundColor: AppColors.textSecondary,
        padding: const EdgeInsets.symmetric(vertical: AppSpacing.md),
      ),
      icon: const Icon(Icons.logout_rounded, size: 18),
      label: const Text(
        'Odjavi se',
        style: TextStyle(fontWeight: FontWeight.w600),
      ),
    );
  }
}

class _MenuItem {
  final IconData icon;
  final String label;
  final VoidCallback onTap;

  _MenuItem(this.icon, this.label, this.onTap);
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
    return NestlyHeroHeader(
      padding: const EdgeInsets.fromLTRB(
        AppSpacing.xl,
        AppSpacing.md,
        AppSpacing.xl,
        AppSpacing.xl,
      ),
      child: loading
          ? const Center(
              child: CircularProgressIndicator(color: Colors.white),
            )
          : Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                    fontWeight: FontWeight.w800,
                    color: Colors.white,
                  ),
                ),
                const SizedBox(height: AppSpacing.sm),
                Text(
                  subtitle,
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    color: Colors.white.withOpacity(.9),
                    fontWeight: FontWeight.w600,
                  ),
                ),
                const SizedBox(height: AppSpacing.lg),
                ClipRRect(
                  borderRadius: BorderRadius.circular(20),
                  child: LinearProgressIndicator(
                    value: progress.clamp(0, 1),
                    minHeight: 10,
                    color: Colors.white,
                    backgroundColor: Colors.white.withOpacity(.25),
                  ),
                ),
                const SizedBox(height: 6),
                Text(
                  '${week.clamp(1, 40)} / 40 sedmica • ${(progress * 100).round()}%',
                  style: Theme.of(context).textTheme.bodySmall?.copyWith(
                    color: Colors.white.withOpacity(.9),
                  ),
                ),
              ],
            ),
    );
  }
}

class _CtaBanner extends StatelessWidget {
  const _CtaBanner({
    required this.icon,
    required this.label,
    required this.subtitle,
    required this.onTap,
    this.gradient = AppGradients.brandCool,
  });

  final IconData icon;
  final String label;
  final String subtitle;
  final VoidCallback onTap;
  final Gradient gradient;

  @override
  Widget build(BuildContext context) {
    return Material(
      color: Colors.transparent,
      borderRadius: BorderRadius.circular(AppRadius.xl),
      clipBehavior: Clip.antiAlias,
      child: InkWell(
        onTap: onTap,
        child: Ink(
          decoration: BoxDecoration(gradient: gradient),
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              Container(
                width: 52,
                height: 52,
                decoration: BoxDecoration(
                  color: Colors.white.withOpacity(.2),
                  shape: BoxShape.circle,
                ),
                child: Icon(icon, color: Colors.white, size: 28),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      label,
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.w800,
                        color: Colors.white,
                      ),
                    ),
                    const SizedBox(height: 2),
                    Text(
                      subtitle,
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: Colors.white.withOpacity(.9),
                      ),
                    ),
                  ],
                ),
              ),
              const Icon(Icons.chevron_right_rounded, color: Colors.white),
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
