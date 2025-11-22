import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';

/// ===============================
/// CONFIG
/// ===============================

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  if (Platform.isIOS || Platform.isMacOS) return 'http://localhost:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _jsonHeaders() => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
};

/// ===============================
/// MODELI (FE)
/// ===============================

enum QuestionStatus { pending, answered }

class Answer {
  final String text;
  final DateTime createdAt;
  final String? responderName; // ako backend kasnije pošalje ime

  Answer({required this.text, required this.createdAt, this.responderName});
}

class Question {
  final int id;
  final String text;
  final DateTime createdAt;
  final QuestionStatus status;
  final Answer? answer;

  Question({
    required this.id,
    required this.text,
    required this.createdAt,
    this.status = QuestionStatus.pending,
    this.answer,
  });
}

/// ===============================
/// QA SERVICE INTERFACE
/// ===============================

abstract class QaService {
  Future<List<Question>> fetchMyQuestions();
  Future<Question> createQuestion(String text);
}

/// ===============================
/// API IMPLEMENTACIJA
/// ===============================

class ApiQaService implements QaService {
  final int parentProfileId;
  final String baseUrl;

  ApiQaService({required this.parentProfileId, String? baseUrl})
    : baseUrl = baseUrl ?? _apiBase;

  String get _qBase => '$baseUrl/api/QaQuestion';

  @override
  Future<List<Question>> fetchMyQuestions() async {
    final uri = Uri.parse('$_qBase?AskedByUserId=$parentProfileId');

    final res = await http
        .get(uri, headers: _jsonHeaders())
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 200) {
      throw Exception('Greška pri učitavanju pitanja (${res.statusCode}).');
    }

    final list = jsonDecode(res.body) as List;
    final questions = <Question>[];

    for (final item in list) {
      final qJson = item as Map<String, dynamic>;

      final id = (qJson['id'] ?? qJson['Id']) as int;
      final text = (qJson['questionText'] ?? qJson['QuestionText'] ?? '')
          .toString();

      final createdRaw = (qJson['createdAt'] ?? qJson['CreatedAt'])?.toString();
      final createdAt = DateTime.tryParse(createdRaw ?? '') ?? DateTime.now();

      // --- povuci odgovore za to pitanje ---
      Answer? latestAnswer;
      try {
        final ansRes = await http
            .get(Uri.parse('$_qBase/$id/answers'), headers: _jsonHeaders())
            .timeout(const Duration(seconds: 10));

        if (ansRes.statusCode == 200) {
          final ansBody = jsonDecode(ansRes.body);
          if (ansBody is List && ansBody.isNotEmpty) {
            // uzmi zadnji po createdAt (ako postoji) ili prvi
            Map<String, dynamic> best = ansBody.first;
            DateTime? bestCreated = _parseDate(
              best['createdAt'] ?? best['CreatedAt'],
            );

            for (final raw in ansBody.skip(1)) {
              final a = raw as Map<String, dynamic>;
              final ca = _parseDate(a['createdAt'] ?? a['CreatedAt']);
              if (ca != null &&
                  (bestCreated == null || ca.isAfter(bestCreated))) {
                best = a;
                bestCreated = ca;
              }
            }

            final aText = (best['answerText'] ?? best['AnswerText'] ?? '')
                .toString();
            final aCreated = bestCreated ?? createdAt;

            // ako backend nekad doda ime, mapiraj ovdje:
            final responderName =
                (best['answeredByName'] ?? best['AnsweredByName'])?.toString();

            if (aText.isNotEmpty) {
              latestAnswer = Answer(
                text: aText,
                createdAt: aCreated,
                responderName: responderName,
              );
            }
          }
        }
      } catch (_) {
        // ako faila odgovori, samo prikaži pitanje bez odgovora
      }

      final status = latestAnswer == null
          ? QuestionStatus.pending
          : QuestionStatus.answered;

      questions.add(
        Question(
          id: id,
          text: text,
          createdAt: createdAt,
          status: status,
          answer: latestAnswer,
        ),
      );
    }

