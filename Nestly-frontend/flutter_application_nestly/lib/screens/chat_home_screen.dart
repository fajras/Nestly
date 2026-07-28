import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/layouts/nestly_widgets.dart';
import 'package:flutter_application_nestly/providers/api_response_helper.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'chat_screen.dart';
import 'package:flutter_application_nestly/network/api_client.dart';

class ChatConversation {
  final int conversationId;
  final int otherUserId;
  final String firstName;
  final String lastName;
  final String parentStatus;
  final int? babyAgeMonths;
  final int? pregnancyTrimester;
  final String? lastMessage;
  final DateTime? lastMessageTime;

  ChatConversation({
    required this.conversationId,
    required this.otherUserId,
    required this.firstName,
    required this.lastName,
    required this.parentStatus,
    this.babyAgeMonths,
    this.pregnancyTrimester,
    this.lastMessage,
    this.lastMessageTime,
  });

  factory ChatConversation.fromJson(Map<String, dynamic> json) {
    return ChatConversation(
      conversationId: json['conversationId'],
      otherUserId: json['otherParentId'] ?? json['otherUserId'],
      firstName: json['firstName'],
      lastName: json['lastName'],
      parentStatus: json['parentStatus'],
      babyAgeMonths: json['babyAgeMonths'],
      pregnancyTrimester: json['pregnancyTrimester'],
      lastMessage: json['lastMessage'],
      lastMessageTime: json['lastMessageTime'] != null
          ? DateTime.parse(json['lastMessageTime'])
          : null,
    );
  }
}

class AppUserFull {
  final int id;
  final String firstName;
  final String lastName;
  final String parentStatus;
  final int? babyAgeMonths;
  final int? pregnancyTrimester;

  AppUserFull({
    required this.id,
    required this.firstName,
    required this.lastName,
    required this.parentStatus,
    this.babyAgeMonths,
    this.pregnancyTrimester,
  });

  factory AppUserFull.fromJson(Map<String, dynamic> json) {
    return AppUserFull(
      id: json['id'],
      firstName: json['firstName'],
      lastName: json['lastName'],
      parentStatus: json['parentStatus'],
      babyAgeMonths: json['babyAgeMonths'],
      pregnancyTrimester: json['pregnancyTrimester'],
    );
  }
}

class ChatHomeApiService {
  Future<List<ChatConversation>> getMyConversations() async {
    final res = await ApiClient.get('/api/chat/conversations');

    if (res.statusCode != 200) {
      throw Exception('Failed to load conversations');
    }

    final List data = ApiResponseHelper.extractList(res.body);
    return data.map((e) => ChatConversation.fromJson(e)).toList();
  }

  Future<List<AppUserFull>> getAllUsers() async {
    final res = await ApiClient.get('/api/chat/available-users');

    if (res.statusCode != 200) {
      throw Exception('Failed to load users');
    }

    final List data = ApiResponseHelper.extractList(res.body);

    return data.map((e) => AppUserFull.fromJson(e)).toList();
  }
}

class ChatHomeScreen extends StatefulWidget {
  final String gender;

  const ChatHomeScreen({super.key, this.gender = 'female'});
  @override
  State<ChatHomeScreen> createState() => _ChatHomeScreenState();
}

class _ChatHomeScreenState extends State<ChatHomeScreen> {
  final _api = ChatHomeApiService();

  bool get _isGirl {
    final g = widget.gender.toLowerCase();
    return g == 'female' || g == 'f';
  }

  Color get _accent => _isGirl ? AppColors.roseDark : AppColors.seed;
  int? _currentUserId;
  bool _loading = true;
  String _search = '';

  List<ChatConversation> _conversations = [];
  List<AppUserFull> _users = [];

  @override
  void initState() {
    super.initState();
    _loadCurrentUser();
    _load();
  }

  Future<void> _loadCurrentUser() async {
    final token = await AuthStorage.getToken();

    if (token == null) return;

    final claims = JwtDecoder.decode(token);

    setState(() {
      _currentUserId = int.tryParse(claims["userId"].toString())!;
    });
  }

  Future<void> _load() async {
    try {
      final conversations = await _api.getMyConversations();
      final users = await _api.getAllUsers();

      if (!mounted) return;

      setState(() {
        _conversations = conversations;
        _users = users;
      });
    } catch (_) {
      if (!mounted) return;

      NestlyToast.error(context, 'Greška pri učitavanju chata');
    } finally {
      if (!mounted) return;
      setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        centerTitle: true,
        leading: IconButton(
          icon: Icon(Icons.arrow_back_rounded, color: _accent),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Chat',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: _accent,
          ),
        ),
      ),

