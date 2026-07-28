import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_widgets.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/providers/notification_signalr_service.dart';
import 'package:flutter_application_nestly/providers/notification_state.dart';
import 'package:flutter_application_nestly/screens/edit_baby_profile_screen.dart';
import 'package:flutter_application_nestly/screens/notifications_screen.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:flutter_application_nestly/screens/baby_growth_tracker_screen.dart';
import 'package:flutter_application_nestly/screens/calendar_event_screen.dart';
import 'package:flutter_application_nestly/screens/chat_home_screen.dart';
import 'package:flutter_application_nestly/screens/diaper_log_calendar_screen.dart';
import 'package:flutter_application_nestly/screens/feeding_calendar_screen.dart';
import 'package:flutter_application_nestly/screens/health_alerts_screen.dart';
import 'package:flutter_application_nestly/screens/health_tracking_screen.dart';
import 'package:flutter_application_nestly/screens/meal_plan_screen.dart';
import 'package:flutter_application_nestly/screens/milestone_screen.dart';
import 'package:flutter_application_nestly/screens/sleep_log_screen.dart';

class BabyTimeHomeScreen extends StatefulWidget {
  final String babyName;
  final int babyId;
  final String gender;

  const BabyTimeHomeScreen({
    super.key,
    required this.babyName,
    required this.babyId,
    required this.gender,
  });

  @override
  State<BabyTimeHomeScreen> createState() => _BabyTimeHomeScreenState();
}

class _BabyTimeHomeScreenState extends State<BabyTimeHomeScreen> {
  final NotificationSignalRService _signalRService =
      NotificationSignalRService();
  String? _babyName;
  bool get _isGirl {
    final g = widget.gender.toLowerCase();
    return g == 'female' || g == 'f';
  }

  Color get _accent => _isGirl ? AppColors.roseDark : AppColors.seed;

