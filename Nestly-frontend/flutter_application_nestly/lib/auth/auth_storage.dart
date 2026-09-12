import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:shared_preferences/shared_preferences.dart';

class AuthStorage {
  static const _secureStorage = FlutterSecureStorage();

  static const _tokenKey = 'auth_token';
  static const _refreshTokenKey = 'auth_refresh_token';
  static const _roleKey = 'auth_role';
  static const _parentIdKey = 'parent_profile_id';

  // flutter_secure_storage on web needs the Web Crypto API, which browsers
  // only expose on HTTPS/localhost origins. Fall back to SharedPreferences
  // on web so the app also works over plain HTTP (e.g. LAN IP demos).
  static Future<void> _write(String key, String? value) async {
    if (kIsWeb) {
      final prefs = await SharedPreferences.getInstance();
      if (value == null) {
        await prefs.remove(key);
      } else {
        await prefs.setString(key, value);
      }
    } else {
      await _secureStorage.write(key: key, value: value);
    }
  }

  static Future<String?> _read(String key) async {
    if (kIsWeb) {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getString(key);
    }
    return _secureStorage.read(key: key);
  }

  static Future<void> saveLogin({
    String? token,
    String? refreshToken,
    String? role,
    int? parentProfileId,
  }) async {
    await _write(_tokenKey, token);
    if (refreshToken != null) {
      await _write(_refreshTokenKey, refreshToken);
    }
    await _write(_roleKey, role);
    if (parentProfileId != null) {
      await _write(_parentIdKey, parentProfileId.toString());
    }
  }

  /// Persists a freshly rotated access/refresh token pair after a silent
  /// refresh, without touching the rest of the session (role, parent id).
  static Future<void> saveTokens({
    required String token,
    required String refreshToken,
  }) async {
    await _write(_tokenKey, token);
    await _write(_refreshTokenKey, refreshToken);
  }

  static Future<String?> getToken() async {
    return _read(_tokenKey);
  }

  static Future<String?> getRefreshToken() async {
    return _read(_refreshTokenKey);
  }

  static Future<int?> getUserId() async {
    final token = await _read(_tokenKey);
    if (token == null) return null;

    final decoded = JwtDecoder.decode(token);

    final userId = decoded["userId"];
    if (userId == null) return null;

    return int.tryParse(userId.toString());
  }

  static Future<String?> getRole() async {
    return _read(_roleKey);
  }

  static Future<int?> getParentProfileId() async {
    final v = await _read(_parentIdKey);
    return v == null ? null : int.tryParse(v);
  }

  static Future<void> clear() async {
    if (kIsWeb) {
      final prefs = await SharedPreferences.getInstance();
      await prefs.remove(_tokenKey);
      await prefs.remove(_refreshTokenKey);
      await prefs.remove(_roleKey);
      await prefs.remove(_parentIdKey);
    } else {
      await _secureStorage.deleteAll();
    }
  }
}
