import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/screens/home_dashboard.dart';
import 'package:flutter_application_nestly/screens/register.dart';

void main() {
  runApp(const NestlyApp());
}

class AppColors {
  static const babyBlue = Color(0xFFA2D2FF);
  static const babyPink = Color(0xFFFFC8DD);
  static const seed = Color(0xFF00A6A6);
  static const bg = Color(0xFFFFFBEA);
  static const card = Color.fromARGB(255, 255, 254, 246);
  static const textPrimary = Color(0xFF0F172A);
  static const textSecondary = Color(0xFF475569);
  static const roseDark = Color.fromARGB(255, 168, 40, 89);
}

class AppRadius {
  static const xl = 24.0;
  static const lg = 16.0;
  static const md = 12.0;
}

class AppSpacing {
  static const xxl = 32.0;
  static const xl = 24.0;
  static const lg = 16.0;
  static const md = 12.0;
  static const sm = 8.0;
}

ThemeData buildTheme() {
  final base = ThemeData(useMaterial3: true, colorSchemeSeed: AppColors.seed);
  return base.copyWith(
    scaffoldBackgroundColor: AppColors.bg,
    textTheme: base.textTheme.apply(
      bodyColor: AppColors.textPrimary,
      displayColor: AppColors.textPrimary,
    ),
    inputDecorationTheme: InputDecorationTheme(
      isDense: true,
      filled: true,
      fillColor: Colors.white,
      contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide(color: Colors.black.withOpacity(0.08)),
      ),
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide(color: Colors.black.withOpacity(0.08)),
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: const BorderSide(color: AppColors.seed, width: 1.6),
      ),
      hintStyle: const TextStyle(color: AppColors.textSecondary),
    ),
    elevatedButtonTheme: ElevatedButtonThemeData(
      style: ElevatedButton.styleFrom(
        elevation: 0,
        padding: const EdgeInsets.symmetric(vertical: 14),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
        ),
        textStyle: const TextStyle(fontWeight: FontWeight.w600),
      ),
    ),
    outlinedButtonTheme: OutlinedButtonThemeData(
      style: OutlinedButton.styleFrom(
        padding: const EdgeInsets.symmetric(vertical: 14),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
        ),
        side: const BorderSide(color: AppColors.seed, width: 1.2),
        textStyle: const TextStyle(fontWeight: FontWeight.w600),
        foregroundColor: AppColors.seed,
      ),
    ),
    cardTheme: CardThemeData(
      color: AppColors.card,
      elevation: 2,
      margin: EdgeInsets.zero,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
    ),
  );
}

class NestlyApp extends StatelessWidget {
  const NestlyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Nestly',
      theme: buildTheme(),
      home: const LoginScreen(),
    );
  }
}

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final _emailCtrl = TextEditingController();
  final _pwCtrl = TextEditingController();
  bool _obscure = true;
  bool _loading = false;

  @override
  void dispose() {
    _emailCtrl.dispose();
    _pwCtrl.dispose();
    super.dispose();
  }

  Future<void> _onLogin() async {
    Navigator.of(context).pushReplacement(
      MaterialPageRoute(
        builder: (_) =>
            HomeDashboardScreen(conceptionDate: DateTime(2025, 1, 5)),
      ),
    );
  }

  void _onRegister() {
    Navigator.of(
      context,
    ).push(MaterialPageRoute(builder: (_) => const RegisterScreen()));
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
              constraints: const BoxConstraints(maxWidth: 440),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // HEADER (logo + naslov)
                  const _LogoHeader(),
                  const SizedBox(height: AppSpacing.xl),

                  // KARTICA S FORMOM
                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(AppSpacing.xl),
                      child: Form(
                        key: _formKey,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.stretch,
                          children: [
                            Text(
                              'Prijava',
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
                              validator: (v) {
                                if (v == null || v.trim().isEmpty) {
                                  return 'Unesite email';
                                }
                                final ok = RegExp(
                                  r'^[^@\n]+@[^@\n]+\.[^@\n]+$',
                                ).hasMatch(v.trim());
                                return ok ? null : 'Email nije ispravan';
                              },
                            ),
                            const SizedBox(height: AppSpacing.md),

                            TextFormField(
                              controller: _pwCtrl,
                              obscureText: _obscure,
                              textInputAction: TextInputAction.done,
                              onFieldSubmitted: (_) => _onLogin(),
                              decoration: InputDecoration(
                                labelText: 'Lozinka',
                                prefixIcon: const Icon(
                                  Icons.lock_outline_rounded,
                                ),
                                suffixIcon: IconButton(
                                  onPressed: () => setState(() {
                                    _obscure = !_obscure;
                                  }),
                                  icon: Icon(
                                    _obscure
                                        ? Icons.visibility
                                        : Icons.visibility_off,
                                  ),
                                  tooltip: _obscure ? 'Prika\u017Ei' : 'Sakrij',
                                ),
                              ),
                              validator: (v) {
                                if (v == null || v.isEmpty) {
                                  return 'Unesite lozinku';
                                }
                                if (v.length < 6) {
                                  return 'Minimalno 6 znakova';
                                }
                                return null;
                              },
                            ),

                            const SizedBox(height: AppSpacing.xl),

                            // PRIJAVI SE (primary)
                            SizedBox(
                              height: 52,
                              child: ElevatedButton(
                                onPressed: _loading ? null : _onLogin,
                                child: _loading
                                    ? const SizedBox(
                                        width: 22,
                                        height: 22,
                                        child: CircularProgressIndicator(
                                          strokeWidth: 2.4,
                                        ),
                                      )
                                    : const Text('Prijavi se'),
                              ),
                            ),

                            const SizedBox(height: AppSpacing.md),

                            // REGISTRUJ SE (secondary)
                            SizedBox(
                              height: 52,
                              child: OutlinedButton(
                                onPressed: _onRegister,
                                child: const Text('Registruj se'),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),

                  const SizedBox(height: AppSpacing.lg),

                  // FOOTER (terms / help linkovi - po\u017Eeljno kasnije)
                  Text(
                    'Nastavkom prihvatate Uslove kori\u0161tenja i Politiku privatnosti',
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

class _LogoHeader extends StatelessWidget {
  const _LogoHeader();

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Privremeni logo/slika dok ne ubacimo pravi (FlutterLogo je lagan i lijep)
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
          'Dobrodo\u0161li u Nestly',
          style: Theme.of(
            context,
          ).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.w700),
        ),
        const SizedBox(height: AppSpacing.sm),
        Text(
          'Digitalni saputnik za trudno\u0107u i rani razvoj',
          style: Theme.of(
            context,
          ).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }
}

/// Privremeni ekran nakon uspje\u0161ne prijave
class _DummyHome extends StatelessWidget {
  const _DummyHome();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Nestly')),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const Icon(Icons.check_circle_outline, size: 64),
              const SizedBox(height: AppSpacing.lg),
              const Text('Uspje\u0161na prijava! Ovo je demo Home.'),
              const SizedBox(height: AppSpacing.lg),
              ElevatedButton(
                onPressed: () => Navigator.of(
                  context,
                ).push(MaterialPageRoute(builder: (_) => const LoginScreen())),
                child: const Text('Odjava (nazad na Login)'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
