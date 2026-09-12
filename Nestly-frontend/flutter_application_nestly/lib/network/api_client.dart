import 'dart:convert';
import 'dart:io';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/main.dart';

class ApiClient {
  static const String _baseUrl = String.fromEnvironment('API_URL');

  static bool _isRedirecting = false;

  // Concurrent 401s (several requests in flight at once) must not each
  // trigger their own refresh call - they'd race to rotate the same
  // refresh token and all but the first would fail. Every caller awaits
  // this same in-flight future instead.
  static Future<bool>? _refreshInFlight;

  static String get baseUrl {
    if (_baseUrl.isEmpty) {
      throw Exception(
        'API_URL nije definisan. Pokreni aplikaciju sa --dart-define=API_URL=<url>',
      );
    }

    return _baseUrl;
  }

  static Future<void> _handleUnauthorized() async {
    if (_isRedirecting) return;

    _isRedirecting = true;

    await AuthStorage.clear();

    navigatorKey.currentState?.pushAndRemoveUntil(
      MaterialPageRoute(builder: (_) => const LoginScreen()),
      (route) => false,
    );

    _isRedirecting = false;
  }

  /// Exchanges the stored refresh token for a new access/refresh token
  /// pair. Returns false (without throwing) if there's no refresh token,
  /// or the server rejects it (expired/revoked) - the caller then falls
  /// back to a full logout.
  static Future<bool> _refreshAccessToken() {
    return _refreshInFlight ??= () async {
      try {
        final refreshToken = await AuthStorage.getRefreshToken();
        if (refreshToken == null) return false;

        final response = await http.post(
          Uri.parse('$baseUrl/api/auth/refresh'),
          headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
          },
          body: jsonEncode({'refreshToken': refreshToken}),
        );

        if (response.statusCode != 200) return false;

        final data = jsonDecode(response.body);
        final newToken = data['token'];
        final newRefreshToken = data['refreshToken'];

        if (newToken == null || newRefreshToken == null) return false;

        await AuthStorage.saveTokens(
          token: newToken,
          refreshToken: newRefreshToken,
        );

        return true;
      } catch (_) {
        return false;
      }
    }().whenComplete(() => _refreshInFlight = null);
  }

  static Future<http.Response> _checkResponse(
    http.Response response, {
    bool skipUnauthorizedHandler = false,
  }) async {
    if (response.statusCode == 401 && !skipUnauthorizedHandler) {
      await _handleUnauthorized();
    }

    return response;
  }

  static Map<String, String> _headers(String? token) {
    final headers = <String, String>{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };

    if (token != null) {
      headers['Authorization'] = 'Bearer $token';
    }

    return headers;
  }

  /// Runs [send] with the current access token; on a 401 it tries a
  /// silent refresh once and retries [send] with the new token before
  /// giving up and letting `_checkResponse` sign the user out.
  static Future<http.Response> _sendWithRefresh(
    Future<http.Response> Function(String? token) send, {
    bool skipUnauthorizedHandler = false,
  }) async {
    final token = await AuthStorage.getToken();
    var response = await send(token);

    if (response.statusCode == 401 && !skipUnauthorizedHandler) {
      final refreshed = await _refreshAccessToken();

      if (refreshed) {
        final newToken = await AuthStorage.getToken();
        response = await send(newToken);
      }
    }

    return _checkResponse(
      response,
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> get(
    String path, {
    bool skipUnauthorizedHandler = false,
  }) {
    return _sendWithRefresh(
      (token) => http.get(Uri.parse('$baseUrl$path'), headers: _headers(token)),
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> post(
    String path, {
    Object? body,
    bool skipUnauthorizedHandler = false,
  }) {
    return _sendWithRefresh(
      (token) => http.post(
        Uri.parse('$baseUrl$path'),
        headers: _headers(token),
        body: body == null ? null : jsonEncode(body),
      ),
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> patch(
    String path, {
    Object? body,
    bool skipUnauthorizedHandler = false,
  }) {
    return _sendWithRefresh(
      (token) => http.patch(
        Uri.parse('$baseUrl$path'),
        headers: _headers(token),
        body: body == null ? null : jsonEncode(body),
      ),
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> delete(
    String path, {
    bool skipUnauthorizedHandler = false,
  }) {
    return _sendWithRefresh(
      (token) => http.delete(Uri.parse('$baseUrl$path'), headers: _headers(token)),
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<void> multipart(
    String path, {
    required File file,
    bool skipUnauthorizedHandler = false,
  }) async {
    Future<http.Response> sendOnce(String? token) async {
      final req = http.MultipartRequest('POST', Uri.parse('$baseUrl$path'));

      if (token != null) {
        req.headers['Authorization'] = 'Bearer $token';
      }

      req.files.add(
        await http.MultipartFile.fromPath(
          'file',
          file.path,
          filename: file.path.split('/').last,
        ),
      );

      final streamed = await req.send();
      return http.Response.fromStream(streamed);
    }

    final res = await _sendWithRefresh(
      sendOnce,
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );

    if (res.statusCode == 401) {
      // Already handled (redirect to login) by _sendWithRefresh/_checkResponse.
      return;
    }

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception(res.body);
    }
  }

  /// Best-effort server-side revocation of the current session's refresh
  /// token (and the access token used to call it), then clears local
  /// storage. Call this instead of `AuthStorage.clear()` directly whenever
  /// the user explicitly signs out, so a manually-logged-out token can't
  /// still be replayed until it naturally expires.
  static Future<void> logout() async {
    try {
      final refreshToken = await AuthStorage.getRefreshToken();

      if (refreshToken != null) {
        await post(
          '/api/auth/logout',
          body: {'refreshToken': refreshToken},
          skipUnauthorizedHandler: true,
        );
      }
    } catch (_) {
      // Best-effort: local sign-out must proceed even if the server call
      // fails (offline, token already expired, etc).
    } finally {
      await AuthStorage.clear();
    }
  }
}
