import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
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

    final List data = jsonDecode(res.body);
    return data.map((e) => ChatConversation.fromJson(e)).toList();
  }

  Future<List<AppUserFull>> getAllUsers() async {
    final res = await ApiClient.get('/AppUser?roleId=1');

    if (res.statusCode != 200) {
      throw Exception('Failed to load users');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => AppUserFull.fromJson(e)).toList();
  }
}

class ChatHomeScreen extends StatefulWidget {
  final int currentUserId;

  const ChatHomeScreen({super.key, required this.currentUserId});

  @override
  State<ChatHomeScreen> createState() => _ChatHomeScreenState();
}

class _ChatHomeScreenState extends State<ChatHomeScreen> {
  final _api = ChatHomeApiService();

  bool _loading = true;
  String _search = '';

  List<ChatConversation> _conversations = [];
  List<AppUserFull> _users = [];

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    try {
      _conversations = await _api.getMyConversations();
      _users = await _api.getAllUsers();
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju chata');
    }

    if (mounted) {
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
          icon: const Icon(Icons.arrow_back_rounded, color: AppColors.roseDark),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Chat',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
      ),

      body: _loading
          ? const Center(
              child: CircularProgressIndicator(color: AppColors.roseDark),
            )
          : ListView(
              padding: const EdgeInsets.all(AppSpacing.lg),
              children: [
                _sectionTitle('Moji razgovori'),
                ..._buildConversationList(),
                const SizedBox(height: 24),
                _sectionTitle('Pronađi roditelja'),
                _searchField(),
                const SizedBox(height: 12),
                ..._buildUserList(),
              ],
            ),
    );
  }

  Widget _sectionTitle(String text) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 8),
      child: Text(
        text,
        style: Theme.of(context).textTheme.titleMedium?.copyWith(
          fontWeight: FontWeight.w700,
          color: AppColors.roseDark,
        ),
      ),
    );
  }

  Widget _searchField() {
    return TextField(
      cursorColor: AppColors.roseDark,
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
          borderSide: const BorderSide(color: AppColors.roseDark, width: 2),
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
        onTap: () => _openChatSmart(c.otherUserId, c.firstName),
      );
    }).toList();
  }

  List<Widget> _buildUserList() {
    final filtered = _users.where((u) {
      final name = '${u.firstName} ${u.lastName}'.toLowerCase();
      return name.contains(_search) && u.id != widget.currentUserId;
    }).toList();

    if (filtered.isEmpty) {
      return [_emptyBox('Nema korisnika')];
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
    VoidCallback? onTap,
  }) {
    return Container(
      margin: const EdgeInsets.only(bottom: 10),
      decoration: BoxDecoration(
        color: AppColors.card,
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: ListTile(
        onTap: onTap,
        leading: CircleAvatar(
          backgroundColor: AppColors.babyPink,
          child: Text(name.characters.first),
        ),
        title: Text(name),
        subtitle: Text(subtitle),
        trailing: const Icon(Icons.chat_bubble_outline_rounded),
      ),
    );
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
          currentUserId: widget.currentUserId,
          otherUserId: otherUserId,
          conversationId: conversationId,
          otherUserName: name,
        ),
      ),
    );

    if (mounted) {
      setState(() => _loading = true);
      await _load();
    }
  }
}
