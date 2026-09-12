import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/providers/api_response_helper.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/layouts/nestly_widgets.dart';

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
      createdAt: DateTime.tryParse(json['createdAt'] ?? '') ?? DateTime.now(),
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
      createdAt: DateTime.tryParse(json['createdAt'] ?? '') ?? DateTime.now(),
    );
  }
}

class ChatMessagePage {
  final List<ChatMessage> items;
  final bool hasMore;

  ChatMessagePage({required this.items, required this.hasMore});
}

class ChatApiService {
  /// Cursor-based history: without [beforeId] this returns the most recent
  /// page (oldest-first within the page); passing the id of the oldest
  /// message already loaded walks one page further back in history. The
  /// backend caps `take` and reports [ChatMessagePage.hasMore] so the UI
  /// knows whether to keep offering "load older messages".
  Future<ChatMessagePage> getMessages(
    int conversationId, {
    int take = 40,
    int? beforeId,
  }) async {
    final query = StringBuffer('take=$take');
    if (beforeId != null) query.write('&beforeId=$beforeId');

    final res = await ApiClient.get(
      '/api/chat/messages/$conversationId?$query',
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load messages');
    }

    final decoded = jsonDecode(res.body);
    final List data = ApiResponseHelper.extractList(res.body);
    final hasMore = decoded is Map<String, dynamic>
        ? (decoded['hasMore'] as bool? ?? false)
        : false;

    return ChatMessagePage(
      items: data.map((e) => ChatMessage.fromJson(e)).toList(),
      hasMore: hasMore,
    );
  }

  /// Sends a message and returns the conversation id the backend
  /// created/used for it, so a brand-new conversation (conversationId
  /// starting at 0) can start being filtered correctly right away.
  Future<int?> sendMessage({
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

    try {
      final decoded = jsonDecode(res.body);
      if (decoded is Map<String, dynamic> && decoded['conversationId'] != null) {
        return decoded['conversationId'] as int;
      }
    } catch (_) {
      // Older/empty response body - caller just keeps its current id.
    }

    return null;
  }
}

class ChatScreen extends StatefulWidget {
  final int currentUserId;
  final int otherUserId;
  final int conversationId;
  final String otherUserName;
  final String gender;

  const ChatScreen({
    super.key,
    required this.currentUserId,
    required this.otherUserId,
    required this.conversationId,
    required this.otherUserName,
    this.gender = 'female',
  });

  @override
  State<ChatScreen> createState() => _ChatScreenState();
}

class _ChatScreenState extends State<ChatScreen> {
  final _api = ChatApiService();

  bool get _isGirl {
    final g = widget.gender.toLowerCase();
    return g == 'female' || g == 'f';
  }

  Color get _accent => _isGirl ? AppColors.roseDark : AppColors.seed;

  final _messages = <ChatMessage>[];
  final _msgCtrl = TextEditingController();
  final _scrollCtrl = ScrollController();

  HubConnection? _hub;
  bool _loading = true;
  bool _loadingOlder = false;
  bool _hasMoreOlder = false;

  // Mirrors widget.conversationId but can be updated once a brand-new
  // conversation (id 0) gets its real id from the first sent/received
  // message, so subsequent real-time messages can be filtered by id too.
  late int _conversationId;

  @override
  void initState() {
    super.initState();
    _conversationId = widget.conversationId;
    _loadMessages();
    _connectRealtime();
    _scrollCtrl.addListener(_onScroll);
  }

  void _onScroll() {
    // Message list is not reversed, so "near the top" means close to
    // minScrollExtent - that's where older history should load.
    if (!_hasMoreOlder || _loadingOlder || _loading) return;

    if (_scrollCtrl.position.pixels <=
        _scrollCtrl.position.minScrollExtent + 80) {
      _loadOlderMessages();
    }
  }

  Future<void> _loadMessages() async {
    try {
      if (_conversationId != 0) {
        final page = await _api.getMessages(_conversationId);

        if (!mounted) return;

        setState(() {
          _messages
            ..clear()
            ..addAll(page.items);
          _hasMoreOlder = page.hasMore;
        });
      }
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju poruka');
    } finally {
      if (!mounted) return;

      setState(() => _loading = false);
      _scrollToBottom();
    }
  }

