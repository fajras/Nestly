import 'dart:convert';

class ApiResponseHelper {
  static List<dynamic> extractList(String responseBody) {
    final decoded = jsonDecode(responseBody);

    if (decoded is List) {
      return decoded;
    }

    if (decoded is Map<String, dynamic>) {
      final items = decoded['items'];

      if (items is List) {
        return items;
      }
    }

    return [];
  }
}
