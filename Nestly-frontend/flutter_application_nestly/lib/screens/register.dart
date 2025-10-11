import 'package:flutter/material.dart';
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
  DateTime? _conceptionDate; // 🟢 NOVO
  String? _gender;

  bool _obscure = true;
  bool _loading = false;

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

  Future<void> _pickDateOfBirth() async {
    final now = DateTime.now();
    final first = DateTime(now.year - 100, 1, 1);
    final last = DateTime(now.year, now.month, now.day);
    final picked = await showDatePicker(
      context: context,
      initialDate: _dob ?? DateTime(now.year - 20, now.month, now.day),
      firstDate: first,
      lastDate: last,
      helpText: 'Odaberi datum rođenja',
    );
    if (picked != null) {
      setState(() => _dob = picked);
    }
  }

  // 🟢 NOVO — Date picker za datum začeća
  Future<void> _pickConceptionDate() async {
    final now = DateTime.now();
    final first = DateTime(now.year - 2, 1, 1);
    final last = DateTime(now.year, now.month, now.day);
    final picked = await showDatePicker(
      context: context,
      initialDate: _conceptionDate ?? DateTime(now.year, now.month, now.day),
      firstDate: first,
      lastDate: last,
      helpText: 'Odaberi datum začeća',
    );
    if (picked != null) {
      setState(() => _conceptionDate = picked);
    }
  }

  String? _required(String? v, {String msg = 'Obavezno polje'}) {
    if (v == null || v.trim().isEmpty) return msg;
    return null;
  }

  String? _validateEmail(String? v) {
    if (_required(v) != null) return 'Unesite email';
    final ok = RegExp(r'^[^@\n]+@[^@\n]+\.[^@\n]+$').hasMatch(v!.trim());
    return ok ? null : 'Email nije ispravan';
  }

  String? _validatePassword(String? v) {
    if (_required(v) != null) return 'Unesite lozinku';
    if (v!.length < 6) return 'Minimalno 6 znakova';
    return null;
  }

  String? _validatePhone(String? v) {
    if (_required(v) != null) return 'Unesite broj telefona';
    final cleaned = v!.replaceAll(RegExp(r'[^0-9+]'), '');
    if (cleaned.length < 6) return 'Broj telefona nije ispravan';
    return null;
  }

  Future<void> _onSubmit() async {
    final valid = _formKey.currentState?.validate() ?? false;
    if (!valid) return;

    if (_dob == null) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Odaberite datum rođenja')));
      return;
    }
    if (_gender == null) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Odaberite spol')));
      return;
    }
    if (_conceptionDate == null) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Odaberite datum začeća')));
      return;
    }

    setState(() => _loading = true);
    await Future.delayed(const Duration(milliseconds: 900));
    setState(() => _loading = false);

    // 🟢 Dodano conceptionDate u payload
    final payload = {
      "email": _emailCtrl.text.trim(),
      "firstName": _firstNameCtrl.text.trim(),
      "lastName": _lastNameCtrl.text.trim(),
      "phoneNumber": _phoneCtrl.text.trim(),
      "dateOfBirth": _dob!.toIso8601String(),
      "conceptionDate": _conceptionDate!.toIso8601String(),
      "gender": _gender,
      "username": _usernameCtrl.text.trim(),
      "password": _passwordCtrl.text,
    };

    // ignore: use_build_context_synchronously
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Registracija uspješna (demo). Payload: $payload'),
      ),
    );

    if (!mounted) return;
    Navigator.pop(context);
  }

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.sizeOf(context);

    return Scaffold(
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 520),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  const _LogoHeaderRegister(),
                  const SizedBox(height: AppSpacing.xl),

                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(AppSpacing.xl),
                      child: Form(
                        key: _formKey,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.stretch,
                          children: [
                            Text(
                              'Registracija',
                              style: Theme.of(context).textTheme.headlineSmall
                                  ?.copyWith(fontWeight: FontWeight.w700),
                              textAlign: TextAlign.center,
                            ),
                            const SizedBox(height: AppSpacing.lg),

                            TextFormField(
                              controller: _emailCtrl,
                              keyboardType: TextInputType.emailAddress,
                              textInputAction: TextInputAction.next,
                              decoration: const InputDecoration(
                                labelText: 'Email',
                                hintText: 'primjer@nestly.app',
                                prefixIcon: Icon(Icons.alternate_email_rounded),
                              ),
                              validator: _validateEmail,
                            ),
                            const SizedBox(height: AppSpacing.md),

                            TextFormField(
                              controller: _firstNameCtrl,
                              textInputAction: TextInputAction.next,
                              decoration: const InputDecoration(
                                labelText: 'Ime',
                                prefixIcon: Icon(Icons.person_outline_rounded),
                              ),
                              validator: _required,
                            ),
                            const SizedBox(height: AppSpacing.md),

                            TextFormField(
                              controller: _lastNameCtrl,
                              textInputAction: TextInputAction.next,
                              decoration: const InputDecoration(
                                labelText: 'Prezime',
                                prefixIcon: Icon(Icons.person_2_outlined),
                              ),
                              validator: _required,
                            ),
                            const SizedBox(height: AppSpacing.md),

                            TextFormField(
                              controller: _phoneCtrl,
                              keyboardType: TextInputType.phone,
                              textInputAction: TextInputAction.next,
                              decoration: const InputDecoration(
                                labelText: 'Telefon',
                                hintText: '+387 61 234 567',
                                prefixIcon: Icon(Icons.phone_outlined),
                              ),
                              validator: _validatePhone,
                            ),
                            const SizedBox(height: AppSpacing.md),

                            // 🟢 Datum rođenja
                            GestureDetector(
                              onTap: _pickDateOfBirth,
                              child: AbsorbPointer(
                                child: TextFormField(
                                  readOnly: true,
                                  decoration: InputDecoration(
                                    labelText: 'Datum rođenja',
                                    hintText: 'Odaberite datum',
                                    prefixIcon: const Icon(Icons.cake_outlined),
                                    suffixIcon: IconButton(
                                      onPressed: _pickDateOfBirth,
                                      icon: const Icon(Icons.calendar_today),
                                    ),
                                  ),
                                  controller: TextEditingController(
                                    text: _dob == null
                                        ? ''
                                        : '${_dob!.day.toString().padLeft(2, '0')}.${_dob!.month.toString().padLeft(2, '0')}.${_dob!.year}',
                                  ),
                                ),
                              ),
                            ),
                            const SizedBox(height: AppSpacing.md),

                            // 🟢 Datum začeća (novo polje)
                            GestureDetector(
                              onTap: _pickConceptionDate,
                              child: AbsorbPointer(
                                child: TextFormField(
                                  readOnly: true,
                                  decoration: InputDecoration(
                                    labelText: 'Datum začeća',
                                    hintText: 'Odaberite datum',
                                    prefixIcon: const Icon(
                                      Icons.favorite_outline_rounded,
                                    ),
                                    suffixIcon: IconButton(
                                      onPressed: _pickConceptionDate,
                                      icon: const Icon(Icons.calendar_today),
                                    ),
                                  ),
                                  controller: TextEditingController(
                                    text: _conceptionDate == null
                                        ? ''
                                        : '${_conceptionDate!.day.toString().padLeft(2, '0')}.${_conceptionDate!.month.toString().padLeft(2, '0')}.${_conceptionDate!.year}',
                                  ),
                                ),
                              ),
                            ),
                            const SizedBox(height: AppSpacing.md),

                            DropdownButtonFormField<String>(
                              value: _gender,
                              items: const [
                                DropdownMenuItem(
                                  value: 'F',
                                  child: Text('Žensko'),
                                ),
                                DropdownMenuItem(
                                  value: 'M',
                                  child: Text('Muško'),
                                ),
                              ],
                              onChanged: (v) => setState(() => _gender = v),
                              decoration: const InputDecoration(
                                labelText: 'Spol',
                                prefixIcon: Icon(Icons.wc_outlined),
                              ),
                              validator: (v) =>
                                  v == null ? 'Odaberite spol' : null,
                            ),
                            const SizedBox(height: AppSpacing.md),

                            TextFormField(
                              controller: _usernameCtrl,
                              textInputAction: TextInputAction.next,
                              decoration: const InputDecoration(
                                labelText: 'Korisničko ime',
                                prefixIcon: Icon(Icons.badge_outlined),
                              ),
                              validator: _required,
                            ),
                            const SizedBox(height: AppSpacing.md),

                            TextFormField(
                              controller: _passwordCtrl,
                              obscureText: _obscure,
                              textInputAction: TextInputAction.done,
                              onFieldSubmitted: (_) => _onSubmit(),
                              decoration: InputDecoration(
                                labelText: 'Lozinka',
                                prefixIcon: const Icon(
                                  Icons.lock_outline_rounded,
                                ),
                                suffixIcon: IconButton(
                                  onPressed: () =>
                                      setState(() => _obscure = !_obscure),
                                  icon: Icon(
                                    _obscure
                                        ? Icons.visibility
                                        : Icons.visibility_off,
                                  ),
                                  tooltip: _obscure ? 'Prikaži' : 'Sakrij',
                                ),
                              ),
                              validator: _validatePassword,
                            ),

                            const SizedBox(height: AppSpacing.xl),

                            SizedBox(
                              height: 52,
                              child: ElevatedButton(
                                onPressed: _loading ? null : _onSubmit,
                                child: _loading
                                    ? const SizedBox(
                                        width: 22,
                                        height: 22,
                                        child: CircularProgressIndicator(
                                          strokeWidth: 2.4,
                                        ),
                                      )
                                    : const Text('Kreiraj nalog'),
                              ),
                            ),
                            const SizedBox(height: AppSpacing.md),

                            SizedBox(
                              height: 52,
                              child: OutlinedButton(
                                onPressed: _loading
                                    ? null
                                    : () => Navigator.pop(context),
                                child: const Text('Imate nalog? Prijavite se'),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                  const SizedBox(height: AppSpacing.lg),

                  Text(
                    'Kreiranjem naloga prihvatate Uslove korištenja i Politiku privatnosti',
                    textAlign: TextAlign.center,
                    style: Theme.of(context).textTheme.bodySmall?.copyWith(
                      color: AppColors.textSecondary,
                    ),
                  ),
                  SizedBox(height: size.height * 0.04),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class _LogoHeaderRegister extends StatelessWidget {
  const _LogoHeaderRegister();

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Container(
          width: 96,
          height: 96,
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(24),
            boxShadow: [
              BoxShadow(
                color: Colors.black.withOpacity(0.05),
                blurRadius: 16,
                offset: const Offset(0, 8),
              ),
            ],
          ),
          alignment: Alignment.center,
          child: const FlutterLogo(size: 56),
        ),
        const SizedBox(height: AppSpacing.lg),
        Text(
          'Kreirajte svoj Nestly nalog',
          style: Theme.of(
            context,
          ).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.w700),
        ),
        const SizedBox(height: AppSpacing.sm),
        Text(
          'Brz početak – samo nekoliko detalja',
          style: Theme.of(
            context,
          ).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }
}
