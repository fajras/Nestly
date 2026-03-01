import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';

class BabyProfileApiService {
  static const String _basePath = '/api/BabyProfile';

  Future<void> createBabyProfile({
    required int parentProfileId,
    required String babyName,
    required String gender,
    required DateTime birthDate,
    double? birthHeightCm,
    double? birthWeightKg,
    int? pregnancyId,
  }) async {
    final body = {
      'parentProfileId': parentProfileId,
      'babyName': babyName,
      'gender': gender,
      'birthDate': birthDate.toIso8601String(),
      'pregnancyId': pregnancyId,
      'birthHeightCm': birthHeightCm,
      'birthWeightKg': birthWeightKg,
    };

    final res = await ApiClient.post(
      _basePath,
      body: body,
    ).timeout(const Duration(seconds: 10));

    if (res.statusCode != 201 && res.statusCode != 200) {
      throw Exception('Failed to create baby profile (${res.statusCode})');
    }
  }
}

class BabyProfileCreateScreen extends StatefulWidget {
  const BabyProfileCreateScreen({
    super.key,
    required this.parentProfileId,
    this.pregnancyId,
  });

  final int parentProfileId;
  final int? pregnancyId;

  @override
  State<BabyProfileCreateScreen> createState() =>
      _BabyProfileCreateScreenState();
}

class _BabyProfileCreateScreenState extends State<BabyProfileCreateScreen> {
  final _formKey = GlobalKey<FormState>();

  final _nameCtrl = TextEditingController();
  final _heightCtrl = TextEditingController();
  final _weightCtrl = TextEditingController();

  final _service = BabyProfileApiService();

  String _gender = 'F';
  DateTime _birthDate = DateTime.now();
  bool _saving = false;

  @override
  void dispose() {
    _nameCtrl.dispose();
    _heightCtrl.dispose();
    _weightCtrl.dispose();
    super.dispose();
  }

  String _formatDate(DateTime d) =>
      '${d.day.toString().padLeft(2, '0')}.'
      '${d.month.toString().padLeft(2, '0')}.'
      '${d.year}.';

  double? _parseDouble(String value) {
    final v = value.trim().replaceAll(',', '.');
    return v.isEmpty ? null : double.tryParse(v);
  }

