class AppConfig {
  static const String apiUrl = String.fromEnvironment('API_URL');

  static void validate() {
    if (apiUrl.isEmpty) {
      throw Exception(
        'API_URL is not defined. Start the app with --dart-define=API_URL=<url>',
      );
    }
  }
}
