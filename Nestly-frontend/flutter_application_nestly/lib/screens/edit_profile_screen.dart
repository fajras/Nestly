import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class EditProfileScreen extends StatefulWidget {
  final int userId;

  const EditProfileScreen({super.key, required this.userId});

  @override
  State<EditProfileScreen> createState() => _EditProfileScreenState();
}

class _EditProfileScreenState extends State<EditProfileScreen> {
  final _formKey = GlobalKey<FormState>();

  final _firstNameCtrl = TextEditingController();
  final _lastNameCtrl = TextEditingController();
  final _phoneCtrl = TextEditingController();
  final _cycleCtrl = TextEditingController();
  final _oldPasswordCtrl = TextEditingController();
  final _newPasswordCtrl = TextEditingController();
  final _confirmPasswordCtrl = TextEditingController();

  bool _changePassword = false;

  DateTime? _dob;
  DateTime? _lmpDate;
  DateTime? _dueDate;

  String? _gender;

  int? _pregnancyId;
  int? _parentProfileId;

  bool _loading = true;
  bool _saving = false;
  bool _obscure = true;
  bool _dateError = false;

  // Greške za pojedina polja
  String? _oldPasswordError;
  String? _firstNameError;
  String? _lastNameError;
  String? _phoneError;
  String? _cycleError;

  @override
  void initState() {
    super.initState();
    _loadAll();
  }

