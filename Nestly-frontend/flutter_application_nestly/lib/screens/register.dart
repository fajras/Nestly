import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _formKey = GlobalKey<FormState>();

  final _emailCtrl = TextEditingController();
  final _firstNameCtrl = TextEditingController();
  final _lastNameCtrl = TextEditingController();
  final _phoneCtrl = TextEditingController();
  final _usernameCtrl = TextEditingController();
  final _passwordCtrl = TextEditingController();

  DateTime? _dob;
  DateTime? _dueDate;
  String? _gender;
  bool _isPregnant = false;

  bool _obscure = true;
  bool _loading = false;

  static const _baseUrl = 'http://10.0.2.2:5167';

  @override
  void dispose() {
    _emailCtrl.dispose();
    _firstNameCtrl.dispose();
    _lastNameCtrl.dispose();
    _phoneCtrl.dispose();
    _usernameCtrl.dispose();
    _passwordCtrl.dispose();
    super.dispose();
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

  Future<void> _pickDate({
    required DateTime initial,
    required void Function(DateTime) onPicked,
    required String title,
  }) async {
    final picked = await showDatePicker(
      context: context,
      initialDate: initial,
      firstDate: DateTime(1900),
      lastDate: DateTime(2100),
      helpText: title,
    );
    if (picked != null) onPicked(picked);
  }

  String? _required(String? v, String msg) =>
      v == null || v.trim().isEmpty ? msg : null;

  String? _validateEmail(String? v) {
    if (_required(v, '') != null) return 'Unesite email';
    final ok = RegExp(r'^[^@\n]+@[^@\n]+\.[^@\n]+$').hasMatch(v!.trim());
    return ok ? null : 'Email nije ispravan';
  }

  Future<void> _onSubmit() async {
    if (!_formKey.currentState!.validate()) return;

    if (_dob == null) {
      NestlyToast.info(context, 'Odaberite datum rođenja');
      return;
    }

    if (_gender == null) {
      NestlyToast.info(context, 'Odaberite spol');
      return;
    }

    if (_isPregnant && _dueDate == null) {
      NestlyToast.info(context, 'Odaberite termin poroda');
      return;
    }

    setState(() => _loading = true);

    try {
      final res = await http.post(
        Uri.parse('$_baseUrl/AppUser'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          "email": _emailCtrl.text.trim(),
          "firstName": _firstNameCtrl.text.trim(),
          "lastName": _lastNameCtrl.text.trim(),
          "phoneNumber": _phoneCtrl.text.trim(),
          "dateOfBirth": _dob!.toIso8601String(),
          "gender": _gender,
          "username": _usernameCtrl.text.trim(),
          "password": _passwordCtrl.text,
          "roleId": 1,
          "dueDate": _isPregnant ? _dueDate?.toIso8601String() : null,
        }),
      );

      if (res.statusCode == 201) {
        NestlyToast.success(context, 'Registracija uspješna 🎉');
        if (mounted) Navigator.pop(context);
      } else {
        NestlyToast.error(context, 'Greška pri registraciji');
      }
    } catch (_) {
      NestlyToast.error(context, 'Server nedostupan');
    } finally {
      if (mounted) setState(() => _loading = false);
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
          'Registracija',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
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
                    controller: _emailCtrl,
                    decoration: _decoration(
                      label: 'Email',
                      icon: Icons.email_rounded,
                    ),
                    validator: _validateEmail,
                  ),
                  const SizedBox(height: AppSpacing.md),

                  TextFormField(
                    controller: _firstNameCtrl,
                    decoration: _decoration(
                      label: 'Ime',
                      icon: Icons.person_rounded,
                    ),
                    validator: (v) => _required(v, 'Unesite ime'),
                  ),
                  const SizedBox(height: AppSpacing.md),

                  TextFormField(
                    controller: _lastNameCtrl,
                    decoration: _decoration(
                      label: 'Prezime',
                      icon: Icons.person_outline,
                    ),
                    validator: (v) => _required(v, 'Unesite prezime'),
                  ),
                  const SizedBox(height: AppSpacing.md),

                  TextFormField(
                    controller: _phoneCtrl,
                    decoration: _decoration(
                      label: 'Telefon',
                      icon: Icons.phone,
                    ),
                    keyboardType: TextInputType.phone,
                  ),
                  const SizedBox(height: AppSpacing.md),

                  TextFormField(
                    controller: _usernameCtrl,
                    decoration: _decoration(
                      label: 'Korisničko ime',
                      icon: Icons.account_circle_rounded,
                    ),
                  ),
                  const SizedBox(height: AppSpacing.md),

                  TextFormField(
                    controller: _passwordCtrl,
                    obscureText: _obscure,
                    decoration:
                        _decoration(
                          label: 'Lozinka',
                          icon: Icons.lock_rounded,
                        ).copyWith(
                          suffixIcon: IconButton(
                            icon: Icon(
                              _obscure
                                  ? Icons.visibility_off
                                  : Icons.visibility,
                            ),
                            onPressed: () =>
                                setState(() => _obscure = !_obscure),
                          ),
                        ),
                  ),
                  const SizedBox(height: AppSpacing.md),

                  DropdownButtonFormField<String>(
                    value: _gender,
                    decoration: _decoration(
                      label: 'Spol',
                      icon: Icons.wc_rounded,
                    ),
                    items: const [
                      DropdownMenuItem(value: 'female', child: Text('Ženski')),
                      DropdownMenuItem(value: 'male', child: Text('Muški')),
                    ],
                    onChanged: (v) => setState(() => _gender = v),
                  ),
                  const SizedBox(height: AppSpacing.md),

                  ListTile(
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                    ),
                    tileColor: AppColors.babyPink.withOpacity(.15),
                    title: Text(
                      _dob == null
                          ? 'Datum rođenja'
                          : '${_dob!.day}.${_dob!.month}.${_dob!.year}',
                    ),
                    trailing: const Icon(Icons.calendar_today),
                    onTap: () => _pickDate(
                      initial: DateTime.now(),
                      title: 'Datum rođenja',
                      onPicked: (d) => setState(() => _dob = d),
                    ),
                  ),

                  SwitchListTile(
                    title: const Text('Trenutno sam trudna'),
                    value: _isPregnant,
                    activeColor: AppColors.roseDark,
                    onChanged: (v) => setState(() => _isPregnant = v),
                  ),

                  if (_isPregnant)
                    ListTile(
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(AppRadius.lg),
                      ),
                      tileColor: AppColors.babyPink.withOpacity(.15),
                      title: Text(
                        _dueDate == null
                            ? 'Termin poroda'
                            : '${_dueDate!.day}.${_dueDate!.month}.${_dueDate!.year}',
                      ),
                      trailing: const Icon(Icons.calendar_today),
                      onTap: () => _pickDate(
                        initial: DateTime.now(),
                        title: 'Termin poroda',
                        onPicked: (d) => setState(() => _dueDate = d),
                      ),
                    ),

                  const SizedBox(height: AppSpacing.lg),

                  SizedBox(
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: _loading ? null : _onSubmit,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: AppColors.roseDark,
                        foregroundColor: Colors.white,
                        padding: const EdgeInsets.symmetric(vertical: 14),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                        ),
                      ),
                      child: _loading
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
                          : const Text(
                              'Kreiraj nalog',
                              style: TextStyle(
                                fontWeight: FontWeight.w700,
                                fontSize: 16,
                              ),
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
