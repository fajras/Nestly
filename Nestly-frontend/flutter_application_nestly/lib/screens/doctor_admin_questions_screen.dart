import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';

class QaQuestionRow {
  final int id;
  final String questionText;
  final DateTime createdAt;
  final bool isAnswered;

  QaQuestionRow({
    required this.id,
    required this.questionText,
    required this.createdAt,
    required this.isAnswered,
  });

  factory QaQuestionRow.fromJson(Map<String, dynamic> json) {
    return QaQuestionRow(
      id: json['id'],
      questionText: json['questionText'],
      createdAt: DateTime.parse(json['createdAt']),
      isAnswered: json['isAnswered'],
    );
  }
}

class QaAdminService {
  Future<List<QaQuestionRow>> getUnansweredQuestions() async {
    final res = await ApiClient.get('/api/qaquestion');

    if (res.statusCode != 200) {
      throw Exception('Failed to load questions');
    }

    final List data = jsonDecode(res.body);

    return data
        .map((e) => QaQuestionRow.fromJson(e))
        .where((q) => q.isAnswered == false)
        .toList();
  }

  Future<void> answerQuestion({
    required int questionId,
    required String answerText,
    required int answeredById,
  }) async {
    final res = await ApiClient.post(
      '/api/QaQuestion/$questionId/answers',
      body: {'answerText': answerText, 'answeredById': answeredById},
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to save answer');
    }
  }
}

class DoctorAdminQuestionsScreen extends StatefulWidget {
  const DoctorAdminQuestionsScreen({super.key});

  @override
  State<DoctorAdminQuestionsScreen> createState() =>
      _DoctorAdminQuestionsScreenState();
}

class _DoctorAdminQuestionsScreenState
    extends State<DoctorAdminQuestionsScreen> {
  final _service = QaAdminService();

  List<QaQuestionRow> _questions = [];
  List<QaQuestionRow> _filtered = [];
  bool _loading = true;
  final _searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);

    try {
      final data = await _service.getUnansweredQuestions();

      setState(() {
        _questions = data;
        _filtered = data;
        _searchController.clear();
      });
    } catch (e) {
      NestlyToast.error(context, 'Greška pri učitavanju pitanja');
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  void _onSearch(String value) {
    final q = value.toLowerCase();

    setState(() {
      _filtered = _questions.where((x) {
        return x.questionText.toLowerCase().contains(q);
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
          'Pitanja korisnica',
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
        ),

        const SizedBox(height: AppSpacing.xl),

        _StatCard(
          title: 'Neodgovorena pitanja',
          value: _questions.length.toString(),
          icon: Icons.question_answer_outlined,
        ),

        const SizedBox(height: AppSpacing.xl),
        TextField(
          controller: _searchController,
          onChanged: _onSearch,
          decoration: const InputDecoration(
            hintText: 'Pretraga po tekstu pitanja',
            prefixIcon: Icon(Icons.search),
          ),
        ),

        const SizedBox(height: AppSpacing.lg),
        Expanded(
          child: _filtered.isEmpty
              ? const Center(
                  child: Text(
                    'Nema neodgovorenih pitanja',
                    style: TextStyle(
                      color: AppColors.textSecondary,
                      fontSize: 16,
                    ),
                  ),
                )
              : ListView.separated(
                  itemCount: _filtered.length,
                  separatorBuilder: (_, __) => const SizedBox(height: 12),
                  itemBuilder: (context, i) {
                    return _QuestionCard(
                      question: _filtered[i],

                      onAnswered: _load,
                    );
                  },
                ),
        ),
      ],
    );
  }
}

class _QuestionCard extends StatefulWidget {
  final QaQuestionRow question;

  final VoidCallback onAnswered;

  const _QuestionCard({required this.question, required this.onAnswered});

  @override
  State<_QuestionCard> createState() => _QuestionCardState();
}

class _QuestionCardState extends State<_QuestionCard> {
  bool _answering = false;
  bool _saving = false;
  final _controller = TextEditingController();
  final _service = QaAdminService();

  Future<void> _save() async {
    final text = _controller.text.trim();

    if (text.isEmpty) {
      NestlyToast.error(context, 'Odgovor ne može biti prazan');
      return;
    }

    if (text.length < 5) {
      NestlyToast.error(context, 'Odgovor je prekratak');
      return;
    }

    setState(() => _saving = true);

    try {
      await _service.answerQuestion(
        questionId: widget.question.id,
        answerText: text,
        answeredById: 1,
      );

      NestlyToast.success(
        context,
        'Odgovor spremljen',
        accentColor: AppColors.seed,
      );

      _controller.clear();
      widget.onAnswered();
    } catch (e) {
      NestlyToast.error(context, 'Greška pri spremanju odgovora');
    } finally {
      if (mounted) setState(() => _saving = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              widget.question.questionText,
              style: const TextStyle(fontSize: 16, fontWeight: FontWeight.w700),
            ),

            const SizedBox(height: 12),

            if (_answering) ...[
              TextField(
                controller: _controller,
                maxLines: 3,
                decoration: const InputDecoration(hintText: 'Upišite odgovor'),
              ),
              const SizedBox(height: 12),
              Row(
                children: [
                  ElevatedButton(
                    onPressed: _saving ? null : _save,
                    child: _saving
                        ? const SizedBox(
                            width: 16,
                            height: 16,
                            child: CircularProgressIndicator(strokeWidth: 2),
                          )
                        : const Text('Spremi'),
                  ),
                  const SizedBox(width: 12),
                  TextButton(
                    onPressed: () => setState(() => _answering = false),
                    child: const Text('Otkaži'),
                  ),
                ],
              ),
            ] else
              Align(
                alignment: Alignment.centerRight,
                child: TextButton.icon(
                  onPressed: () => setState(() => _answering = true),
                  icon: const Icon(Icons.reply),
                  label: const Text('Odgovori'),
                ),
              ),
          ],
        ),
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
    return Card(
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
    );
  }
}
