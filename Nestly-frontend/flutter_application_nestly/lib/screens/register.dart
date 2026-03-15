import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
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
  DateTime? _lmpDate;
  final _cycleCtrl = TextEditingController();
  DateTime? _dob;
  DateTime? _dueDate;
  String? _gender;

  bool _obscure = true;
  bool _loading = false;

  @override
  void dispose() {
    _cycleCtrl.dispose();
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
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: const ColorScheme.light(
              primary: AppColors.roseDark,
              onPrimary: Colors.white,
              onSurface: AppColors.textPrimary,
            ),
            textButtonTheme: TextButtonThemeData(
              style: TextButton.styleFrom(foregroundColor: AppColors.roseDark),
            ),
          ),
          child: child!,
        );
      },
    );

    if (picked != null) {
      onPicked(picked);
    }
  }

  String _translateBackendError(String message) {
    if (message.contains("Email already exists")) {
      return "Email već postoji.";
    }

    if (message.contains("Role not found")) {
      return "Uloga ne postoji.";
    }

    if (message.contains("Email is required")) {
      return "Email je obavezan.";
    }

    if (message.contains("Username is required")) {
      return "Korisničko ime je obavezno.";
    }

    if (message.contains("Password is required")) {
      return "Lozinka je obavezna.";
    }

    if (message.contains("FirstName is required")) {
      return "Ime je obavezno.";
    }

    if (message.contains("LastName is required")) {
      return "Prezime je obavezno.";
    }

    if (message.contains("PhoneNumber is required")) {
      return "Telefon je obavezan.";
    }

    if (message.contains("Gender is required")) {
      return "Spol je obavezan.";
    }

    if (message.contains("Failed to create Identity user")) {
      return "Korisnik nije mogao biti kreiran. Provjerite lozinku.";
    }

    if (message.contains("Passwords must")) {
      return "Lozinka ne ispunjava sigurnosne uslove.";
    }

    if (message.contains("LMP")) {
      return "Datum posljednje menstruacije nije ispravan.";
    }

    if (message.contains("DueDate")) {
      return "Termin poroda nije ispravan.";
    }

    return message;
  }

  String? _required(String? v, String msg) =>
      v == null || v.trim().isEmpty ? msg : null;

  String? _validateEmail(String? v) {
    if (_required(v, '') != null) return 'Unesite email';
    final ok = RegExp(r'^[^@\n]+@[^@\n]+\.[^@\n]+$').hasMatch(v!.trim());
    return ok ? null : 'Email nije ispravan';
  }

  String? _validateUsername(String? v) {
    if (v == null || v.trim().isEmpty) return 'Unesite korisničko ime';
    if (v.trim().length < 4)
      return 'Korisničko ime mora imati najmanje 4 znaka';
    return null;
  }

  String? _validatePassword(String? v) {
    if (v == null || v.isEmpty) return 'Unesite lozinku';
    if (v.length < 8) return 'Lozinka mora imati najmanje 8 znakova';
    if (!RegExp(r'[A-Z]').hasMatch(v)) {
      return 'Lozinka mora sadržavati veliko slovo';
    }
    if (!RegExp(r'[0-9]').hasMatch(v)) {
      return 'Lozinka mora sadržavati broj';
    }
    return null;
  }

  String? _validatePhone(String? v) {
    if (v == null || v.trim().isEmpty) return 'Unesite broj telefona';
    if (!RegExp(r'^[0-9+ ]+$').hasMatch(v.trim())) {
      return 'Telefon nije ispravan';
    }
    return null;
  }

  Future<void> _onSubmit() async {
    if (!_formKey.currentState!.validate()) return;
    setState(() => _loading = true);
    if (_dob == null || _lmpDate == null || _dueDate == null) {
      setState(() => _loading = false);
      NestlyToast.error(context, 'Popunite sve datume');
      return;
    }
    try {
      final res = await ApiClient.post(
        '/AppUser',
        body: {
          "email": _emailCtrl.text.trim(),
          "firstName": _firstNameCtrl.text.trim(),
          "lastName": _lastNameCtrl.text.trim(),
          "phoneNumber": _phoneCtrl.text.trim(),
          "dateOfBirth": _dob!.toUtc().toIso8601String(),
          "gender": _gender,
          "username": _usernameCtrl.text.trim(),
          "password": _passwordCtrl.text,
          "roleId": 1,
          "lmpDate": _lmpDate!.toUtc().toIso8601String(),
          "dueDate": _dueDate!.toUtc().toIso8601String(),
          "cycleLengthDays": int.parse(_cycleCtrl.text),
        },
      );
      if (res.statusCode == 201) {
        NestlyToast.success(context, 'Registracija uspješna 🎉');
        if (mounted) Navigator.pop(context);
      } else {
        String message = 'Greška pri registraciji';

        try {
          final body = jsonDecode(res.body);

          if (body is Map) {
            if (body['message'] != null) {
              message = body['message'].toString();
            } else if (body['title'] != null) {
              message = body['title'].toString();
            } else if (body['detail'] != null) {
              message = body['detail'].toString();
            } else if (body['errors'] != null) {
              message = body['errors'].toString();
            } else {
              message = res.body.toString();
            }
          } else {
            message = res.body.toString();
          }
        } catch (_) {
          message = res.body.toString();
        }
        if (!mounted) return;
        message = _translateBackendError(message);
        if (message.trim().isEmpty) {
          message = "Greška pri registraciji";
        }
        NestlyToast.error(context, message);
      }
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Server nedostupan');
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: Theme.of(context).copyWith(
        textSelectionTheme: const TextSelectionThemeData(
          cursorColor: AppColors.roseDark,
          selectionColor: AppColors.roseDark,
          selectionHandleColor: AppColors.roseDark,
        ),
      ),
      child: Scaffold(
        backgroundColor: AppColors.bg,
        appBar: AppBar(
          backgroundColor: Colors.transparent,
          elevation: 0,
          iconTheme: const IconThemeData(color: AppColors.roseDark),
          title: Text(
            'Registracija',
            style: Theme.of(context).textTheme.titleLarge?.copyWith(
              fontWeight: FontWeight.w700,
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
                      validator: _validatePhone,
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
                      validator: _validateUsername,
                    ),
                    const SizedBox(height: AppSpacing.md),

                    TextFormField(
                      controller: _passwordCtrl,
                      obscureText: _obscure,
                      validator: _validatePassword,
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
                        DropdownMenuItem(
                          value: 'female',
                          child: Text('Ženski'),
                        ),
                        DropdownMenuItem(value: 'male', child: Text('Muški')),
                      ],
                      onChanged: (v) => setState(() => _gender = v),
                      validator: (value) {
                        if (value == null) {
                          return 'Odaberite spol';
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: AppSpacing.md),

                    FormField<DateTime>(
                      validator: (value) {
                        if (_dob == null) {
                          return 'Odaberite datum rođenja';
                        }
                        final age = DateTime.now().year - _dob!.year;
                        if (age < 13) {
                          return 'Morate imati najmanje 13 godina';
                        }
                        return null;
                      },
                      builder: (state) => Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
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
                              onPicked: (d) {
                                setState(() => _dob = d);
                                state.didChange(d);
                              },
                            ),
                          ),
                          if (state.hasError)
                            Padding(
                              padding: const EdgeInsets.only(left: 12, top: 6),
                              child: Text(
                                state.errorText!,
                                style: const TextStyle(
                                  color: Colors.red,
                                  fontSize: 12,
                                ),
                              ),
                            ),
                        ],
                      ),
                    ),

                    const SizedBox(height: AppSpacing.md),

                    FormField<DateTime>(
                      validator: (value) {
                        if (_dueDate == null) {
                          return 'Odaberite termin poroda';
                        }
                        if (_dueDate!.isBefore(
                          DateTime.now().subtract(const Duration(days: 7)),
                        )) {
                          return 'Termin poroda ne može biti u prošlosti';
                        }
                        return null;
                      },
                      builder: (state) => Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
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
                              onPicked: (d) {
                                setState(() => _dueDate = d);
                                state.didChange(d);
                              },
                            ),
                          ),
                          if (state.hasError)
                            Padding(
                              padding: const EdgeInsets.only(left: 12, top: 6),
                              child: Text(
                                state.errorText!,
                                style: const TextStyle(
                                  color: Colors.red,
                                  fontSize: 12,
                                ),
                              ),
                            ),
                        ],
                      ),
                    ),

                    const SizedBox(height: AppSpacing.md),

                    FormField<DateTime>(
                      validator: (value) {
                        if (_lmpDate == null) {
                          return 'Odaberite datum posljednje menstruacije';
                        }
                        if (_lmpDate!.isAfter(DateTime.now())) {
                          return 'Datum ne može biti u budućnosti';
                        }
                        if (_dueDate != null) {
                          final diff = _dueDate!.difference(_lmpDate!).inDays;
                          if (diff < 240 || diff > 300) {
                            return 'Razmak mora biti oko 9 mjeseci';
                          }
                        }
                        return null;
                      },
                      builder: (state) => Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          ListTile(
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(AppRadius.lg),
                            ),
                            tileColor: AppColors.babyPink.withOpacity(.15),
                            title: Text(
                              _lmpDate == null
                                  ? 'Datum posljednje menstruacije'
                                  : '${_lmpDate!.day}.${_lmpDate!.month}.${_lmpDate!.year}',
                            ),
                            trailing: const Icon(Icons.calendar_today),
                            onTap: () => _pickDate(
                              initial: DateTime.now(),
                              title: 'Datum posljednje menstruacije',
                              onPicked: (d) {
                                setState(() => _lmpDate = d);
                                state.didChange(d);
                              },
                            ),
                          ),
                          if (state.hasError)
                            Padding(
                              padding: const EdgeInsets.only(left: 12, top: 6),
                              child: Text(
                                state.errorText!,
                                style: const TextStyle(
                                  color: Colors.red,
                                  fontSize: 12,
                                ),
                              ),
                            ),
                        ],
                      ),
                    ),

                    const SizedBox(height: AppSpacing.md),

                    TextFormField(
                      controller: _cycleCtrl,
                      keyboardType: TextInputType.number,
                      decoration: _decoration(
                        label: 'Dužina ciklusa (dani)',
                        icon: Icons.repeat,
                      ),
                      validator: (v) {
                        if (v == null || v.trim().isEmpty) {
                          return 'Unesite dužinu ciklusa';
                        }

                        final n = int.tryParse(v);
                        if (n == null || n < 20 || n > 40) {
                          return 'Ciklus mora biti između 20 i 40 dana';
                        }

                        return null;
                      },
                    ),

                    const SizedBox(height: AppSpacing.md),
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
      ),
    );
  }
}
