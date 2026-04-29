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
      final res = await ApiClient.get('/api/Notification/unread-count');

      if (res.statusCode == 200) {
        _unreadCount = jsonDecode(res.body);
        notifyListeners();
      }
    } catch (e) {
      debugPrint("Unread error: $e");
    }
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
