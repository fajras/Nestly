import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'dart:convert';
import 'package:flutter_application_nestly/model/app_user_row.dart';
import 'package:flutter_application_nestly/providers/notification_signalr_service.dart';
import 'package:flutter_application_nestly/providers/notification_state.dart';
import 'package:flutter_application_nestly/screens/admin_blog_screen.dart';
import 'package:flutter_application_nestly/screens/doctor_admin_questions_screen.dart';
import 'package:flutter_application_nestly/screens/doctor_admin_weekly_advice.dart';
import 'package:flutter_application_nestly/screens/doctor_system_management_screen.dart';
import 'package:flutter_application_nestly/screens/notifications_screen.dart';
import 'package:flutter_application_nestly/screens/user_detail_screen.dart';
import 'package:flutter_application_nestly/network/api_client.dart';

class AdminDashboardService {
  Future<List<AppUserRow>> getUsers() async {
    final res = await ApiClient.get('/AppUser?RoleId=1');

    if (res.statusCode != 200) {
      throw Exception('Failed to load users');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => AppUserRow.fromJson(e)).toList();
  }

  Future<int> getQuestionCount() async {
    final res = await ApiClient.get('/api/qaquestion');

    if (res.statusCode != 200) {
      throw Exception('Failed to load questions');
    }

    final List data = jsonDecode(res.body);
    return data.length;
  }
}

class DoctorAdminDashboardScreen extends StatefulWidget {
  const DoctorAdminDashboardScreen({super.key});

  @override
  State<DoctorAdminDashboardScreen> createState() =>
      _DoctorAdminDashboardScreenState();
}

class _DoctorAdminDashboardScreenState
    extends State<DoctorAdminDashboardScreen> {
  int _selectedIndex = 0;
  final NotificationSignalRService _signalRService =
      NotificationSignalRService();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: AppColors.bg,
        iconTheme: const IconThemeData(color: AppColors.seed),
        actions: [
          IconButton(
            icon: const Icon(Icons.notifications_rounded),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (_) => const NotificationsScreen()),
              );
            },
          ),
        ],
      ),
      body: Row(
        children: [
          _Sidebar(
            selectedIndex: _selectedIndex,
            onSelect: (i) => setState(() => _selectedIndex = i),
            onLogout: _handleLogout,
          ),

          Expanded(
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: _buildContent(),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _handleLogout() async {
    await _signalRService.disconnect();
    notificationState.reset();
    await AuthStorage.clear();

    if (!mounted) return;

    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(builder: (_) => const LoginScreen()),
      (_) => false,
    );
  }

  Widget _buildContent() {
    switch (_selectedIndex) {
      case 0:
        return _DashboardOverview();
      case 1:
        return UserDetailsScreen();
      case 2:
        return DoctorAdminQuestionsScreen();
      case 3:
        return DoctorAdminWeeklyAdviceScreen();
      case 4:
        return DoctorAdminBlogScreen();
      case 5:
        return SystemManagementScreen();
      default:
        return const SizedBox();
    }
  }
}

class _Sidebar extends StatelessWidget {
  final int selectedIndex;
  final ValueChanged<int> onSelect;
  final VoidCallback onLogout;
  const _Sidebar({
    required this.selectedIndex,
    required this.onSelect,
    required this.onLogout,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 260,
      padding: const EdgeInsets.symmetric(
        vertical: AppSpacing.xl,
        horizontal: AppSpacing.md,
      ),
      decoration: BoxDecoration(
        color: Colors.white,
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.04),
            blurRadius: 20,
            offset: const Offset(4, 0),
          ),
        ],
      ),
      child: Column(
        children: [
          Center(
            child: ClipRRect(
              borderRadius: BorderRadius.circular(12),
              child: Image.asset(
                'assets/images/nestly_logo_seed.png',
                width: 150,
                height: 150,
                fit: BoxFit.contain,
              ),
            ),
          ),

          const SizedBox(height: AppSpacing.xl),

          _SidebarItem(
            icon: Icons.dashboard_rounded,
            label: 'Dashboard',
            index: 0,
            selectedIndex: selectedIndex,
            onTap: onSelect,
          ),
          _SidebarItem(
            icon: Icons.people_alt_rounded,
            label: 'Korisnice',
            index: 1,
            selectedIndex: selectedIndex,
            onTap: onSelect,
          ),
          _SidebarItem(
            icon: Icons.question_answer_rounded,
            label: 'Pitanja',
            index: 2,
            selectedIndex: selectedIndex,
            onTap: onSelect,
          ),
          _SidebarItem(
            icon: Icons.tips_and_updates_rounded,
            label: 'Savjeti',
            index: 3,
            selectedIndex: selectedIndex,
            onTap: onSelect,
          ),
          _SidebarItem(
            icon: Icons.article_rounded,
            label: 'Blog',
            index: 4,
            selectedIndex: selectedIndex,
            onTap: onSelect,
          ),
          _SidebarItem(
            icon: Icons.admin_panel_settings_rounded,
            label: 'Upravljanje sistemom',
            index: 5,
            selectedIndex: selectedIndex,
            onTap: onSelect,
          ),

          const Spacer(),

          TextButton.icon(
            onPressed: onLogout,
            icon: const Icon(Icons.logout_rounded),
            label: const Text('Odjava'),
          ),
        ],
      ),
    );
  }
}

