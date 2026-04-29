import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
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
import 'package:flutter_application_nestly/screens/health_tracking_screen.dart';
import 'package:flutter_application_nestly/screens/meal_plan_screen.dart';
import 'package:flutter_application_nestly/screens/milestone_screen.dart';
import 'package:flutter_application_nestly/screens/sleep_log_screen.dart';

class BabyTimeHomeScreen extends StatefulWidget {
  final String babyName;
  final int babyId;
  final int parentProfileId;
  final String gender;

  const BabyTimeHomeScreen({
    super.key,
    required this.babyName,
    required this.babyId,
    required this.parentProfileId,
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

      final decoded = JwtDecoder.decode(token);
      final userId = decoded["userId"];

      await _signalRService.connect(
        userId.toString(),
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
        iconTheme: const IconThemeData(color: AppColors.roseDark),
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
                      padding: const EdgeInsets.symmetric(
                        horizontal: 6,
                        vertical: 2,
                      ),
                      decoration: BoxDecoration(
                        color: AppColors.roseDark,
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
              _HeaderCard(babyName: _babyName ?? '', isGirl: _isGirl),
              const SizedBox(height: AppSpacing.xl),

              _menu(
                AppColors.seed,
                Icons.show_chart_rounded,
                'Praćenje rasta',
                () => _push(
                  context,
                  BabyGrowthTrackerScreen(
                    babyId: widget.babyId,
                    babyName: _babyName ?? widget.babyName,
                  ),
                ),
              ),

              _menu(
                AppColors.roseDark,
                Icons.restaurant_rounded,
                'Plan ishrane',
                () => _push(
                  context,
                  MealRecommendationScreen(babyId: widget.babyId),
                ),
              ),

              _menu(
                AppColors.seed,
                Icons.local_drink_rounded,
                'Dnevnik hranjenja',
                () => _push(
                  context,
                  FeedingCalendarScreen(babyId: widget.babyId),
                ),
              ),

              _menu(
                AppColors.roseDark,
                Icons.favorite_border_rounded,
                'Praćenje zdravlja',
                () => _push(
                  context,
                  HealthTrackingScreen(
                    babyId: widget.babyId,
                    babyName: _babyName ?? widget.babyName,
                    userId: widget.parentProfileId,
                  ),
                ),
              ),

              _menu(
                AppColors.seed,
                Icons.nights_stay_rounded,
                'Dnevnik spavanja',
                () => _push(
                  context,
                  SleepLogOverviewScreen(
                    babyId: widget.babyId,
                    babyName: _babyName ?? widget.babyName,
                  ),
                ),
              ),

              _menu(
                AppColors.roseDark,
                Icons.baby_changing_station_rounded,
                'Praćenje pelena',
                () => _push(
                  context,
                  DiaperLogCalendarScreen(babyId: widget.babyId),
                ),
              ),

              _menu(
                AppColors.seed,
                Icons.emoji_events_rounded,
                'Dostignuća',
                () => _push(
                  context,
                  MilestoneScreen(
                    babyId: widget.babyId,
                    babyName: _babyName ?? widget.babyName,
                  ),
                ),
              ),

              _menu(
                AppColors.roseDark,
                Icons.chat_bubble_outline_rounded,
                'Chat',
                () => _push(
                  context,
                  ChatHomeScreen(currentUserId: widget.parentProfileId),
                ),
              ),

              _menu(
                AppColors.seed,
                Icons.event_note_rounded,
                'Kalendar termina',
                () => _push(
                  context,
                  CalendarEventScreen(
                    babyId: widget.babyId,
                    babyName: _babyName ?? widget.babyName,
                    userId: widget.parentProfileId,
                  ),
                ),
              ),

              const SizedBox(height: AppSpacing.xl),
              _BackCard(onTap: () => Navigator.pop(context)),
            ],
          ),
        ),
      ),
    );
  }

  Widget _menu(Color color, IconData icon, String label, VoidCallback onTap) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Card(
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
                Icon(icon, color: color, size: 28),
                const SizedBox(width: AppSpacing.lg),
                Expanded(
                  child: Text(
                    label,
                    style: Theme.of(context).textTheme.titleMedium?.copyWith(
                      fontWeight: FontWeight.w700,
                      color: color,
                    ),
                  ),
                ),
                Icon(Icons.chevron_right_rounded, color: color),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _HeaderCard extends StatelessWidget {
  final String babyName;
  final bool isGirl;

  const _HeaderCard({required this.babyName, required this.isGirl});

  @override
  Widget build(BuildContext context) {
    final headerColor = isGirl ? AppColors.babyPink : AppColors.babyBlue;

    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Ink(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(AppRadius.xl),
          gradient: LinearGradient(
            colors: [headerColor.withOpacity(.18), AppColors.card],
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
                decoration: BoxDecoration(
                  color: headerColor,
                  shape: BoxShape.circle,
                ),
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
                        color: headerColor,
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
  final VoidCallback onTap;

  const _BackCard({required this.onTap});

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
                  color: AppColors.roseDark.withOpacity(.12),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.arrow_back_rounded,
                  color: AppColors.roseDark,
                ),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Text(
                  'Povratak u mamin svijet',
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
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