    // najnovije prvo
    questions.sort((a, b) => b.createdAt.compareTo(a.createdAt));
    return questions;
  }

  @override
  Future<Question> createQuestion(String text) async {
    final uri = Uri.parse(_qBase);

    final body = jsonEncode({
      'questionText': text,
      'askedById': parentProfileId,
    });

    final res = await http
        .post(uri, headers: _jsonHeaders(), body: body)
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 201 && res.statusCode != 200) {
      throw Exception('Greška pri slanju pitanja (${res.statusCode}).');
    }

    final jsonRes = jsonDecode(res.body) as Map<String, dynamic>;

    final id = (jsonRes['id'] ?? jsonRes['Id']) as int;
    final createdRaw = (jsonRes['createdAt'] ?? jsonRes['CreatedAt'])
        ?.toString();
    final createdAt = DateTime.tryParse(createdRaw ?? '') ?? DateTime.now();

    return Question(
      id: id,
      text: text,
      createdAt: createdAt,
      status: QuestionStatus.pending,
    );
  }

  DateTime? _parseDate(dynamic v) {
    if (v == null) return null;
    return DateTime.tryParse(v.toString());
  }
}

/// (Opcija za testiranje bez backend-a, možeš ostaviti ili obrisati)
class InMemoryQaService implements QaService {
  final List<Question> _data = [];
  int _id = 0;

  @override
  Future<List<Question>> fetchMyQuestions() async {
    await Future.delayed(const Duration(milliseconds: 200));
    return _data.toList()..sort((a, b) => b.createdAt.compareTo(a.createdAt));
  }

  @override
  Future<Question> createQuestion(String text) async {
    await Future.delayed(const Duration(milliseconds: 150));
    final q = Question(
      id: ++_id,
      text: text,
      createdAt: DateTime.now(),
      status: QuestionStatus.pending,
    );
    _data.add(q);
    return q;
  }
}

/// ===============================
/// EKRAN: MOJA PITANJA
/// ===============================

class MyQuestionsScreen extends StatefulWidget {
  const MyQuestionsScreen({
    super.key,
    this.service,
    required this.parentProfileId,
  });

  final QaService? service;
  final int parentProfileId;

  @override
  State<MyQuestionsScreen> createState() => _MyQuestionsScreenState();
}

class _MyQuestionsScreenState extends State<MyQuestionsScreen> {
  late final QaService _service;
  bool _loading = true;
  String? _error;
  List<Question> _items = [];

  @override
  void initState() {
    super.initState();
    _service =
        widget.service ?? ApiQaService(parentProfileId: widget.parentProfileId);
    _load();
  }

  Future<void> _load() async {
    setState(() {
      _loading = true;
      _error = null;
    });

    try {
      _items = await _service.fetchMyQuestions();
    } catch (e) {
      _error = e.toString();
    }

    if (mounted) {
      setState(() => _loading = false);
    }
  }

  Future<void> _openAsk() async {
    final created = await Navigator.of(context).push<Question>(
      MaterialPageRoute(builder: (_) => AskQuestionScreen(service: _service)),
    );
    if (created != null && mounted) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Pitanje poslano.')));
      _load();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back_ios_new_rounded,
            color: AppColors.roseDark,
          ),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Moja pitanja',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: RefreshIndicator(
        color: AppColors.roseDark,
        onRefresh: _load,
        child: _buildBody(),
      ),
      bottomNavigationBar: SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(
            AppSpacing.xl,
            0,
            AppSpacing.xl,
            AppSpacing.lg,
          ),
          child: SizedBox(
            height: 52,
            child: ElevatedButton(
              onPressed: _openAsk,
              style: ElevatedButton.styleFrom(
                foregroundColor: AppColors.card,
                backgroundColor: AppColors.roseDark,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                ),
                elevation: 0,
                textStyle: const TextStyle(fontWeight: FontWeight.w700),
              ),
              child: const Text('Postavi pitanje'),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildBody() {
    if (_loading) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
          Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 520),
              child: Column(
                children: List.generate(
                  3,
                  (_) => Container(
                    height: 110,
                    margin: const EdgeInsets.only(bottom: 12),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black.withOpacity(.04),
                          blurRadius: 10,
                          offset: const Offset(0, 4),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ),
        ],
      );
    }

    if (_error != null) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
          Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 520),
              child: Container(
                padding: const EdgeInsets.all(AppSpacing.lg),
                decoration: BoxDecoration(
                  color: Colors.red.withOpacity(.06),
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                  border: Border.all(color: Colors.red.withOpacity(.35)),
                ),
                child: Text(
                  'Greška: $_error',
                  style: const TextStyle(
                    color: Colors.red,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ),
          ),
        ],
      );
    }

    if (_items.isEmpty) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
          Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 520),
              child: Container(
                padding: const EdgeInsets.all(AppSpacing.lg),
                decoration: BoxDecoration(
                  color: AppColors.babyBlue.withOpacity(.18),
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                ),
                child: Text(
                  'Još nemate postavljenih pitanja.\nDodirnite "Postavi pitanje" da unesete prvo.',
                  textAlign: TextAlign.center,
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    color: AppColors.textSecondary,
                  ),
                ),
              ),
            ),
          ),
        ],
      );
    }

    return ListView.builder(
      physics: const AlwaysScrollableScrollPhysics(),
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.xl,
        vertical: AppSpacing.lg,
      ),
      itemCount: _items.length + 1,
      itemBuilder: (context, i) {
        if (i == _items.length) {
          return const SizedBox(height: 90);
        }
        final q = _items[i];
        return _QuestionCard(question: q);
      },
    );
  }
}

