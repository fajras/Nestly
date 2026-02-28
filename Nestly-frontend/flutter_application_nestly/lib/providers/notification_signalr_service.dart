import 'dart:ui';

import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_top_notification.dart';
import 'package:flutter_application_nestly/main.dart';

class NotificationSignalRService {
  HubConnection? _connection;

  VoidCallback? _onNotification;

  Future<void> connect(
    String userId,
    String token, {
    VoidCallback? onNotification,
  }) async {
    _onNotification = onNotification;

    final hubUrl = "${ApiClient.baseUrl}/hubs/notifications";

    _connection = HubConnectionBuilder()
        .withUrl(
          hubUrl,
          options: HttpConnectionOptions(accessTokenFactory: () async => token),
        )
        .withAutomaticReconnect()
        .build();

    _connection!.on("ReceiveNotification", (arguments) {
      if (arguments == null || arguments.isEmpty) return;

      final data = arguments.first as Map;

      final title = data["title"] ?? "";
      final message = data["message"] ?? "";

      final context = navigatorKey.currentContext;
      if (context != null) {
        NestlyTopNotification.show(context, title: title, message: message);
      }

      if (_onNotification != null) {
        _onNotification!();
      }
    });

    await _connection!.start();
    await _connection!.invoke("JoinUserGroup", args: [userId]);
  }

  Future<void> disconnect() async {
    await _connection?.stop();
  }
}