  @override
  void initState() {
    super.initState();
    notificationState.loadUnreadCount();
    _babyName = widget.babyName;
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

  Future<void> _reloadBaby() async {
    final resp = await ApiClient.get('/api/BabyProfile/${widget.babyId}');
    final data = jsonDecode(resp.body);

    setState(() {
      _babyName = data['babyName'];
    });
  }

  void _push(BuildContext context, Widget screen) {
    if (!mounted) return;
    Navigator.of(context).push(MaterialPageRoute(builder: (_) => screen));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: IconThemeData(color: _accent),
        actions: [
          Stack(
            children: [
              IconButton(
                icon: const Icon(Icons.notifications_rounded),
                onPressed: () async {
                  await Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (_) => NotificationsScreen(
                        accent: _accent,
                        soft: _isGirl ? AppColors.babyPink : AppColors.babyBlue,
                      ),
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
                      padding: const EdgeInsets.symmetric(
                        horizontal: 6,
                        vertical: 2,
                      ),
                      decoration: BoxDecoration(
                        color: _accent,
                        borderRadius: BorderRadius.circular(12),
                      ),
                      constraints: const BoxConstraints(
                        minWidth: 18,
                        minHeight: 18,
                      ),
                      child: Text(
                        count > 9 ? '9+' : count.toString(),
                        style: const TextStyle(
                          color: Colors.white,
                          fontSize: 10,
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

          IconButton(
            icon: const Icon(Icons.account_circle_rounded),
            onPressed: () async {
              final result = await Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (_) => EditBabyProfileScreen(babyId: widget.babyId),
                ),
              );

              if (result == true) {
                await _reloadBaby();
              }
            },
          ),
        ],
      ),

      body: SafeArea(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              _HeaderCard(
                babyName: _babyName ?? '',
                accent: _accent,
                soft: _isGirl ? AppColors.babyPink : AppColors.babyBlue,
              ),
              const SizedBox(height: AppSpacing.xl),
              const NestlySectionHeader(title: 'Aktivnosti bebe'),
              const SizedBox(height: AppSpacing.sm),
              _buildMenuGrid(context),

              const SizedBox(height: AppSpacing.xl),
              _BackCard(accent: _accent, onTap: () => Navigator.pop(context)),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildMenuGrid(BuildContext context) {
    final items = <_BabyMenuItem>[
      _BabyMenuItem(
        Icons.show_chart_rounded,
        'Praćenje rasta',
        () => _push(
          context,
          BabyGrowthTrackerScreen(
            babyId: widget.babyId,
            babyName: _babyName ?? widget.babyName,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.restaurant_rounded,
        'Plan ishrane',
        () => _push(
          context,
          MealRecommendationScreen(
            babyId: widget.babyId,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.local_drink_rounded,
        'Dnevnik hranjenja',
        () => _push(
          context,
          FeedingCalendarScreen(babyId: widget.babyId, gender: widget.gender),
        ),
      ),
      _BabyMenuItem(
        Icons.favorite_border_rounded,
        'Praćenje zdravlja',
        () => _push(
          context,
          HealthTrackingScreen(
            babyId: widget.babyId,
            babyName: _babyName ?? widget.babyName,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.nights_stay_rounded,
        'Dnevnik spavanja',
        () => _push(
          context,
          SleepLogOverviewScreen(
            babyId: widget.babyId,
            babyName: _babyName ?? widget.babyName,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.baby_changing_station_rounded,
        'Praćenje pelena',
        () => _push(
          context,
          DiaperLogCalendarScreen(
            babyId: widget.babyId,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.health_and_safety_rounded,
        'Zdravstvena upozorenja',
        () => _push(
          context,
          HealthAlertsScreen(
            babyId: widget.babyId,
            babyName: _babyName ?? widget.babyName,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.emoji_events_rounded,
        'Dostignuća',
        () => _push(
          context,
          MilestoneScreen(
            babyId: widget.babyId,
            babyName: _babyName ?? widget.babyName,
            gender: widget.gender,
          ),
        ),
      ),
      _BabyMenuItem(
        Icons.chat_bubble_outline_rounded,
        'Chat',
        () => _push(context, ChatHomeScreen(gender: widget.gender)),
      ),
      _BabyMenuItem(
        Icons.event_note_rounded,
        'Kalendar termina',
        () => _push(
          context,
          CalendarEventScreen(
            babyId: widget.babyId,
            babyName: _babyName ?? widget.babyName,
            gender: widget.gender,
          ),
        ),
      ),
    ];

    return Column(
      // Simple list, one row under another, all in the baby's accent color
      // (dark rose for a girl, dark blue for a boy).
      children: [
        for (var i = 0; i < items.length; i++)
          Padding(
            padding: EdgeInsets.only(
              bottom: i == items.length - 1 ? 0 : AppSpacing.sm,
            ),
            child: NestlyMenuRow(
              icon: items[i].icon,
              label: items[i].label,
              color: _accent,
              onTap: items[i].onTap,
            ),
          ),
      ],
    );
  }
}

class _BabyMenuItem {
  final IconData icon;
  final String label;
  final VoidCallback onTap;

  _BabyMenuItem(this.icon, this.label, this.onTap);
}

class _HeaderCard extends StatelessWidget {
  final String babyName;
  final Color accent;
  final Color soft;

  const _HeaderCard({
    required this.babyName,
    required this.accent,
    required this.soft,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Ink(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(AppRadius.xl),
          gradient: LinearGradient(
            colors: [soft.withOpacity(.18), AppColors.card],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Row(
            children: [
              Container(
                width: 64,
                height: 64,
                decoration: BoxDecoration(color: accent, shape: BoxShape.circle),
                child: const Icon(
                  Icons.child_care_rounded,
                  color: Colors.white,
                  size: 32,
                ),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      babyName,
                      style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        fontWeight: FontWeight.w700,
                        color: accent,
                      ),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      'Sve važne stvari o vašoj bebi na jednom mjestu',
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.textSecondary,
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _BackCard extends StatelessWidget {
  final Color accent;
  final VoidCallback onTap;

  const _BackCard({required this.accent, required this.onTap});

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
              Container(
                width: 46,
                height: 46,
                decoration: BoxDecoration(
                  color: accent.withOpacity(.12),
                  shape: BoxShape.circle,
                ),
                child: Icon(Icons.arrow_back_rounded, color: accent),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Text(
                  'Povratak u mamin svijet',
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: accent,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
