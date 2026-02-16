import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:http/http.dart' as http;
import 'package:signalr_netcore/signalr_client.dart';

import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

/// =============================================================
/// MODELS
/// =============================================================

class ChatMessage {
  final int id;
  final int senderId;
  final String content;
  final DateTime createdAt;

  ChatMessage({
    required this.id,
    required this.senderId,
    required this.content,
    required this.createdAt,
  });

  factory ChatMessage.fromJson(Map<String, dynamic> json) {
    return ChatMessage(
      id: json['id'],
      senderId: json['senderId'],
      content: json['content'],
      createdAt: DateTime.parse(json['createdAt']),
    );
  }
}

class ChatRealtimeMessage {
  final int conversationId;
  final int senderId;
  final String content;
  final DateTime createdAt;

  ChatRealtimeMessage({
    required this.conversationId,
    required this.senderId,
    required this.content,
    required this.createdAt,
  });

  factory ChatRealtimeMessage.fromJson(Map<String, dynamic> json) {
    return ChatRealtimeMessage(
      conversationId: json['conversationId'],
      senderId: json['senderId'],
      content: json['content'],
      createdAt: DateTime.parse(json['createdAt']),
    );
  }
}

/// =============================================================
/// API SERVICE
/// =============================================================
class ChatApiService {
  Future<List<ChatMessage>> getMessages(int conversationId) async {
    final res = await ApiClient.get('/api/chat/messages/$conversationId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load messages');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => ChatMessage.fromJson(e)).toList();
  }

  Future<void> sendMessage({
    required int receiverUserId,
    required String content,
  }) async {
    final res = await ApiClient.post(
      '/api/chat/send',
      body: {'receiverUserId': receiverUserId, 'content': content},
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to send message');
    }
  }
}

/// =============================================================
/// SCREEN
/// =============================================================

class ChatScreen extends StatefulWidget {
  final int currentUserId;
  final int otherUserId;
  final int conversationId;
  final String otherUserName;

  const ChatScreen({
    super.key,
    required this.currentUserId,
    required this.otherUserId,
    required this.conversationId,
    required this.otherUserName,
  });

  @override
  State<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends State<ChatScreen> {
  final _api = ChatApiService();

  final _messages = <ChatMessage>[];
  final _msgCtrl = TextEditingController();
  final _scrollCtrl = ScrollController();

  HubConnection? _hub;
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _loadMessages();
    _connectRealtime();
  }

  Future<void> _loadMessages() async {
    try {
      if (widget.conversationId != 0) {
        final list = await _api.getMessages(widget.conversationId);
        if (!mounted) return;
        _messages.addAll(list);
      }
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju poruka');
    }

    if (mounted) {
      setState(() => _loading = false);
      _scrollToBottom();
    }
  }

  Future<void> _connectRealtime() async {
    final token = await AuthStorage.getToken();
    if (token == null) return;

    final baseUrl = ApiClient.baseUrl;

    _hub = HubConnectionBuilder()
        .withUrl(
          '$baseUrl/hubs/chat',
          options: HttpConnectionOptions(accessTokenFactory: () async => token),
        )
        .withAutomaticReconnect()
        .build();

    _hub!.on('ReceiveMessage', (args) {
      if (args == null || args.isEmpty) return;

      final raw = args.first;
      if (raw is! Map) return;

      final data = Map<String, dynamic>.from(raw as Map);
      final msg = ChatRealtimeMessage.fromJson(data);

      if (msg.conversationId != widget.conversationId) return;
      if (!mounted) return;

      setState(() {
        _messages.add(
          ChatMessage(
            id: 0,
            senderId: msg.senderId,
            content: msg.content,
            createdAt: msg.createdAt,
          ),
        );
      });

      _scrollToBottom();
    });

    await _hub!.start();
  }

  Future<void> _send() async {
    final text = _msgCtrl.text.trim();
    if (text.isEmpty) return;

    _msgCtrl.clear();

    setState(() {
      _messages.add(
        ChatMessage(
          id: 0,
          senderId: widget.currentUserId,
          content: text,
          createdAt: DateTime.now(),
        ),
      );
    });

    _scrollToBottom();

    try {
      await _api.sendMessage(receiverUserId: widget.otherUserId, content: text);
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Poruka nije poslana');
    }
  }

  void _scrollToBottom() {
    Future.delayed(const Duration(milliseconds: 120), () {
      if (_scrollCtrl.hasClients) {
        _scrollCtrl.jumpTo(_scrollCtrl.position.maxScrollExtent);
      }
    });
  }

  @override
  void dispose() {
    _msgCtrl.dispose();
    _scrollCtrl.dispose();
    _hub?.stop();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.roseDark),
        title: Text(
          widget.otherUserName,
          style: Theme.of(context).textTheme.titleMedium?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: _loading
          ? const Center(child: CircularProgressIndicator())
          : Column(
              children: [
                Expanded(child: _messageList()),
                _inputBar(),
              ],
            ),
    );
  }

  Widget _messageList() {
    return ListView.builder(
      controller: _scrollCtrl,
      padding: const EdgeInsets.all(AppSpacing.lg),
      itemCount: _messages.length,
      itemBuilder: (_, i) {
        final m = _messages[i];
        final mine = m.senderId == widget.currentUserId;

        return Align(
          alignment: mine ? Alignment.centerRight : Alignment.centerLeft,
          child: Container(
            margin: const EdgeInsets.only(bottom: 10),
            padding: const EdgeInsets.all(12),
            constraints: const BoxConstraints(maxWidth: 280),
            decoration: BoxDecoration(
              color: mine
                  ? AppColors.roseDark
                  : AppColors.babyPink.withOpacity(.35),
              borderRadius: BorderRadius.circular(AppRadius.lg),
            ),
            child: Text(
              m.content,
              style: TextStyle(color: mine ? Colors.white : Colors.black87),
            ),
          ),
        );
      },
    );
  }

  Widget _inputBar() {
    return SafeArea(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.md),
        child: Row(
          children: [
            Expanded(
              child: TextField(
                controller: _msgCtrl,
                cursorColor: AppColors.roseDark,
                decoration: InputDecoration(
                  hintText: 'Napiši poruku...',
                  filled: true,
                  fillColor: AppColors.card,
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                    borderSide: BorderSide.none,
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                    borderSide: const BorderSide(
                      color: AppColors.roseDark,
                      width: 2,
                    ),
                  ),
                ),
              ),
            ),

            const SizedBox(width: 8),
            IconButton(
              icon: const Icon(Icons.send_rounded),
              color: AppColors.roseDark,
              onPressed: _send,
            ),
          ],
        ),
      ),
    );
  }
}