class _QuestionCard extends StatelessWidget {
  const _QuestionCard({required this.question});
  final Question question;

  @override
  Widget build(BuildContext context) {
    final answered = question.status == QuestionStatus.answered;

    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      decoration: BoxDecoration(
        color: AppColors.card,
        borderRadius: BorderRadius.circular(AppRadius.lg),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(.04),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              question.text,
              style: Theme.of(context).textTheme.titleMedium?.copyWith(
                color: AppColors.roseDark,
                fontWeight: FontWeight.w800,
              ),
            ),
            const SizedBox(height: 6),
            Text(
              _formatDate(question.createdAt),
              style: Theme.of(
                context,
              ).textTheme.labelSmall?.copyWith(color: AppColors.textSecondary),
            ),
            const SizedBox(height: AppSpacing.md),

            if (answered && question.answer != null) ...[
              Container(
                padding: const EdgeInsets.all(AppSpacing.md),
                decoration: BoxDecoration(
                  color: AppColors.babyBlue.withOpacity(.16),
                  borderRadius: BorderRadius.circular(AppRadius.md),
                ),
                child: Text(
                  question.answer!.text,
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    color: AppColors.textSecondary,
                    height: 1.4,
                  ),
                ),
              ),
              const SizedBox(height: 8),
              Row(
                children: [
                  const Icon(Icons.verified, size: 16, color: AppColors.seed),
                  const SizedBox(width: 6),
                  Text(
                    'Odgovorio: ${question.answer!.responderName ?? 'Tim'} • ${_formatDate(question.answer!.createdAt)}',
                    style: Theme.of(context).textTheme.labelSmall?.copyWith(
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
            ] else
              Container(
                padding: const EdgeInsets.symmetric(
                  horizontal: 15,
                  vertical: 6,
                ),
                decoration: BoxDecoration(
                  color: AppColors.seed.withOpacity(.10),
                  borderRadius: BorderRadius.circular(999),
                ),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Icon(
                      Icons.hourglass_bottom_rounded,
                      size: 16,
                      color: AppColors.seed,
                    ),
                    const SizedBox(width: 6),
                    Text(
                      'Čeka odgovor',
                      style: Theme.of(context).textTheme.labelMedium?.copyWith(
                        color: AppColors.textPrimary,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ],
                ),
              ),
          ],
        ),
      ),
    );
  }

  String _formatDate(DateTime dt) {
    String two(int x) => x.toString().padLeft(2, '0');
    return '${two(dt.day)}.${two(dt.month)}.${dt.year}. ${two(dt.hour)}:${two(dt.minute)}';
  }
}

/// ===============================
/// EKRAN: POSTAVI PITANJE
/// ===============================

class AskQuestionScreen extends StatefulWidget {
  const AskQuestionScreen({super.key, required this.service});
  final QaService service;

  @override
  State<AskQuestionScreen> createState() => _AskQuestionScreenState();
}

class _AskQuestionScreenState extends State<AskQuestionScreen> {
  late final QaService _service;
  final _controller = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  bool _sending = false;

  @override
  void initState() {
    super.initState();
    _service = widget.service;
  }

