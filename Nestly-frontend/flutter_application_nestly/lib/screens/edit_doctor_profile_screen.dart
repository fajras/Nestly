import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class EditDoctorProfileScreen extends StatefulWidget {
  final int userId;

  const EditDoctorProfileScreen({super.key, required this.userId});

  @override
  State<EditDoctorProfileScreen> createState() =>
      _EditDoctorProfileScreenState();
}

class _EditDoctorProfileScreenState extends State<EditDoctorProfileScreen> {
  final _formKey = GlobalKey<FormState>();

  final _firstNameCtrl = TextEditingController();
  final _lastNameCtrl = TextEditingController();
  final _phoneCtrl = TextEditingController();
  final _oldPasswordCtrl = TextEditingController();
  final _newPasswordCtrl = TextEditingController();
  final _confirmPasswordCtrl = TextEditingController();

  bool _changePassword = false;
  bool _loading = true;
  bool _saving = false;

  DateTime? _dob;
  String? _gender;

  String? _oldPasswordError;

  @override
  void initState() {
    super.initState();
    _loadUser();
  }

  Future<void> _loadUser() async {
    try {
      final res = await ApiClient.get('/AppUser/${widget.userId}');

      if (res.statusCode != 200) {
        throw Exception("API error");
      }

      final data = jsonDecode(res.body);

      setState(() {
        _firstNameCtrl.text = data['firstName'] ?? '';
        _lastNameCtrl.text = data['lastName'] ?? '';
        _phoneCtrl.text = data['phoneNumber'] ?? '';
        _gender = data['gender']?.toString().toLowerCase().trim();

        if (data['dateOfBirth'] != null) {
          _dob = DateTime.parse(data['dateOfBirth']);
        }
      });
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju');
    } finally {
      setState(() => _loading = false);
    }
  }

  InputDecoration _decoration({
    required String label,
    required IconData icon,
    String? errorText,
  }) {
    return InputDecoration(
      labelText: label,
      prefixIcon: Icon(icon),
      prefixIconColor: AppColors.seed,
      filled: true,
      fillColor: AppColors.seed.withOpacity(.08),
      errorText: errorText,
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide.none,
      ),
    );
  }

  String? _validatePhone(String? value) {
    if (value == null || value.trim().isEmpty) return 'Obavezno polje';

    final regex = RegExp(r'^\+?[0-9]{8,15}$');
    if (!regex.hasMatch(value)) return 'Neispravan broj';

    return null;
  }

  Future<void> _save() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _saving = true;
      _oldPasswordError = null;
    });

    try {
      bool success = true;
      print("=== CHANGE PASSWORD DEBUG ===");
      print("UserId koji šaljem: ${widget.userId}");
      print("OldPassword: ${_oldPasswordCtrl.text}");
      print("NewPassword: ${_newPasswordCtrl.text}");
      if (_changePassword &&
          _oldPasswordCtrl.text.isNotEmpty &&
          _newPasswordCtrl.text.isNotEmpty &&
          _confirmPasswordCtrl.text.isNotEmpty) {
        final res = await ApiClient.post(
          '/api/Auth/change-password/${widget.userId}',
          body: {
            "oldPassword": _oldPasswordCtrl.text,
            "newPassword": _newPasswordCtrl.text,
            "confirmPassword": _confirmPasswordCtrl.text,
          },
        );

        if (res.statusCode != 200) {
          _oldPasswordError = 'Greška lozinke';
          success = false;
        }
      }

      final body = {
        "firstName": _firstNameCtrl.text.trim(),
        "lastName": _lastNameCtrl.text.trim(),
        "phoneNumber": _phoneCtrl.text.trim(),
        if (_dob != null) "dateOfBirth": _dob!.toIso8601String(),
        if (_gender != null) "gender": _gender,
      };

      final res = await ApiClient.patch(
        '/AppUser/${widget.userId}',
        body: body,
      );

      if (res.statusCode != 200) success = false;

      if (success) {
        NestlyToast.success(
          context,
          'Uspješno spremljeno',
          accentColor: AppColors.seed,
        );
      } else {
        NestlyToast.error(context, 'Greška pri spremanju');
      }
    } catch (_) {
      NestlyToast.error(context, 'Greška');
    } finally {
      setState(() => _saving = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_loading) {
      return const Scaffold(body: Center(child: CircularProgressIndicator()));
    }

    return Container(
      color: AppColors.bg,
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Uredi profil doktora',
              style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
            ),
            const SizedBox(height: AppSpacing.xl),

            Expanded(
              child: Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(AppRadius.xl),
                ),
                child: Padding(
                  padding: const EdgeInsets.all(AppSpacing.lg),
                  child: Form(
                    key: _formKey,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Text(
                          'Osnovni podaci',
                          style: TextStyle(
                            fontSize: 18,
                            fontWeight: FontWeight.w700,
                          ),
                        ),
                        const SizedBox(height: AppSpacing.md),

                        TextFormField(
                          controller: _firstNameCtrl,
                          validator: (v) =>
                              v == null || v.isEmpty ? 'Obavezno' : null,
                          decoration: _decoration(
                            label: 'Ime',
                            icon: Icons.person,
                          ),
                        ),
                        const SizedBox(height: 12),

                        TextFormField(
                          controller: _lastNameCtrl,
                          validator: (v) =>
                              v == null || v.isEmpty ? 'Obavezno' : null,
                          decoration: _decoration(
                            label: 'Prezime',
                            icon: Icons.person_outline,
                          ),
                        ),
                        const SizedBox(height: 12),

                        TextFormField(
                          controller: _phoneCtrl,
                          validator: _validatePhone,
                          decoration: _decoration(
                            label: 'Telefon',
                            icon: Icons.phone,
                          ),
                        ),

                        const SizedBox(height: AppSpacing.xl),

                        Row(
                          children: [
                            Checkbox(
                              value: _changePassword,
                              onChanged: (v) =>
                                  setState(() => _changePassword = v!),
                            ),
                            const Text("Izmijeni lozinku"),
                          ],
                        ),

                        if (_changePassword) ...[
                          const SizedBox(height: 12),
                          TextFormField(
                            controller: _oldPasswordCtrl,
                            obscureText: true,
                            decoration: _decoration(
                              label: 'Stara lozinka',
                              icon: Icons.lock,
                              errorText: _oldPasswordError,
                            ),
                          ),
                          const SizedBox(height: 12),
                          TextFormField(
                            controller: _newPasswordCtrl,
                            obscureText: true,
                            decoration: _decoration(
                              label: 'Nova lozinka',
                              icon: Icons.lock,
                            ),
                          ),
                          const SizedBox(height: 12),
                          TextFormField(
                            controller: _confirmPasswordCtrl,
                            obscureText: true,
                            decoration: _decoration(
                              label: 'Potvrdi lozinku',
                              icon: Icons.lock,
                            ),
                          ),
                        ],

                        const Spacer(),

                        SizedBox(
                          width: double.infinity,
                          height: 50,
                          child: ElevatedButton(
                            style: ElevatedButton.styleFrom(
                              backgroundColor: AppColors.seed,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(
                                  AppRadius.lg,
                                ),
                              ),
                            ),
                            onPressed: _saving ? null : _save,
                            child: _saving
                                ? const CircularProgressIndicator(
                                    color: Colors.white,
                                  )
                                : const Text(
                                    'Sačuvaj promjene',
                                    style: TextStyle(
                                      fontWeight: FontWeight.w600,
                                      color: Colors.white,
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
          ],
        ),
      ),
    );
  }
}
