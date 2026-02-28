import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';

class NotificationsScreen extends StatefulWidget {
  const NotificationsScreen({super.key});

  @override
  State<NotificationsScreen> createState() => _NotificationsScreenState();
}

class _NotificationsScreenState extends State<NotificationsScreen> {
  List<dynamic> _notifications = [];
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<int> _getUserId() async {
    final token = await AuthStorage.getToken();
    final decoded = JwtDecoder.decode(token!);
    return int.parse(
      decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
          .toString(),
    );
  }

  Future<void> _load() async {
    try {
      final userId = await _getUserId();
      final res = await ApiClient.get('/api/Notification/$userId');

      if (res.statusCode == 200) {
        final all = jsonDecode(res.body) as List<dynamic>;

        final threeDaysAgo = DateTime.now().subtract(const Duration(days: 3));

        final filtered = all.where((n) {
          if (n["createdAt"] == null) return false;

          final date = DateTime.parse(n["createdAt"]);
          return date.isAfter(threeDaysAgo);
        }).toList();

        setState(() {
          _notifications = filtered;
          _loading = false;
        });
      }
    } catch (_) {
      setState(() => _loading = false);
    }
  }

  Future<void> _markAsRead(int id) async {
    await ApiClient.post('/api/Notification/mark-as-read/$id');
    await _load();
  }

  Future<void> _markAllAsRead() async {
    for (var n in _notifications) {
      if (n["isRead"] == false) {
        await ApiClient.post('/api/Notification/mark-as-read/${n["id"]}');
      }
    }
    await _load();
  }

  int get _unreadCount =>
      _notifications.where((n) => n["isRead"] == false).length;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.roseDark),
        centerTitle: true,
        title: Text(
          'Notifikacije',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        actions: [
          if (_unreadCount > 0)
            Padding(
              padding: const EdgeInsets.only(right: AppSpacing.md),
              child: GestureDetector(
                onTap: _markAllAsRead,
                child: Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: 14,
                    vertical: 8,
                  ),
                  decoration: BoxDecoration(
                    color: AppColors.babyBlue.withOpacity(.2),
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                  child: const Text(
                    'Sve pročitano',
                    style: TextStyle(
                      fontWeight: FontWeight.w700,
                      color: AppColors.roseDark,
                    ),
                  ),
                ),
              ),
            ),
        ],
      ),
      body: _loading
          ? const Center(
              child: CircularProgressIndicator(color: AppColors.roseDark),
            )
          : _notifications.isEmpty
          ? const _EmptyState()
          : RefreshIndicator(
              color: AppColors.roseDark,
              onRefresh: _load,
              child: ListView.builder(
                padding: const EdgeInsets.all(AppSpacing.lg),
                itemCount: _notifications.length,
                itemBuilder: (_, index) {
                  final n = _notifications[index];
                  final isRead = n["isRead"] == true;

                  return _NotificationCard(
                    title: n["title"] ?? "",
                    message: n["message"] ?? "",
                    isRead: isRead,
                    onTap: () {
                      if (!isRead) {
                        _markAsRead(n["id"]);
                      }
                    },
                  );
                },
              ),
            ),
    );
  }
}

class _NotificationCard extends StatelessWidget {
  const _NotificationCard({
    required this.title,
    required this.message,
    required this.isRead,
    required this.onTap,
  });

  final String title;
  final String message;
  final bool isRead;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 0,
      margin: const EdgeInsets.only(bottom: AppSpacing.md),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: InkWell(
        borderRadius: BorderRadius.circular(AppRadius.xl),
        onTap: onTap,
        child: Container(
          padding: const EdgeInsets.all(AppSpacing.lg),
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(AppRadius.xl),
            color: isRead
                ? AppColors.card
                : AppColors.babyBlue.withOpacity(.15),
          ),
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Container(
                width: 42,
                height: 42,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: isRead
                      ? AppColors.babyBlue.withOpacity(.25)
                      : AppColors.babyPink,
                ),
                child: Icon(
                  Icons.notifications_rounded,
                  color: isRead ? AppColors.roseDark : Colors.white,
                ),
              ),
              const SizedBox(width: AppSpacing.md),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: isRead ? FontWeight.w600 : FontWeight.w800,
                        color: AppColors.textPrimary,
                      ),
                    ),
                    const SizedBox(height: 6),
                    Text(
                      message,
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                        color: AppColors.textSecondary,
                      ),
                    ),
                  ],
                ),
              ),
              if (!isRead)
                Container(
                  width: 10,
                  height: 10,
                  margin: const EdgeInsets.only(top: 6),
                  decoration: const BoxDecoration(
                    shape: BoxShape.circle,
                    color: AppColors.roseDark,
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}

class _EmptyState extends StatelessWidget {
  const _EmptyState();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Card(
          elevation: 0,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppRadius.xl),
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: const [
                Icon(
                  Icons.notifications_none_rounded,
                  size: 60,
                  color: AppColors.babyBlue,
                ),
                SizedBox(height: 16),
                Text(
                  'Nemate notifikacija',
                  style: TextStyle(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