  Future<void> _submit() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _sending = true);
    try {
      final q = await _service.createQuestion(_controller.text.trim());
      if (!mounted) return;
      Navigator.of(context).pop(q);
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Greška pri slanju: $e')));
    } finally {
      if (mounted) {
        setState(() => _sending = false);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back_ios_new_rounded,
            color: AppColors.roseDark,
          ),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Postavi pitanje',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: ConstrainedBox(
          constraints: const BoxConstraints(maxWidth: 520),
          child: Card(
            elevation: 3,
            color: AppColors.card,
            shadowColor: AppColors.babyPink.withOpacity(0.35),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(AppRadius.xl),
            ),
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Container(
                        padding: const EdgeInsets.all(10),
                        decoration: BoxDecoration(
                          color: AppColors.babyPink.withOpacity(.2),
                          borderRadius: BorderRadius.circular(AppRadius.md),
                        ),
                        child: const Icon(
                          Icons.chat_bubble_rounded,
                          color: AppColors.roseDark,
                          size: 22,
                        ),
                      ),
                      const SizedBox(width: AppSpacing.md),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Imate pitanje?',
                              style: Theme.of(context).textTheme.titleMedium
                                  ?.copyWith(
                                    color: AppColors.roseDark,
                                    fontWeight: FontWeight.w800,
                                  ),
                            ),
                            const SizedBox(height: 4),
                            Text(
                              'Pošaljite poruku doktoru ili savjetniku. '
                              'Pokušajte ukratko opisati problem ili situaciju.',
                              style: Theme.of(context).textTheme.bodySmall
                                  ?.copyWith(
                                    color: AppColors.textSecondary,
                                    height: 1.4,
                                  ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),

                  const SizedBox(height: AppSpacing.xl),

                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text(
                        'Pitanje',
                        style: TextStyle(
                          color: AppColors.roseDark,
                          fontWeight: FontWeight.w700,
                        ),
                      ),
                      Text(
                        'min. 8 znakova',
                        style: Theme.of(context).textTheme.labelSmall?.copyWith(
                          color: AppColors.textSecondary.withOpacity(0.8),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: AppSpacing.sm),
                  Form(
                    key: _formKey,
                    child: TextFormField(
                      controller: _controller,
                      minLines: 2,
                      maxLines: 8,
                      decoration: InputDecoration(
                        hintText:
                            'Npr. Imam jake mučnine ujutro, šta mogu uraditi?',
                        hintStyle: Theme.of(context).textTheme.bodyMedium
                            ?.copyWith(
                              color: AppColors.textSecondary.withOpacity(0.7),
                            ),
                        filled: true,
                        fillColor: Colors.white,
                        prefixIcon: const Padding(
                          padding: EdgeInsets.only(
                            left: 12,
                            right: 4,
                            top: 12,
                            bottom: 12,
                          ),
                          child: Icon(
                            Icons.edit_note_rounded,
                            color: AppColors.roseDark,
                          ),
                        ),
                        prefixIconConstraints: const BoxConstraints(
                          minWidth: 40,
                        ),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                          borderSide: BorderSide.none,
                        ),
                        contentPadding: const EdgeInsets.all(AppSpacing.lg),
                      ),
                      validator: (v) {
                        if (v == null || v.trim().isEmpty) {
                          return 'Molimo unesite pitanje.';
                        }
                        if (v.trim().length < 8) {
                          return 'Pitanje je prekratko.';
                        }
                        return null;
                      },
                    ),
                  ),

                  const SizedBox(height: AppSpacing.lg),

                  // mali info tekst ispod
                  Row(
                    children: [
                      const Icon(
                        Icons.info_outline_rounded,
                        size: 16,
                        color: AppColors.textSecondary,
                      ),
                      const SizedBox(width: 6),
                      Expanded(
                        child: Text(
                          'Odgovor ćete dobiti u aplikaciji čim ga naš tim obradi.',
                          style: Theme.of(context).textTheme.labelSmall
                              ?.copyWith(color: AppColors.textSecondary),
                        ),
                      ),
                    ],
                  ),

                  const SizedBox(height: AppSpacing.xl),

                  SizedBox(
                    height: 52,
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: _sending ? null : _submit,
                      style: ElevatedButton.styleFrom(
                        foregroundColor: AppColors.card,
                        backgroundColor: AppColors.roseDark,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                        ),
                        elevation: 0,
                        textStyle: const TextStyle(fontWeight: FontWeight.w700),
                      ),
                      child: _sending
                          ? const SizedBox(
                              height: 22,
                              width: 22,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : const Text('Pošalji'),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
