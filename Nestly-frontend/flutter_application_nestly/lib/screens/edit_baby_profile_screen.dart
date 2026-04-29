import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class EditBabyProfileScreen extends StatefulWidget {
  final int babyId;

  const EditBabyProfileScreen({super.key, required this.babyId});

  @override
  State<EditBabyProfileScreen> createState() => _EditBabyProfileScreenState();
}

class _EditBabyProfileScreenState extends State<EditBabyProfileScreen> {
  final _formKey = GlobalKey<FormState>();

  final _nameCtrl = TextEditingController();
  String? _gender;
  DateTime? _birthDate;

  bool _loading = true;
  bool _saving = false;

  @override
  void initState() {
    super.initState();
    _loadBaby();
  }

  Future<void> _loadBaby() async {
    try {
      final res = await ApiClient.get('/api/BabyProfile/${widget.babyId}');
      final data = jsonDecode(res.body);

      _nameCtrl.text = data['babyName'] ?? '';
      final g = data['gender']?.toString().toLowerCase();

      if (g == 'male' || g == 'female') {
        _gender = g;
      } else {
        _gender = null;
      }
      _birthDate = DateTime.parse(data['birthDate']);
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju bebe');
    } finally {
      setState(() => _loading = false);
    }
  }

  Future<void> _pickDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _birthDate ?? DateTime.now(),
      firstDate: DateTime(2000),
      lastDate: DateTime.now(),
    );

    if (picked != null) {
      setState(() => _birthDate = picked);
    }
  }

  Future<void> _save() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _saving = true);

    try {
      final body = {
        if (_nameCtrl.text.isNotEmpty) "babyName": _nameCtrl.text.trim(),
        if (_gender != null) "gender": _gender,
        if (_birthDate != null)
          "birthDate": _birthDate!.toUtc().toIso8601String(),
      };

      final res = await ApiClient.patch(
        '/api/BabyProfile/${widget.babyId}',
        body: body,
      );

      if (res.statusCode == 200) {
        NestlyToast.success(context, 'Uspješno spremljeno');
        Navigator.pop(context, true);
      } else {
        NestlyToast.error(context, 'Greška pri spremanju');
      }
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju');
    } finally {
      setState(() => _saving = false);
    }
  }

  InputDecoration _dec(String label, IconData icon) {
    return InputDecoration(
      labelText: label,
      prefixIcon: Icon(icon, color: AppColors.roseDark),
      filled: true,
      fillColor: AppColors.babyPink.withOpacity(.15),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide.none,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (_loading) {
      return const Scaffold(body: Center(child: CircularProgressIndicator()));
    }

    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.roseDark),
        title: const Text(
          'Uredi bebu',
          style: TextStyle(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Card(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppRadius.xl),
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.lg),
            child: Form(
              key: _formKey,
              child: Column(
                children: [
                  TextFormField(
                    controller: _nameCtrl,
                    validator: (v) =>
                        v == null || v.isEmpty ? 'Obavezno polje' : null,
                    decoration: _dec('Ime bebe', Icons.child_care),
                  ),
                  const SizedBox(height: 12),

                  DropdownButtonFormField<String>(
                    value: _gender,
                    items: const [
                      DropdownMenuItem(value: 'male', child: Text('Muško')),
                      DropdownMenuItem(value: 'female', child: Text('Žensko')),
                    ],
                    onChanged: (val) => setState(() => _gender = val),
                    decoration: _dec('Spol', Icons.wc),
                  ),
                  const SizedBox(height: 12),

                  ListTile(
                    tileColor: AppColors.babyPink.withOpacity(.15),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                    ),
                    title: Text(
                      _birthDate == null
                          ? 'Odaberi datum rođenja'
                          : '${_birthDate!.day}.${_birthDate!.month}.${_birthDate!.year}',
                    ),
                    trailing: const Icon(
                      Icons.calendar_today,
                      color: AppColors.roseDark,
                    ),
                    onTap: _pickDate,
                  ),

                  const SizedBox(height: 20),

                  SizedBox(
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: _saving ? null : _save,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: AppColors.roseDark,
                      ),
                      child: _saving
                          ? const CircularProgressIndicator(color: Colors.white)
                          : const Text(
                              'Sačuvaj',
                              style: TextStyle(color: Colors.white),
                            ),
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