  Future<void> _loadOlderMessages() async {
    if (_messages.isEmpty) return;

    setState(() => _loadingOlder = true);

    try {
      final oldestId = _messages.first.id;
      final page = await _api.getMessages(
        _conversationId,
        beforeId: oldestId == 0 ? null : oldestId,
      );

      if (!mounted) return;

      // Preserve the user's scroll position relative to the content they
      // were already looking at, instead of jumping to the top after the
      // older page is prepended.
      final previousMaxExtent = _scrollCtrl.position.maxScrollExtent;

      setState(() {
        _messages.insertAll(0, page.items);
        _hasMoreOlder = page.hasMore;
        _loadingOlder = false;
      });

      WidgetsBinding.instance.addPostFrameCallback((_) {
        if (!_scrollCtrl.hasClients) return;
        final newMaxExtent = _scrollCtrl.position.maxScrollExtent;
        _scrollCtrl.jumpTo(
          _scrollCtrl.position.pixels + (newMaxExtent - previousMaxExtent),
        );
      });
    } catch (_) {
      if (!mounted) return;
      setState(() => _loadingOlder = false);
      NestlyToast.error(context, 'Greška pri učitavanju starijih poruka');
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

      final data = Map<String, dynamic>.from(raw);
      final msg = ChatRealtimeMessage.fromJson(data);

      // Until we know the real conversationId (brand-new conversation),
      // fall back to matching by sender: since a chat is only ever between
      // these two specific users, any message from the other participant
      // is guaranteed to belong to this conversation.
      final belongsToThisChat = _conversationId != 0
          ? msg.conversationId == _conversationId
          : msg.senderId == widget.otherUserId;

      if (!belongsToThisChat) {
        return;
      }

      if (_conversationId == 0 && msg.conversationId != 0) {
        _conversationId = msg.conversationId;
      }

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

    try {
      await _hub!.start();
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri povezivanju na chat');
    }
  }

  Future<void> _send() async {
    final text = _msgCtrl.text.trim();

    if (text.isEmpty) return;

    _msgCtrl.clear();

    _scrollToBottom();

    try {
      final conversationId = await _api.sendMessage(
        receiverUserId: widget.otherUserId,
        content: text,
      );

      if (conversationId != null) {
        _conversationId = conversationId;
      }
    } catch (_) {
      if (!mounted) return;

      // Restore the text so the user doesn't lose what they typed.
      _msgCtrl.text = text;
      _msgCtrl.selection = TextSelection.collapsed(offset: text.length);

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
    _scrollCtrl.removeListener(_onScroll);
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
        iconTheme: IconThemeData(color: _accent),
        titleSpacing: 0,
        title: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            NestlyAvatar(name: widget.otherUserName, radius: 18),
            const SizedBox(width: 10),
            Flexible(
              child: Text(
                widget.otherUserName,
                overflow: TextOverflow.ellipsis,
                style: Theme.of(context).textTheme.titleMedium?.copyWith(
                  fontWeight: FontWeight.w700,
                  color: _accent,
                ),
              ),
            ),
          ],
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
    final showLoader = _loadingOlder;

    return ListView.builder(
      controller: _scrollCtrl,
      padding: const EdgeInsets.all(AppSpacing.lg),
      itemCount: _messages.length + (showLoader ? 1 : 0),
      itemBuilder: (_, i) {
        if (showLoader && i == 0) {
          return const Padding(
            padding: EdgeInsets.symmetric(vertical: 12),
            child: Center(
              child: SizedBox(
                width: 20,
                height: 20,
                child: CircularProgressIndicator(strokeWidth: 2),
              ),
            ),
          );
        }

        final m = _messages[showLoader ? i - 1 : i];
        final mine = m.senderId == widget.currentUserId;

        final timeText =
            '${m.createdAt.hour.toString().padLeft(2, '0')}:${m.createdAt.minute.toString().padLeft(2, '0')}';

        return Align(
          alignment: mine ? Alignment.centerRight : Alignment.centerLeft,
          child: Container(
            margin: const EdgeInsets.only(bottom: 10),
            padding: const EdgeInsets.all(12),
            constraints: const BoxConstraints(maxWidth: 280),
            decoration: BoxDecoration(
              color: mine ? _accent : AppColors.seed.withOpacity(.12),
              borderRadius: BorderRadius.only(
                topLeft: const Radius.circular(AppRadius.lg),
                topRight: const Radius.circular(AppRadius.lg),
                bottomLeft: Radius.circular(mine ? AppRadius.lg : 4),
                bottomRight: Radius.circular(mine ? 4 : AppRadius.lg),
              ),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.end,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  m.content,
                  style: TextStyle(
                    color: mine ? Colors.white : AppColors.textPrimary,
                  ),
                ),
                const SizedBox(height: 4),
                Text(
                  timeText,
                  style: TextStyle(
                    fontSize: 10,
                    color: mine
                        ? Colors.white.withOpacity(.75)
                        : AppColors.textSecondary,
                  ),
                ),
              ],
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
                cursorColor: _accent,
                textInputAction: TextInputAction.send,
                onSubmitted: (_) => _send(),
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
                    borderSide: BorderSide(color: _accent, width: 2),
                  ),
                ),
              ),
            ),

            const SizedBox(width: 8),
            Material(
              color: _accent,
              shape: const CircleBorder(),
              child: InkWell(
                customBorder: const CircleBorder(),
                onTap: _send,
                child: const Padding(
                  padding: EdgeInsets.all(12),
                  child: Icon(
                    Icons.send_rounded,
                    color: Colors.white,
                    size: 20,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