class _SidebarItem extends StatelessWidget {
  final IconData icon;
  final String label;
  final int index;
  final int selectedIndex;
  final ValueChanged<int> onTap;

  const _SidebarItem({
    required this.icon,
    required this.label,
    required this.index,
    required this.selectedIndex,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    final bool selected = index == selectedIndex;

    return Padding(
      padding: const EdgeInsets.only(bottom: 6),
      child: InkWell(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        onTap: () => onTap(index),
        child: Container(
          padding: const EdgeInsets.symmetric(
            horizontal: AppSpacing.md,
            vertical: AppSpacing.md,
          ),
          decoration: BoxDecoration(
            color: selected
                ? AppColors.seed.withOpacity(.12)
                : Colors.transparent,
            borderRadius: BorderRadius.circular(AppRadius.lg),
          ),
          child: Row(
            children: [
              Icon(
                icon,
                color: selected ? AppColors.seed : AppColors.textSecondary,
              ),
              const SizedBox(width: 12),
              Text(
                label,
                style: TextStyle(
                  fontWeight: FontWeight.w600,
                  color: selected ? AppColors.seed : AppColors.textSecondary,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _DashboardOverview extends StatefulWidget {
  const _DashboardOverview();

  @override
  State<_DashboardOverview> createState() => _DashboardOverviewState();
}

class _DashboardOverviewState extends State<_DashboardOverview> {
  List<AppUserRow> _users = [];
  List<AppUserRow> _filtered = [];

  int _userCount = 0;
  int _questionCount = 0;

  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  final _service = AdminDashboardService();

  Future<void> _loadData() async {
    setState(() => _loading = true);

    try {
      final users = await _service.getUsers();
      final questions = await _service.getQuestionCount();

      if (!mounted) return;
      setState(() {
        _users = users;
        _filtered = users;
        _userCount = users.length;
        _questionCount = questions;
      });
    } catch (e) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju podataka');
    } finally {
      if (!mounted) return;
      setState(() => _loading = false);
    }
  }

  void _onSearch(String value) {
    final q = value.toLowerCase();
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

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Admin upravljačka ploča',
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
        ),

        const SizedBox(height: AppSpacing.xl),

        Row(
          children: [
            _StatCard(
              title: 'Ukupno korisnica',
              value: _userCount.toString(),
              icon: Icons.people_outline,
            ),
            const SizedBox(width: AppSpacing.lg),
            _StatCard(
              title: 'Ukupno pitanja',
              value: _questionCount.toString(),
              icon: Icons.question_answer_outlined,
            ),
          ],
        ),

        const SizedBox(height: AppSpacing.xl),

        Expanded(
          child: Card(
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Pregled korisnica',
                    style: TextStyle(fontSize: 18, fontWeight: FontWeight.w700),
                  ),

                  const SizedBox(height: AppSpacing.md),

                  TextField(
                    onChanged: _onSearch,
                    decoration: const InputDecoration(
                      hintText: 'Pretraga po imenu i prezimenu',
                      prefixIcon: Icon(Icons.search),
                    ),
                  ),

                  const SizedBox(height: AppSpacing.lg),

                  Expanded(child: _UsersTable(users: _filtered)),
                ],
              ),
            ),
          ),
        ),
      ],
    );
  }
}

class _UsersTable extends StatelessWidget {
  final List<AppUserRow> users;

  const _UsersTable({required this.users});

  @override
  Widget build(BuildContext context) {
    if (users.isEmpty) {
      return Center(
        child: Text(
          'Nema korisnica za prikaz',
          style: TextStyle(color: AppColors.textSecondary, fontSize: 16),
        ),
      );
    }

    return ListView.separated(
      itemCount: users.length,
      separatorBuilder: (_, __) => const SizedBox(height: 12),
      itemBuilder: (context, i) {
        final u = users[i];

        return InkWell(
          borderRadius: BorderRadius.circular(AppRadius.lg),
          child: Card(
            elevation: 0,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(AppRadius.lg),
              side: BorderSide(color: Colors.black.withOpacity(.05)),
            ),
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Row(
                children: [
                  CircleAvatar(
                    radius: 26,
                    backgroundColor: AppColors.seed.withOpacity(.15),
                    child: Text(
                      u.firstName.isNotEmpty
                          ? u.firstName[0].toUpperCase()
                          : '?',
                      style: const TextStyle(
                        fontWeight: FontWeight.w700,
                        color: AppColors.seed,
                      ),
                    ),
                  ),

                  const SizedBox(width: AppSpacing.lg),

                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          u.fullName,
                          style: const TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.w700,
                          ),
                        ),
                        const SizedBox(height: 2),
                        Text(
                          u.email,
                          style: const TextStyle(
                            fontSize: 13,
                            color: AppColors.textSecondary,
                          ),
                        ),
                        const SizedBox(height: 6),
                        Align(
                          alignment: Alignment.centerLeft,
                          child: ConstrainedBox(
                            constraints: const BoxConstraints(maxWidth: 100),
                            child: Wrap(
                              spacing: 6,
                              runSpacing: 6,
                              children: [
                                _InfoChip(
                                  icon: Icons.family_restroom,
                                  label: u.parentStatus == 'PREGNANT'
                                      ? 'Trudnica'
                                      : 'Roditelj',
                                  color: u.parentStatus == 'PREGNANT'
                                      ? AppColors.babyPink
                                      : AppColors.babyBlue,
                                ),
                              ],
                            ),
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
      },
    );
  }
}

class _InfoChip extends StatelessWidget {
  final IconData icon;
  final String label;
  final Color? color;

  const _InfoChip({required this.icon, required this.label, this.color});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: (color ?? AppColors.seed).withOpacity(.15),
        borderRadius: BorderRadius.circular(12),
      ),
      child: Row(
        children: [
          Icon(icon, size: 14, color: AppColors.seed),
          const SizedBox(width: 4),
          Text(
            label,
            style: const TextStyle(fontSize: 12, fontWeight: FontWeight.w600),
          ),
        ],
      ),
    );
  }
}

class _StatCard extends StatelessWidget {
  final String title;
  final String value;
  final IconData icon;

  const _StatCard({
    required this.title,
    required this.value,
    required this.icon,
  });

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              CircleAvatar(
                radius: 22,
                backgroundColor: AppColors.seed.withOpacity(.15),
                child: Icon(icon, color: AppColors.seed),
              ),
              const SizedBox(width: AppSpacing.md),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: const TextStyle(
                      color: AppColors.textSecondary,
                      fontWeight: FontWeight.w500,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    value,
                    style: const TextStyle(
                      fontSize: 22,
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