  Future<void> _loadAll() async {
    try {
      final res = await ApiClient.get('/AppUser/${widget.userId}');
      final data = jsonDecode(res.body);

      _firstNameCtrl.text = data['firstName'] ?? '';
      _lastNameCtrl.text = data['lastName'] ?? '';
      _phoneCtrl.text = data['phoneNumber'] ?? '';
      _gender = data['gender']?.toString().toLowerCase().trim();
      _parentProfileId = data['parentProfileId'];

      if (data['dateOfBirth'] != null) {
        _dob = DateTime.parse(data['dateOfBirth']);
      }

      if (_parentProfileId != null) {
        final pregRes = await ApiClient.get(
          '/api/Pregnancy/by-parent/$_parentProfileId',
        );

        if (pregRes.statusCode == 200 && pregRes.body.isNotEmpty) {
          final p = jsonDecode(pregRes.body);

          setState(() {
            _pregnancyId = p['id'];

            _cycleCtrl.text = p['cycleLengthDays']?.toString() ?? '';

            _lmpDate = p['lmpDate'] != null
                ? DateTime.parse(p['lmpDate'])
                : null;

            _dueDate = p['dueDate'] != null
                ? DateTime.parse(p['dueDate'])
                : null;
          });
        }
      }
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
      prefixIconColor: AppColors.roseDark,
      filled: true,
      fillColor: AppColors.babyPink.withOpacity(.15),
      errorStyle: const TextStyle(color: Colors.redAccent),
      errorText: errorText,
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide.none,
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: const BorderSide(color: AppColors.roseDark, width: 1.6),
      ),
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: errorText != null
            ? const BorderSide(color: Colors.red, width: 1.5)
            : BorderSide.none,
      ),
    );
  }

  Future<void> _pickDate(DateTime? initial, Function(DateTime) setter) async {
    final picked = await showDatePicker(
      context: context,
      initialDate: initial ?? DateTime.now(),
      firstDate: DateTime(1900),
      lastDate: DateTime(2100),
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: const ColorScheme.light(
              primary: AppColors.roseDark,
              onPrimary: Colors.white,
              onSurface: AppColors.roseDark,
            ),
          ),
          child: child!,
        );
      },
    );

    if (picked != null) {
      setState(() => setter(picked));
    }
  }

  String? _validatePhone(String? value) {
    if (value == null || value.trim().isEmpty) {
      return 'Obavezno polje';
    }

    final phoneRegex = RegExp(r'^\+?[0-9]{8,15}$');

    if (!phoneRegex.hasMatch(value.trim())) {
      return 'Neispravan broj (npr. +38761123456)';
    }

    return null;
  }

  bool _hasChanges() {
    // Provjera ima li promjena u bilo kojem polju
    bool hasProfileChanges = false;
    bool hasPregnancyChanges = false;
    bool hasPasswordChanges = false;

    // Provjera promjena u profilu
    if (_firstNameCtrl.text.trim() !=
            (_firstNameCtrl.text.trim().isEmpty
                ? ''
                : _firstNameCtrl.text.trim()) ||
        _lastNameCtrl.text.trim() !=
            (_lastNameCtrl.text.trim().isEmpty
                ? ''
                : _lastNameCtrl.text.trim()) ||
        _phoneCtrl.text.trim() !=
            (_phoneCtrl.text.trim().isEmpty ? '' : _phoneCtrl.text.trim())) {
      hasProfileChanges = true;
    }

    // Provjera promjena u trudnoći
    if (_lmpDate != null ||
        _dueDate != null ||
        _cycleCtrl.text.trim().isNotEmpty) {
      hasPregnancyChanges = true;
    }

    // Provjera promjena lozinke
    if (_changePassword &&
        _oldPasswordCtrl.text.isNotEmpty &&
        _newPasswordCtrl.text.isNotEmpty &&
        _confirmPasswordCtrl.text.isNotEmpty) {
      hasPasswordChanges = true;
    }

    return hasProfileChanges || hasPregnancyChanges || hasPasswordChanges;
  }

  Future<void> _save() async {
    if (!_formKey.currentState!.validate()) return;

    // Očisti sve greške prije početka
    setState(() {
      _oldPasswordError = null;
      _firstNameError = null;
      _lastNameError = null;
      _phoneError = null;
      _cycleError = null;
      _dateError = false;
      _saving = true;
    });

    try {
      bool allSuccess = true;

      // Provjera datuma trudnoće
      if (_lmpDate != null && _dueDate != null) {
        final diff = _dueDate!.difference(_lmpDate!).inDays;

        if (diff < 250 || diff > 320) {
          setState(() => _dateError = true);
          allSuccess = false;
        }
      }

      // Promjena lozinke - SAMO AKO JE OZNAČENA I AKO IMA PROMJENA
      if (_changePassword &&
          _oldPasswordCtrl.text.isNotEmpty &&
          _newPasswordCtrl.text.isNotEmpty &&
          _confirmPasswordCtrl.text.isNotEmpty) {
        try {
          final res = await ApiClient.post(
            '/api/Auth/change-password/${widget.userId}',
            body: {
              "oldPassword": _oldPasswordCtrl.text,
              "newPassword": _newPasswordCtrl.text,
              "confirmPassword": _confirmPasswordCtrl.text,
            },
          );

          if (res.statusCode != 200) {
            final errorMsg = jsonDecode(res.body);
            final errorText = errorMsg is List
                ? errorMsg.join(", ")
                : errorMsg.toString();
            setState(() => _oldPasswordError = errorText);
            allSuccess = false;
          }
        } catch (e) {
          setState(() => _oldPasswordError = 'Greška pri promjeni lozinke');
          allSuccess = false;
        }
      }

      // Ažuriranje profila - SAMO AKO IMA PROMJENA
      final userBody = {
        if (_firstNameCtrl.text.trim().isNotEmpty)
          "firstName": _firstNameCtrl.text.trim(),
        if (_lastNameCtrl.text.trim().isNotEmpty)
          "lastName": _lastNameCtrl.text.trim(),
        if (_phoneCtrl.text.trim().isNotEmpty)
          "phoneNumber": _phoneCtrl.text.trim(),
        if (_dob != null) "dateOfBirth": _dob!.toUtc().toIso8601String(),
        if (_gender != null) "gender": _gender,
      };

      if (userBody.isNotEmpty) {
        try {
          final res = await ApiClient.patch(
            '/AppUser/${widget.userId}',
            body: userBody,
          );

          if (res.statusCode != 200) {
            setState(() {
              _firstNameError = 'Greška pri spremanju profila';
              _lastNameError = 'Greška pri spremanju profila';
              _phoneError = 'Greška pri spremanju profila';
            });
            allSuccess = false;
          }
        } catch (e) {
          setState(() {
            _firstNameError = 'Greška pri spremanju profila';
            _lastNameError = 'Greška pri spremanju profila';
            _phoneError = 'Greška pri spremanju profila';
          });
          allSuccess = false;
        }
      }

      // Ažuriranje trudnoće - SAMO AKO IMA PROMJENA
      final pregnancyBody = {
        if (_lmpDate != null) "lmpDate": _lmpDate!.toUtc().toIso8601String(),
        if (_dueDate != null) "dueDate": _dueDate!.toUtc().toIso8601String(),
        if (_cycleCtrl.text.isNotEmpty && _lmpDate != null)
          "cycleLengthDays": int.tryParse(_cycleCtrl.text),
      };

      if (pregnancyBody.isNotEmpty) {
        try {
          final res = _pregnancyId != null
              ? await ApiClient.patch(
                  '/api/Pregnancy/$_pregnancyId',
                  body: pregnancyBody,
                )
              : await ApiClient.post(
                  '/api/Pregnancy',
                  body: {...pregnancyBody, "userId": _parentProfileId},
                );

          if (res.statusCode != 200 && res.statusCode != 201) {
            setState(() => _cycleError = 'Greška pri spremanju trudnoće');
            allSuccess = false;
          }
        } catch (e) {
          setState(() => _cycleError = 'Greška pri spremanju trudnoće');
          allSuccess = false;
        }
      }

      // AKO SVE PRODE USPJESNO
      if (allSuccess) {
        if (!mounted) return;
        NestlyToast.success(context, 'Uspješno spremljeno');
        if (!mounted) return;
        Navigator.pop(context);
      } else {
        // AKO NESTO PADNE - OSTANI NA SCREENU
        if (!mounted) return;
        NestlyToast.error(
          context,
          'Spremanje nije uspjesno. Provjerite greške ispod.',
        );
      }
    } catch (e) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri spremanju');
    } finally {
      if (mounted) {
        setState(() => _saving = false);
      }
    }
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
        title: Text(
          'Uredi profil',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
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
                    controller: _firstNameCtrl,
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    validator: (v) =>
                        v == null || v.trim().isEmpty ? 'Obavezno polje' : null,
                    decoration: _decoration(
                      label: 'Ime',
                      icon: Icons.person,
                      errorText: _firstNameError,
                    ),
                  ),
                  const SizedBox(height: 12),
                  TextFormField(
                    controller: _lastNameCtrl,
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    validator: (v) =>
                        v == null || v.trim().isEmpty ? 'Obavezno polje' : null,
                    decoration: _decoration(
                      label: 'Prezime',
                      icon: Icons.person_outline,
                      errorText: _lastNameError,
                    ),
                  ),
                  const SizedBox(height: 12),
                  TextFormField(
                    controller: _phoneCtrl,
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    validator: _validatePhone,
                    decoration: _decoration(
                      label: 'Telefon',
                      icon: Icons.phone,
                      errorText: _phoneError,
                    ),
                  ),
                  const SizedBox(height: 12),
                  Align(
                    alignment: Alignment.centerLeft,
                    child: Row(
                      children: [
                        Checkbox(
                          value: _changePassword,
                          activeColor: AppColors.roseDark,
                          onChanged: (val) {
                            setState(() {
                              _changePassword = val ?? false;

                              if (!_changePassword) {
                                _oldPasswordCtrl.clear();
                                _newPasswordCtrl.clear();
                                _confirmPasswordCtrl.clear();
                                _oldPasswordError = null;
                              }
                            });
                          },
                        ),
                        const Text(
                          "Izmijeni lozinku",
                          style: TextStyle(
                            color: AppColors.roseDark,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                      ],
                    ),
                  ),
                  if (_changePassword) ...[
                    TextFormField(
                      controller: _oldPasswordCtrl,
                      obscureText: _obscure,
                      validator: (v) =>
                          v == null || v.isEmpty ? 'Obavezno polje' : null,
                      decoration: _decoration(
                        label: 'Stara lozinka',
                        icon: Icons.lock,
                        errorText: _oldPasswordError,
                      ),
                    ),
                    const SizedBox(height: 12),
                    TextFormField(
                      controller: _newPasswordCtrl,
                      obscureText: _obscure,
                      validator: (v) {
                        if (v == null || v.isEmpty) return 'Obavezno polje';
                        if (v.length < 6) return 'Min 6 karaktera';
                        return null;
                      },
                      decoration: _decoration(
                        label: 'Nova lozinka',
                        icon: Icons.lock,
                      ),
                    ),
                    const SizedBox(height: 12),
                    TextFormField(
                      controller: _confirmPasswordCtrl,
                      obscureText: _obscure,
                      validator: (v) =>
                          v != _newPasswordCtrl.text ? 'Ne poklapa se' : null,
                      decoration: _decoration(
                        label: 'Potvrdi lozinku',
                        icon: Icons.lock,
                      ),
                    ),
                  ],
                  const Divider(height: 30),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Text(
                        'Datum zadnje menstruacije',
                        style: TextStyle(
                          fontSize: 12,
                          color: AppColors.roseDark,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      const SizedBox(height: 4),
                      ListTile(
                        tileColor: AppColors.babyPink.withOpacity(.15),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                          side: _dateError
                              ? const BorderSide(color: Colors.red, width: 1.5)
                              : BorderSide.none,
                        ),
                        title: Text(
                          _lmpDate == null
                              ? 'Odaberi datum'
                              : '${_lmpDate!.day}.${_lmpDate!.month}.${_lmpDate!.year}',
                        ),
                        trailing: const Icon(
                          Icons.calendar_today,
                          color: AppColors.roseDark,
                        ),
                        onTap: () => _pickDate(_lmpDate, (d) => _lmpDate = d),
                      ),
                    ],
                  ),
                  const SizedBox(height: 12),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Text(
                        'Termin poroda',
                        style: TextStyle(
                          fontSize: 12,
                          color: AppColors.roseDark,
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      const SizedBox(height: 4),
                      ListTile(
                        tileColor: AppColors.babyPink.withOpacity(.15),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                          side: _dateError
                              ? const BorderSide(color: Colors.red, width: 1.5)
                              : BorderSide.none,
                        ),
                        title: Text(
                          _dueDate == null
                              ? 'Odaberi datum'
                              : '${_dueDate!.day}.${_dueDate!.month}.${_dueDate!.year}',
                        ),
                        trailing: const Icon(
                          Icons.calendar_today,
                          color: AppColors.roseDark,
                        ),
                        onTap: () => _pickDate(_dueDate, (d) => _dueDate = d),
                      ),
                    ],
                  ),
                  if (_dateError)
                    const Padding(
                      padding: EdgeInsets.only(top: 6),
                      child: Align(
                        alignment: Alignment.centerLeft,
                        child: Text(
                          'Razlika između datuma mora biti približno 9 mjeseci',
                          style: TextStyle(color: Colors.red, fontSize: 12),
                        ),
                      ),
                    ),
                  const SizedBox(height: 12),
                  TextFormField(
                    controller: _cycleCtrl,
                    keyboardType: TextInputType.number,
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    validator: (v) {
                      if (v == null || v.isEmpty) return null;

                      final value = int.tryParse(v);
                      if (value == null) return 'Mora biti broj';
                      if (value < 20 || value > 40) {
                        return 'Dozvoljeno 20–40 dana';
                      }
                      return null;
                    },
                    decoration: _decoration(
                      label: 'Dužina ciklusa',
                      icon: Icons.repeat,
                      errorText: _cycleError,
                    ),
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

  @override
  void dispose() {
    _firstNameCtrl.dispose();
    _lastNameCtrl.dispose();
    _phoneCtrl.dispose();
    _cycleCtrl.dispose();
    _oldPasswordCtrl.dispose();
    _newPasswordCtrl.dispose();
    _confirmPasswordCtrl.dispose();
    super.dispose();
  }
}
