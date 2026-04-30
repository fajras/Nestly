import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';

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

  Future<void> _load() async {
    try {
      final res = await ApiClient.get('/api/Notification');

      if (res.statusCode != 200) {
        final error = jsonDecode(res.body);
        throw Exception(
          error["message"] ?? "Greška pri učitavanju notifikacija",
        );
      }

      final all = jsonDecode(res.body) as List<dynamic>;
      final threeDaysAgo = DateTime.now().toUtc().subtract(
        const Duration(days: 3),
      );

      final filtered = all.where((n) {
        if (n["createdAt"] == null) return false;

        final raw = n["createdAt"].toString();
        final date = DateTime.tryParse(raw);
        if (date == null) return false;

        return date.isAfter(threeDaysAgo);
      }).toList();

      if (!mounted) return;

      setState(() {
        _notifications = filtered;
        _loading = false;
      });
    } catch (e) {
      NestlyToast.error(
        context,
        'Greška pri učitavanju notifikacija. Pokušajte ponovo.',
      );
    }
  }

  Future<void> _markAsRead(int id) async {
    try {
      final res = await ApiClient.post('/api/Notification/mark-as-read/$id');

      if (res.statusCode != 200 && res.statusCode != 204) {
        final error = jsonDecode(res.body);
        throw Exception(error["message"] ?? "Greška pri označavanju");
      }

      await _load();
    } catch (e) {
      final msg = e.toString();

      if (msg.contains("not found")) {
        NestlyToast.error(context, 'Notifikacija ne postoji');
      } else {
        NestlyToast.error(context, 'Greška pri označavanju notifikacije');
      }
    }
  }

  Future<void> _markAllAsRead() async {
    try {
      final res = await ApiClient.post('/api/Notification/mark-all-as-read');

      if (res.statusCode != 200 && res.statusCode != 204) {
        final error = jsonDecode(res.body);
        throw Exception(error["message"] ?? "Greška pri označavanju svih");
      }

      await _load();
    } catch (e) {
      NestlyToast.error(context, 'Greška pri označavanju svih notifikacija');
    }
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
                    createdAt: n["createdAt"],
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
    required this.createdAt,
  });

  final String title;
  final String message;
  final bool isRead;
  final VoidCallback onTap;
  final String? createdAt;

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
                    const SizedBox(height: 6),
                    Text(
                      _formatDate(createdAt ?? ""),
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.textSecondary,
                        fontSize: 11,
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

String _formatDate(String raw) {
  final date = DateTime.tryParse(raw);
  if (date == null) return '';

  return "${date.day}.${date.month}.${date.year} ${date.hour}:${date.minute.toString().padLeft(2, '0')}";
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
