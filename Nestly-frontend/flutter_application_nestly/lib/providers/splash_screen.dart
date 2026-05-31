import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/screens/home_dashboard.dart';
import 'package:flutter_application_nestly/screens/doctor_admin_dashboard_screen.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen> {
  @override
  void initState() {
    super.initState();
    _bootstrap();
  }

  Future<void> _bootstrap() async {
    if (forceLoginOnStart) {
      await AuthStorage.clear();

      if (!mounted) return;

      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      );

      return;
    }

    final token = await AuthStorage.getToken();
    final role = await AuthStorage.getRole();
    final parentId = await AuthStorage.getParentProfileId();

    if (token != null && JwtDecoder.isExpired(token)) {
      await AuthStorage.clear();

      if (!mounted) return;

      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      );

      return;
    }

    if (!mounted) return;

    if (token == null || role == null) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      );

      return;
    }

    if (role.toUpperCase() == 'PARENT' && parentId != null) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => HomeDashboardScreen()),
      );
    } else if (role.toUpperCase() == 'DOCTOR') {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const DoctorAdminDashboardScreen()),
      );
    } else {
      await AuthStorage.clear();

      if (!mounted) return;

      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return const Scaffold(body: Center(child: CircularProgressIndicator()));
  }
}
