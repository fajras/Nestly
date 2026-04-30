import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

enum QuestionStatus { pending, answered }

class Answer {
  final String text;
  final DateTime createdAt;
  final String? responderName;

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
    required this.status,
    this.answer,
  });
}

abstract class QaService {
  Future<List<Question>> fetchMyQuestions();
  Future<Question> createQuestion(String text);
  Future<Question> updateQuestion(int id, String text);
  Future<void> deleteQuestion(int id);
}

class ApiQaService implements QaService {
  final int parentProfileId;

  ApiQaService({required this.parentProfileId});
  @override
  Future<Question> updateQuestion(int id, String text) async {
    final res = await ApiClient.patch(
      '/api/QaQuestion/$id',
      body: {'questionText': text},
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to update the question. Please try again later.');
    }

    final jsonRes = jsonDecode(res.body);

    return Question(
      id: jsonRes['id'],
      text: jsonRes['questionText'],
      createdAt: DateTime.parse(jsonRes['createdAt']),
      status: QuestionStatus.pending,
    );
  }

  @override
  Future<void> deleteQuestion(int id) async {
    final res = await ApiClient.delete('/api/QaQuestion/$id');

    if (res.statusCode != 204) {
      throw Exception('Failed to delete the question. Please try again.');
    }
  }

  @override
  @override
  Future<List<Question>> fetchMyQuestions() async {
    int page = 1;
    const pageSize = 100;

    List<Question> result = [];

    while (true) {
      final res = await ApiClient.get(
        '/api/QaQuestion/my'
        '?AskedById=$parentProfileId'
        '&page=$page&pageSize=$pageSize',
      );

      if (res.statusCode != 200) {
        throw Exception('Failed to load your questions.');
      }

      final data = jsonDecode(res.body);
      final List items = data['items'];

      if (items.isEmpty) break;

      final parsed = items.map<Question>((q) {
        final answered = q['isAnswered'] == true;

        return Question(
          id: q['id'],
          text: q['questionText'],
          createdAt: DateTime.parse(q['createdAt']),
          status: answered ? QuestionStatus.answered : QuestionStatus.pending,
          answer: answered && q['latestAnswerText'] != null
              ? Answer(
                  text: q['latestAnswerText'],
                  createdAt: DateTime.parse(
                    q['latestAnswerCreatedAt'] ?? q['createdAt'],
                  ),
                  responderName: q['answeredByName'],
                )
              : null,
        );
      }).toList();

      result.addAll(parsed);

      if (items.length < pageSize) break;

      page++;
    }

    return result;
  }

  @override
  Future<Question> createQuestion(String text) async {
    final res = await ApiClient.post(
      '/api/QaQuestion',
      body: {'questionText': text, 'askedById': parentProfileId},
    );

    if (res.statusCode != 201 && res.statusCode != 200) {
      throw Exception('Failed to send the question. Please try again.');
    }

    final jsonRes = jsonDecode(res.body) as Map<String, dynamic>;

    return Question(
      id: jsonRes['id'],
      text: jsonRes['questionText'],
      createdAt: DateTime.parse(jsonRes['createdAt']),
      status: QuestionStatus.pending,
    );
  }
}

class InMemoryQaService implements QaService {
  final List<Question> _data = [];
  int _id = 0;

  @override
  Future<List<Question>> fetchMyQuestions() async {
    await Future.delayed(const Duration(milliseconds: 200));
    return _data.toList()..sort((a, b) => b.createdAt.compareTo(a.createdAt));
  }

  @override
  Future<Question> updateQuestion(int id, String text) async {
    await Future.delayed(const Duration(milliseconds: 150));

    final index = _data.indexWhere((q) => q.id == id);
    if (index == -1) {
      throw Exception('Question not found.');
    }

    final updated = Question(
      id: _data[index].id,
      text: text,
      createdAt: _data[index].createdAt,
      status: QuestionStatus.pending,
    );

    _data[index] = updated;
    return updated;
  }

