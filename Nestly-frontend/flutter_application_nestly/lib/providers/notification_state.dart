import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/auth/auth_storage.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

class NotificationState extends ChangeNotifier {
  int _unreadCount = 0;

  int get unreadCount => _unreadCount;

  Future<void> loadUnreadCount() async {
    try {
      final token = await AuthStorage.getToken();
      final decoded = JwtDecoder.decode(token!);
      final userId = int.parse(
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
            .toString(),
      );

      final res = await ApiClient.get('/api/Notification/$userId');

      if (res.statusCode == 200) {
        final data = jsonDecode(res.body) as List<dynamic>;
        _unreadCount = data.where((n) => n["isRead"] == false).length;
        notifyListeners();
      }
    } catch (_) {}
  }

  void increment() {
    _unreadCount++;
    notifyListeners();
  }

  void reset() {
    _unreadCount = 0;
    notifyListeners();
  }
}

final notificationState = NotificationState();
