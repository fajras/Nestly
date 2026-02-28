import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/providers/notification_signalr_service.dart';
import 'package:flutter_application_nestly/screens/doctor_admin_dashboard_screen.dart';
import 'package:flutter_application_nestly/providers/splash_screen.dart';
import 'package:http/http.dart' as http;
import 'package:flutter/foundation.dart';
import 'dart:io';
import 'package:flutter_application_nestly/screens/home_dashboard.dart';
import 'package:flutter_application_nestly/screens/register.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:intl/date_symbol_data_local.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

final GlobalKey<NavigatorState> navigatorKey = GlobalKey<NavigatorState>();
final RouteObserver<ModalRoute<void>> routeObserver =
    RouteObserver<ModalRoute<void>>();

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();

  await initializeDateFormatting('bs_BA', null);

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

const bool forceLoginOnStart = true;

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
      navigatorObservers: [routeObserver],
      debugShowCheckedModeBanner: false,
      title: 'Nestly',
      navigatorKey: navigatorKey,
      theme: buildTheme(),

      locale: const Locale('bs', 'BA'),
      supportedLocales: const [Locale('bs', 'BA'), Locale('en', 'US')],
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],

      home: const SplashScreen(),
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
  final NotificationSignalRService _notificationService =
      NotificationSignalRService();
  bool _obscure = true;
  bool _loading = false;

  static String get _baseUrl {
    if (kIsWeb) {
      return 'http://localhost:5167';
    }

    if (Platform.isAndroid) {
      return 'http://10.0.2.2:5167';
    }

    if (Platform.isWindows || Platform.isMacOS || Platform.isLinux) {
      return 'http://localhost:5167';
    }

    return 'http://localhost:5167';
  }

  @override
  void dispose() {
    _emailCtrl.dispose();
    _pwCtrl.dispose();
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

  Future<void> _onLogin() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _loading = true);

    try {
      final uri = Uri.parse('$_baseUrl/api/Auth/login');

      final response = await http.post(
        uri,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'email': _emailCtrl.text.trim(),
          'password': _pwCtrl.text.trim(),
        }),
      );

      if (!mounted) return;

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);

        final token = data['token'];
        final role = data['role'];
        final parentProfileId = data['parentProfileId'];

        if (token == null || role == null) {
          NestlyToast.error(context, 'Neispravan odgovor sa servera');
          return;
        }

        final decoded = JwtDecoder.decode(token);
        final appUserId =
            decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

        if (appUserId == null) {
          NestlyToast.error(context, 'JWT ne sadrži user ID');
          return;
        }

        await AuthStorage.saveLogin(
          token: token,
          role: role,
          parentProfileId: parentProfileId,
        );

        if (role.toUpperCase() == 'PARENT') {
          Navigator.of(context).pushReplacement(
            MaterialPageRoute(
              builder: (_) =>
                  HomeDashboardScreen(parentProfileId: parentProfileId),
            ),
          );
        } else if (role.toUpperCase() == 'DOCTOR') {
          Navigator.of(context).pushReplacement(
            MaterialPageRoute(builder: (_) => DoctorAdminDashboardScreen()),
          );
        }
      } else {
        NestlyToast.error(context, 'Pogrešan email ili lozinka');
      }
    } catch (e) {
      print("LOGIN ERROR: $e");
      NestlyToast.error(context, 'Server nedostupan');
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  void _onRegister() {
    Navigator.of(
      context,
    ).push(MaterialPageRoute(builder: (_) => const RegisterScreen()));
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
        body: SafeArea(
          child: Center(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: ConstrainedBox(
                constraints: const BoxConstraints(maxWidth: 440),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    const _LogoHeader(),
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
                                'Prijava',
                                textAlign: TextAlign.center,
                                style: Theme.of(context).textTheme.headlineSmall
                                    ?.copyWith(
                                      fontWeight: FontWeight.w700,
                                      color: AppColors.roseDark,
                                    ),
                              ),
                              const SizedBox(height: AppSpacing.lg),

                              TextFormField(
                                controller: _emailCtrl,
                                keyboardType: TextInputType.emailAddress,
                                decoration: _decoration(
                                  label: 'Email',
                                  icon: Icons.alternate_email_rounded,
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
                                onFieldSubmitted: (_) => _onLogin(),
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
                                        onPressed: () => setState(
                                          () => _obscure = !_obscure,
                                        ),
                                      ),
                                    ),
                                validator: (v) => v == null || v.isEmpty
                                    ? 'Unesite lozinku'
                                    : null,
                              ),

                              const SizedBox(height: AppSpacing.xl),

                              SizedBox(
                                height: 52,
                                child: ElevatedButton(
                                  onPressed: _loading ? null : _onLogin,
                                  style: ElevatedButton.styleFrom(
                                    backgroundColor: AppColors.roseDark,
                                    foregroundColor: Colors.white,
                                  ),
                                  child: _loading
                                      ? const SizedBox(
                                          width: 20,
                                          height: 20,
                                          child: CircularProgressIndicator(
                                            strokeWidth: 2,
                                            valueColor:
                                                AlwaysStoppedAnimation<Color>(
                                                  Colors.white,
                                                ),
                                          ),
                                        )
                                      : const Text(
                                          'Prijavi se',
                                          style: TextStyle(
                                            fontWeight: FontWeight.w700,
                                            fontSize: 16,
                                          ),
                                        ),
                                ),
                              ),

                              const SizedBox(height: AppSpacing.md),

                              SizedBox(
                                height: 52,
                                child: OutlinedButton(
                                  onPressed: _loading ? null : _onRegister,
                                  style: OutlinedButton.styleFrom(
                                    side: const BorderSide(
                                      color: AppColors.roseDark,
                                      width: 1.4,
                                    ),
                                    foregroundColor: AppColors.roseDark,
                                    shape: RoundedRectangleBorder(
                                      borderRadius: BorderRadius.circular(
                                        AppRadius.lg,
                                      ),
                                    ),
                                  ),
                                  child: const Text(
                                    'Registruj se',
                                    style: TextStyle(
                                      fontWeight: FontWeight.w700,
                                    ),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),

                    const SizedBox(height: AppSpacing.lg),

                    Text(
                      'Nastavkom prihvatate Uslove korištenja i Politiku privatnosti',
                      textAlign: TextAlign.center,
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.textSecondary,
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

class _LogoHeader extends StatelessWidget {
  const _LogoHeader();

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Container(
          width: 200,
          height: 200,
          decoration: BoxDecoration(
            color: const Color.fromARGB(0, 255, 255, 255),
            borderRadius: BorderRadius.circular(28),
          ),
          alignment: Alignment.center,

          child: Image.asset(
            'assets/images/nestly_logo.png',
            fit: BoxFit.contain,
          ),
        ),

        const SizedBox(height: AppSpacing.lg),

        Text(
          'Dobrodošli',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),

        const SizedBox(height: AppSpacing.sm),

        Text(
          'Digitalni saputnik za trudnoću i rani razvoj',
          textAlign: TextAlign.center,
          style: Theme.of(
            context,
          ).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
        ),
      ],
    );
  }
}
