import 'dart:convert';
import 'dart:io';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/main.dart';

class ApiClient {
  static const String _baseUrl = String.fromEnvironment('API_URL');

  static bool _isRedirecting = false;

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

  static Future<http.Response> get(
    String path, {
    bool skipUnauthorizedHandler = false,
  }) async {
    final token = await AuthStorage.getToken();

    final response = await http.get(
      Uri.parse('$baseUrl$path'),
      headers: _headers(token),
    );

    return _checkResponse(
      response,
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> post(
    String path, {
    Object? body,
    bool skipUnauthorizedHandler = false,
  }) async {
    final token = await AuthStorage.getToken();

    final response = await http.post(
      Uri.parse('$baseUrl$path'),
      headers: _headers(token),
      body: body == null ? null : jsonEncode(body),
    );

    return _checkResponse(
      response,
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> patch(
    String path, {
    Object? body,
    bool skipUnauthorizedHandler = false,
  }) async {
    final token = await AuthStorage.getToken();

    final response = await http.patch(
      Uri.parse('$baseUrl$path'),
      headers: _headers(token),
      body: body == null ? null : jsonEncode(body),
    );

    return _checkResponse(
      response,
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<http.Response> delete(
    String path, {
    bool skipUnauthorizedHandler = false,
  }) async {
    final token = await AuthStorage.getToken();

    final response = await http.delete(
      Uri.parse('$baseUrl$path'),
      headers: _headers(token),
    );

    return _checkResponse(
      response,
      skipUnauthorizedHandler: skipUnauthorizedHandler,
    );
  }

  static Future<void> multipart(
    String path, {
    required File file,
    bool skipUnauthorizedHandler = false,
  }) async {
    final token = await AuthStorage.getToken();

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

    final res = await req.send();

    if (res.statusCode == 401 && !skipUnauthorizedHandler) {
      await _handleUnauthorized();
      return;
    }

    if (res.statusCode != 200 && res.statusCode != 201) {
      final body = await res.stream.bytesToString();
      throw Exception(body);
    }
  }
}
