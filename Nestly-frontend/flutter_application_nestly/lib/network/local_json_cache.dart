import 'package:shared_preferences/shared_preferences.dart';

/// Tiny disk-backed cache for raw JSON API responses, keyed by a caller-
/// chosen string. Several read-mostly screens (fetal development per week,
/// weekly pregnancy advice) previously kept their cache purely in memory,
/// so it was lost on every app restart and offered nothing when offline.
/// Persisting the last-known-good response here lets those screens show
/// something useful immediately on next launch, and fall back to it when
/// a network request fails - a first step toward an offline mode, without
/// building a full local database/sync layer.
class LocalJsonCache {
  static const _prefix = 'json_cache_';

  static Future<void> putRaw(String key, String rawJson) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('$_prefix$key', rawJson);
    } catch (_) {
      // Best-effort: a failed write (e.g. storage unavailable) must never
      // break the screen that's just trying to show fresh data it already has.
    }
  }

  static Future<String?> getRaw(String key) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      return prefs.getString('$_prefix$key');
    } catch (_) {
      return null;
    }
  }
}
