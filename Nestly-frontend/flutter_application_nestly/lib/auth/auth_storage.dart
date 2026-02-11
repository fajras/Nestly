import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthStorage {
  static const _storage = FlutterSecureStorage();

  static const _tokenKey = 'auth_token';
  static const _roleKey = 'auth_role';
  static const _parentIdKey = 'parent_profile_id';

  static Future<void> saveLogin({
    String? token,
    String? role,
    int? parentProfileId,
  }) async {
    await _storage.write(key: _tokenKey, value: token);
    await _storage.write(key: _roleKey, value: role);
    if (parentProfileId != null) {
      await _storage.write(
        key: _parentIdKey,
        value: parentProfileId.toString(),
      );
    }
  }

  static Future<String?> getToken() async {
    return _storage.read(key: _tokenKey);
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