  @override
  Future<void> deleteQuestion(int id) async {
    await Future.delayed(const Duration(milliseconds: 150));
    _data.removeWhere((q) => q.id == id);
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
    });

    try {
      final items = await _service.fetchMyQuestions();

      if (!mounted) return;

      setState(() {
        _items = items;
      });
    } catch (_) {
      if (!mounted) return;

      NestlyToast.error(
        context,
        'Trenutno nije moguće učitati pitanja. Pokušajte ponovo.',
      );
    } finally {
      if (mounted) {
        setState(() => _loading = false);
      }
    }
  }

  Future<void> _openAsk() async {
    final created = await Navigator.of(context).push<Question>(
      MaterialPageRoute(builder: (_) => AskQuestionScreen(service: _service)),
    );
    if (created != null && mounted) {
      NestlyToast.success(context, 'Pitanje je uspješno poslano.');
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
            fontWeight: FontWeight.w700,
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
                backgroundColor: AppColors.roseDark,
                foregroundColor: AppColors.card,
                elevation: 0,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                ),
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
      return const Center(
        child: CircularProgressIndicator(color: AppColors.roseDark),
      );
    }

    if (_items.isEmpty) {
      return const Center(child: Text('Još nemate postavljenih pitanja.'));
    }

    return ListView.builder(
      padding: const EdgeInsets.all(AppSpacing.xl),
      itemCount: _items.length,
      itemBuilder: (_, i) => _QuestionCard(
        question: _items[i],
        service: _service,
        onRefresh: _load,
      ),
    );
  }
}

class _QuestionCard extends StatelessWidget {
  const _QuestionCard({
    required this.question,
    required this.service,
    required this.onRefresh,
  });

  final Question question;
  final QaService service;
  final VoidCallback onRefresh;

