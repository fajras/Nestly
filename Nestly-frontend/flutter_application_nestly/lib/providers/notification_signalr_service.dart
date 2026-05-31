import 'dart:async';
import 'dart:ui';

import 'package:flutter_application_nestly/layouts/nestly_top_notification.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:signalr_netcore/signalr_client.dart';

class NotificationSignalRService {
  HubConnection? _connection;

  VoidCallback? _onNotification;

  Timer? _pollingTimer;

  Future<void> connect(String token, {VoidCallback? onNotification}) async {
    try {
      _onNotification = onNotification;

      final hubUrl = "${ApiClient.baseUrl}/hubs/notifications";

      _connection = HubConnectionBuilder()
          .withUrl(
            hubUrl,
            options: HttpConnectionOptions(
              accessTokenFactory: () async => token,
            ),
          )
          .withAutomaticReconnect()
          .build();

      _connection!.onclose(({error}) async {});

      _connection!.onreconnecting(({error}) async {});

      _connection!.onreconnected(({connectionId}) async {});

      _connection!.on("ReceiveNotification", (arguments) {
        if (arguments == null || arguments.isEmpty) {
          return;
        }

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

      _startPolling();
    } catch (e) {}
  }

  void _startPolling() {
    _pollingTimer?.cancel();

    _pollingTimer = Timer.periodic(const Duration(seconds: 30), (_) async {
      if (_onNotification != null) {
        _onNotification!();
      }
    });
  }

  Future<void> disconnect() async {
    _pollingTimer?.cancel();

    await _connection?.stop();
  }
}