  Future<void> _pickBirthDate() async {
    final now = DateTime.now();
    final firstDate = now.subtract(const Duration(days: 365));

    final picked = await showDatePicker(
      context: context,
      initialDate: _birthDate,
      firstDate: firstDate,
      lastDate: now,
      helpText: 'Odaberite datum rođenja',
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: Theme.of(context).colorScheme.copyWith(
              primary: AppColors.roseDark,
              surface: Colors.white,
            ),
          ),
          child: child!,
        );
      },
    );

    if (picked != null && mounted) {
      setState(() => _birthDate = picked);
    }
  }

  Future<void> _save() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _saving = true);

    try {
      await _service.createBabyProfile(
        parentProfileId: widget.parentProfileId,
        babyName: _nameCtrl.text.trim(),
        gender: _gender == 'F' ? 'Female' : 'Male',
        birthDate: _birthDate,
        birthHeightCm: _parseDouble(_heightCtrl.text),
        birthWeightKg: _parseDouble(_weightCtrl.text),
        pregnancyId: widget.pregnancyId,
      );
      if (!mounted) return;

      NestlyToast.success(context, 'Profil bebe je uspješno spremljen.');
      Navigator.of(context).pop();
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(
        context,
        'Došlo je do greške pri spremanju profila bebe.',
      );
    } finally {
      if (mounted) setState(() => _saving = false);
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
          'Unesite podatke o bebi',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Center(
          child: ConstrainedBox(
            constraints: const BoxConstraints(maxWidth: 520),
            child: Card(
              color: AppColors.card,
              elevation: 3,
              shadowColor: AppColors.babyPink.withOpacity(0.4),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(AppRadius.xl),
              ),
              child: Padding(
                padding: const EdgeInsets.all(AppSpacing.xl),
                child: Form(
                  key: _formKey,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: [
                      TextFormField(
                        controller: _nameCtrl,
                        decoration: const InputDecoration(labelText: 'Ime'),
                        validator: (v) => v == null || v.trim().isEmpty
                            ? 'Unesite ime bebe'
                            : null,
                      ),
                      const SizedBox(height: AppSpacing.lg),

                      Text(
                        'Spol',
                        style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                          fontWeight: FontWeight.w600,
                          color: AppColors.textPrimary,
                        ),
                      ),
                      const SizedBox(height: AppSpacing.sm),

                      Row(
                        children: [
                          Expanded(
                            child: _GenderChip(
                              label: 'Djevojčica',
                              icon: Icons.face_3_rounded,
                              selected: _gender == 'F',
                              onTap: () => setState(() => _gender = 'F'),
                              selectedColor: AppColors.roseDark,
                              unselectedColor: AppColors.roseDark,
                              accentColor: AppColors.roseDark,
                            ),
                          ),
                          const SizedBox(width: 10),
                          Expanded(
                            child: _GenderChip(
                              label: 'Dječak',
                              icon: Icons.face_6_rounded,
                              selected: _gender == 'M',
                              onTap: () => setState(() => _gender = 'M'),
                              selectedColor: AppColors.seed,
                              unselectedColor: AppColors.seed,
                              accentColor: AppColors.seed,
                            ),
                          ),
                        ],
                      ),

                      const SizedBox(height: AppSpacing.lg),

                      Text(
                        'Datum rođenja',
                        style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                          fontWeight: FontWeight.w600,
                          color: AppColors.textPrimary,
                        ),
                      ),
                      const SizedBox(height: AppSpacing.sm),
                      OutlinedButton.icon(
                        onPressed: _pickBirthDate,
                        icon: const Icon(Icons.calendar_today_rounded),
                        label: Text(_formatDate(_birthDate)),
                      ),

                      const SizedBox(height: AppSpacing.lg),

                      TextFormField(
                        controller: _heightCtrl,
                        keyboardType: const TextInputType.numberWithOptions(
                          decimal: true,
                        ),
                        inputFormatters: [
                          FilteringTextInputFormatter.allow(
                            RegExp(r'^\d+\.?\d{0,2}'),
                          ),
                        ],
                        decoration: const InputDecoration(
                          labelText: 'Visina',
                          suffixText: 'cm',
                        ),
                        validator: (v) {
                          if (v == null || v.trim().isEmpty) return null;
                          final value = double.tryParse(v.replaceAll(',', '.'));
                          if (value == null || value <= 0) {
                            return 'Unesite pozitivnu vrijednost';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: AppSpacing.lg),

                      TextFormField(
                        controller: _weightCtrl,
                        keyboardType: const TextInputType.numberWithOptions(
                          decimal: true,
                        ),
                        inputFormatters: [
                          FilteringTextInputFormatter.allow(
                            RegExp(r'^\d+\.?\d{0,2}'),
                          ),
                        ],
                        decoration: const InputDecoration(
                          labelText: 'Težina',
                          suffixText: 'kg',
                        ),
                        validator: (v) {
                          if (v == null || v.trim().isEmpty) return null;
                          final value = double.tryParse(v.replaceAll(',', '.'));
                          if (value == null || value <= 0) {
                            return 'Unesite pozitivnu vrijednost';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: AppSpacing.xl),

                      SizedBox(
                        height: 52,
                        child: ElevatedButton(
                          onPressed: _saving ? null : _save,
                          style: ElevatedButton.styleFrom(
                            backgroundColor: AppColors.roseDark,
                            foregroundColor: Colors.white,
                            elevation: 0,
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(AppRadius.lg),
                            ),
                            textStyle: const TextStyle(
                              fontWeight: FontWeight.w700,
                            ),
                          ),
                          child: _saving
                              ? const SizedBox(
                                  width: 22,
                                  height: 22,
                                  child: CircularProgressIndicator(
                                    strokeWidth: 2.4,
                                    valueColor: AlwaysStoppedAnimation(
                                      Colors.white,
                                    ),
                                  ),
                                )
                              : const Text('Spremi'),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class _GenderChip extends StatelessWidget {
  const _GenderChip({
    required this.label,
    required this.icon,
    required this.selected,
    required this.onTap,
    required this.selectedColor,
    required this.unselectedColor,
    required this.accentColor,
  });

  final String label;
  final IconData icon;
  final bool selected;
  final VoidCallback onTap;

  final Color selectedColor;
  final Color unselectedColor;
  final Color accentColor;

  @override
  Widget build(BuildContext context) {
    return InkWell(
      borderRadius: BorderRadius.circular(AppRadius.lg),
      onTap: onTap,
      child: AnimatedContainer(
        duration: const Duration(milliseconds: 150),
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(AppRadius.lg),
          color: selected ? selectedColor : unselectedColor.withOpacity(.12),
          border: Border.all(
            color: selected
                ? selectedColor.withOpacity(.9)
                : Colors.black.withOpacity(.06),
            width: selected ? 1.4 : 1,
          ),
          boxShadow: selected
              ? [
                  BoxShadow(
                    color: selectedColor.withOpacity(.45),
                    blurRadius: 12,
                    offset: const Offset(0, 5),
                  ),
                ]
              : [],
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(icon, size: 20, color: selected ? Colors.white : accentColor),
            const SizedBox(width: 8),
            Text(
              label,
              style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                fontWeight: FontWeight.w700,
                color: selected ? Colors.white : accentColor,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