  @override
  Widget build(BuildContext context) {
    final answered = question.status == QuestionStatus.answered;

    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      padding: const EdgeInsets.all(AppSpacing.lg),
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
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Expanded(
                child: Text(
                  question.text,
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
                  ),
                ),
              ),
              if (!answered)
                Row(
                  children: [
                    _ActionIconButton(
                      icon: Icons.edit_rounded,
                      color: AppColors.roseDark,
                      onTap: () => _showEditDialog(context),
                    ),
                    const SizedBox(width: 8),
                    _ActionIconButton(
                      icon: Icons.delete_outline_rounded,
                      color: Colors.red.shade400,
                      onTap: () => _confirmDelete(context),
                    ),
                  ],
                ),
            ],
          ),
          const SizedBox(height: 8),
          if (answered && question.answer != null)
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  question.answer!.text,
                  style: const TextStyle(fontWeight: FontWeight.w600),
                ),
                const SizedBox(height: 4),
                Text(
                  'Odgovorio: ${question.answer!.responderName ?? ''}',
                  style: const TextStyle(fontSize: 12, color: Colors.grey),
                ),
              ],
            )
          else
            const Text('Čeka odgovor'),
        ],
      ),
    );
  }

  void _showEditDialog(BuildContext context) {
    final controller = TextEditingController(text: question.text);
    final formKey = GlobalKey<FormState>();

    showDialog(
      context: context,
      builder: (_) => Dialog(
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
        ),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Form(
            key: formKey,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  'Uredi pitanje',
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
                  ),
                ),
                const SizedBox(height: AppSpacing.lg),

                TextFormField(
                  controller: controller,
                  cursorColor: AppColors.roseDark,
                  maxLines: 4,
                  decoration: InputDecoration(
                    labelText: 'Vaše pitanje',
                    filled: true,
                    fillColor: AppColors.babyPink.withOpacity(.15),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                      borderSide: BorderSide.none,
                    ),
                    focusedBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                      borderSide: const BorderSide(
                        color: AppColors.roseDark,
                        width: 1.6,
                      ),
                    ),
                    floatingLabelStyle: const TextStyle(
                      color: AppColors.roseDark,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  validator: (v) {
                    if (v == null || v.trim().isEmpty) {
                      return 'Unesite pitanje';
                    }
                    if (v.trim().length < 8) {
                      return 'Pitanje je prekratko';
                    }
                    return null;
                  },
                ),

                const SizedBox(height: AppSpacing.xl),

                Row(
                  children: [
                    Expanded(
                      child: OutlinedButton(
                        onPressed: () => Navigator.pop(context),
                        style: OutlinedButton.styleFrom(
                          foregroundColor: AppColors.roseDark,
                          side: const BorderSide(color: AppColors.roseDark),
                        ),
                        child: const Text('Odustani'),
                      ),
                    ),
                    const SizedBox(width: 12),
                    Expanded(
                      child: ElevatedButton(
                        onPressed: () async {
                          if (!formKey.currentState!.validate()) return;

                          try {
                            await service.updateQuestion(
                              question.id,
                              controller.text.trim(),
                            );

                            if (!context.mounted) return;
                            Navigator.pop(context);
                            onRefresh();
                            NestlyToast.success(
                              context,
                              'Pitanje je ažurirano.',
                            );
                          } catch (_) {
                            if (!context.mounted) return;
                            NestlyToast.error(
                              context,
                              'Greška pri uređivanju pitanja.',
                            );
                          }
                        },
                        style: ElevatedButton.styleFrom(
                          backgroundColor: AppColors.roseDark,
                          foregroundColor: Colors.white,
                        ),
                        child: const Text('Sačuvaj'),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  void _confirmDelete(BuildContext context) {
    showDialog(
      context: context,
      builder: (_) => AlertDialog(
        title: const Text('Obriši pitanje?'),
        content: const Text('Ova radnja se ne može poništiti.'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Odustani'),
          ),
          TextButton(
            onPressed: () async {
              try {
                await service.deleteQuestion(question.id);

                if (!context.mounted) return;
                Navigator.pop(context);
                onRefresh();
                NestlyToast.success(context, 'Pitanje je obrisano.');
              } catch (_) {
                if (!context.mounted) return;
                NestlyToast.error(context, 'Greška pri brisanju pitanja.');
              }
            },
            child: const Text('Obriši', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );
  }
}

class _ActionIconButton extends StatelessWidget {
  const _ActionIconButton({
    required this.icon,
    required this.color,
    required this.onTap,
  });

  final IconData icon;
  final Color color;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return InkWell(
      borderRadius: BorderRadius.circular(10),
      onTap: onTap,
      child: Ink(
        padding: const EdgeInsets.all(8),
        decoration: BoxDecoration(
          color: color.withOpacity(.08),
          borderRadius: BorderRadius.circular(10),
        ),
        child: Icon(icon, size: 18, color: color),
      ),
    );
  }
}

class AskQuestionScreen extends StatefulWidget {
  const AskQuestionScreen({super.key, required this.service});
  final QaService service;

  @override
  State<AskQuestionScreen> createState() => _AskQuestionScreenState();
}

class _AskQuestionScreenState extends State<AskQuestionScreen> {
  final _controller = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  bool _sending = false;
  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  Future<void> _submit() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _sending = true);
    try {
      final q = await widget.service.createQuestion(_controller.text.trim());
      if (!mounted) return;
      Navigator.pop(context, q);
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri slanju pitanja');
    } finally {
      if (mounted) setState(() => _sending = false);
    }
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
          'Postavi pitanje',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                controller: _controller,
                cursorColor: AppColors.roseDark,
                maxLines: 5,
                decoration: _decoration(
                  label: 'Vaše pitanje',
                  icon: Icons.help_outline_rounded,
                ),
                validator: (v) {
                  if (v == null || v.trim().isEmpty) {
                    return 'Unesite pitanje';
                  }
                  if (v.trim().length < 8) {
                    return 'Pitanje je prekratko';
                  }
                  return null;
                },
              ),

              const SizedBox(height: AppSpacing.xl),

              SizedBox(
                height: 52,
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: _sending ? null : _submit,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: AppColors.roseDark,
                    foregroundColor: Colors.white,
                    elevation: 0,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                    ),
                    textStyle: const TextStyle(
                      fontWeight: FontWeight.w700,
                      fontSize: 16,
                    ),
                  ),
                  child: _sending
                      ? const SizedBox(
                          width: 20,
                          height: 20,
                          child: CircularProgressIndicator(
                            strokeWidth: 2,
                            valueColor: AlwaysStoppedAnimation<Color>(
                              Colors.white,
                            ),
                          ),
                        )
                      : const Text('Pošalji'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

InputDecoration _decoration({required String label, required IconData icon}) {
  return InputDecoration(
    labelText: label,
    prefixIcon: Icon(icon),
    filled: true,
    fillColor: AppColors.babyPink.withOpacity(.15),
    border: OutlineInputBorder(
      borderRadius: BorderRadius.circular(AppRadius.lg),
      borderSide: BorderSide.none,
    ),
    focusedBorder: OutlineInputBorder(
      borderRadius: BorderRadius.circular(AppRadius.lg),
      borderSide: const BorderSide(color: AppColors.roseDark, width: 1.6),
    ),
    floatingLabelStyle: const TextStyle(
      color: AppColors.roseDark,
      fontWeight: FontWeight.w600,
    ),
    prefixIconColor: AppColors.roseDark,
  );
}