      body: _loading
          ? Center(child: CircularProgressIndicator(color: _accent))
          : ListView(
              padding: const EdgeInsets.all(AppSpacing.lg),
              children: [
                NestlySectionHeader(title: 'Moji razgovori', color: _accent),
                ..._buildConversationList(),
                const SizedBox(height: 24),
                const NestlySectionHeader(
                  title: 'Pronađi roditelja',
                  color: AppColors.seed,
                ),
                _searchField(),
                const SizedBox(height: 12),
                ..._buildUserList(),
              ],
            ),
    );
  }

  Widget _searchField() {
    return TextField(
      cursorColor: _accent,
      decoration: InputDecoration(
        hintText: 'Pretraži roditelje...',
        filled: true,
        fillColor: AppColors.card,
        prefixIcon: const Icon(Icons.search),
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
          borderSide: BorderSide.none,
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
          borderSide: BorderSide(color: _accent, width: 2),
        ),
      ),
      onChanged: (v) => setState(() => _search = v.toLowerCase()),
    );
  }

  List<Widget> _buildConversationList() {
    if (_conversations.isEmpty) {
      return [_emptyBox('Još nemate započetih razgovora')];
    }

    return _conversations.map((c) {
      return _tile(
        name: '${c.firstName} ${c.lastName}',
        subtitle: _statusText(
          c.parentStatus,
          c.babyAgeMonths,
          c.pregnancyTrimester,
        ),
        lastMessage: c.lastMessage,
        lastMessageTime: c.lastMessageTime,
        onTap: () => _openChatSmart(c.otherUserId, c.firstName),
      );
    }).toList();
  }

  List<Widget> _buildUserList() {
    final filtered = _users.where((u) {
      final name = '${u.firstName} ${u.lastName}'.toLowerCase();
      return name.contains(_search) && u.id != _currentUserId;
    }).toList();

    if (filtered.isEmpty) {
      return [_emptyBox('Nema rezultata pretrage.')];
    }

    return filtered.map((u) {
      return _tile(
        name: '${u.firstName} ${u.lastName}',
        subtitle: 'Roditelj',
        onTap: () => _openChatSmart(u.id, u.firstName),
      );
    }).toList();
  }

  Widget _tile({
    required String name,
    required String subtitle,
    String? lastMessage,
    DateTime? lastMessageTime,
    VoidCallback? onTap,
  }) {
    return Container(
      margin: const EdgeInsets.only(bottom: 10),
      decoration: BoxDecoration(
        color: AppColors.card,
        borderRadius: BorderRadius.circular(AppRadius.lg),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(.03),
            blurRadius: 8,
            offset: const Offset(0, 3),
          ),
        ],
      ),
      child: ListTile(
        contentPadding: const EdgeInsets.symmetric(
          horizontal: AppSpacing.md,
          vertical: 6,
        ),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
        ),
        onTap: onTap,
        leading: NestlyAvatar(name: name),
        title: Text(
          name,
          style: const TextStyle(fontWeight: FontWeight.w700),
        ),
        subtitle: Text(
          lastMessage != null && lastMessage.isNotEmpty
              ? lastMessage
              : subtitle,
          maxLines: 1,
          overflow: TextOverflow.ellipsis,
          style: const TextStyle(color: AppColors.textSecondary),
        ),
        trailing: lastMessageTime != null
            ? Text(
                _formatTime(lastMessageTime),
                style: const TextStyle(
                  fontSize: 11,
                  color: AppColors.textSecondary,
                  fontWeight: FontWeight.w600,
                ),
              )
            : const Icon(
                Icons.chevron_right_rounded,
                color: AppColors.textSecondary,
              ),
      ),
    );
  }

  String _formatTime(DateTime dt) {
    final local = dt.toLocal();
    return '${local.hour.toString().padLeft(2, '0')}:${local.minute.toString().padLeft(2, '0')}';
  }

  Widget _emptyBox(String text) {
    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      padding: const EdgeInsets.all(AppSpacing.md),
      decoration: BoxDecoration(
        color: AppColors.babyBlue.withOpacity(.18),
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Text(
        text,
        textAlign: TextAlign.center,
        style: Theme.of(
          context,
        ).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
      ),
    );
  }

  String _statusText(String status, int? babyMonths, int? trimester) {
    if (status == 'PARENT' && babyMonths != null) {
      return 'Beba $babyMonths mj.';
    }
    if (status == 'PREGNANT' && trimester != null) {
      return '$trimester. trimestrar';
    }
    return 'Roditelj';
  }

  void _openChatSmart(int otherUserId, String name) async {
    final existing = _conversations
        .where((c) => c.otherUserId == otherUserId)
        .toList();

    final conversationId = existing.isNotEmpty
        ? existing.first.conversationId
        : 0;

    await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (_) => ChatScreen(
          currentUserId: _currentUserId ?? 0,
          otherUserId: otherUserId,
          conversationId: conversationId,
          otherUserName: name,
          gender: widget.gender,
        ),
      ),
    );

    if (mounted) {
      setState(() => _loading = true);
      await _load();
    }
  }
}
