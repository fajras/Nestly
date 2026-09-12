import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

class AuthStorage {
  static const _storage = FlutterSecureStorage();

  static const _tokenKey = 'auth_token';
  static const _refreshTokenKey = 'auth_refresh_token';
  static const _roleKey = 'auth_role';
  static const _parentIdKey = 'parent_profile_id';

  static Future<void> saveLogin({
    String? token,
    String? refreshToken,
    String? role,
    int? parentProfileId,
  }) async {
    await _storage.write(key: _tokenKey, value: token);
    if (refreshToken != null) {
      await _storage.write(key: _refreshTokenKey, value: refreshToken);
    }
    await _storage.write(key: _roleKey, value: role);
    if (parentProfileId != null) {
      await _storage.write(
        key: _parentIdKey,
        value: parentProfileId.toString(),
      );
    }
  }

  /// Persists a freshly rotated access/refresh token pair after a silent
  /// refresh, without touching the rest of the session (role, parent id).
  static Future<void> saveTokens({
    required String token,
    required String refreshToken,
  }) async {
    await _storage.write(key: _tokenKey, value: token);
    await _storage.write(key: _refreshTokenKey, value: refreshToken);
  }

  static Future<String?> getToken() async {
    return _storage.read(key: _tokenKey);
  }

  static Future<String?> getRefreshToken() async {
    return _storage.read(key: _refreshTokenKey);
  }

  static Future<int?> getUserId() async {
    final token = await _storage.read(key: _tokenKey);
    if (token == null) return null;

    final decoded = JwtDecoder.decode(token);

    final userId = decoded["userId"];
    if (userId == null) return null;

    return int.tryParse(userId.toString());
  }

  static Future<String?> getRole() async {
    return _storage.read(key: _roleKey);
  }

  static Future<int?> getParentProfileId() async {
    final v = await _storage.read(key: _parentIdKey);
    return v == null ? null : int.tryParse(v);
  }

  static Future<void> clear() async {
    await _storage.deleteAll();
  }
}
