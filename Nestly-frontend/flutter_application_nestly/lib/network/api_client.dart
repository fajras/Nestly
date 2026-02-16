import 'dart:convert';
import 'dart:io';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_application_nestly/auth/auth_storage.dart';

class ApiClient {
  static String get _baseUrl {
    if (kIsWeb) {
      return 'http://localhost:5167';
    }
    if (Platform.isAndroid) {
      return 'http://10.0.2.2:5167';
    }
    return 'http://localhost:5167';
  }

  static String get baseUrl => _baseUrl;

  static Future<http.Response> get(String path) async {
    final token = await AuthStorage.getToken();
    print("GET $path");
    print("TOKEN: $token");

    return http.get(Uri.parse('$_baseUrl$path'), headers: _headers(token));
  }

  static Future<http.Response> post(String path, {Object? body}) async {
    final token = await AuthStorage.getToken();

    return http.post(
      Uri.parse('$_baseUrl$path'),
      headers: _headers(token),
      body: body == null ? null : jsonEncode(body),
    );
  }

  static Future<http.Response> patch(String path, {Object? body}) async {
    final token = await AuthStorage.getToken();

    final headers = <String, String>{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };

    if (token != null) {
      headers['Authorization'] = 'Bearer $token';
    }

    return http.patch(
      Uri.parse('$_baseUrl$path'),
      headers: headers,
      body: body == null ? null : jsonEncode(body),
    );
  }

  static Future<void> multipart(String path, {required File file}) async {
    final token = await AuthStorage.getToken();

    final req = http.MultipartRequest('POST', Uri.parse('$_baseUrl$path'));

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

    if (res.statusCode != 200 && res.statusCode != 201) {
      final body = await res.stream.bytesToString();
      throw Exception(body);
    }
  }

  static Map<String, String> _headers(String? token) {
    final headers = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };

    if (token != null) {
      headers['Authorization'] = 'Bearer $token';
    }

    return headers;
  }

  static Future<http.Response> delete(String path) async {
    final token = await AuthStorage.getToken();

    return http.delete(Uri.parse('$_baseUrl$path'), headers: _headers(token));
  }
}
