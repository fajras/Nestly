import 'dart:io';
import 'package:path_provider/path_provider.dart';
import 'package:pdf/widgets.dart' as pw;
import '../screens/user_detail_screen.dart';
import 'package:flutter/services.dart';

class AdminPdfService {
  Future<File> generateMotherPdf({
    required String userName,
    required List<DetailItem> therapy,
    required List<DetailItem> symptoms,
    required List<DetailItem> questions,
  }) async {
    final font = pw.Font.ttf(
      await rootBundle.load("assets/fonts/RobotoSlab-Regular.ttf"),
    );

    final fontBold = pw.Font.ttf(
      await rootBundle.load("assets/fonts/RobotoSlab-Bold.ttf"),
    );
    final pdf = pw.Document();

    pdf.addPage(
      pw.MultiPage(
        theme: pw.ThemeData.withFont(base: font, bold: fontBold),
        build: (context) => [
          pw.Text(
            'Izvještaj o majci',
            style: pw.TextStyle(fontSize: 24, fontWeight: pw.FontWeight.bold),
          ),

          pw.SizedBox(height: 20),

          pw.Text('Korisnica: $userName'),

          pw.SizedBox(height: 20),

          _section("Terapija", therapy),
          _section("Simptomi", symptoms),
          _section("Pitanja", questions),
        ],
      ),
    );

    final dir = await getApplicationDocumentsDirectory();
    final file = File(
      '${dir.path}/mother_report_${DateTime.now().millisecondsSinceEpoch}.pdf',
    );

    await file.writeAsBytes(await pdf.save());

    return file;
  }

  Future<File> generateBabyPdf({
    required String userName,
    required List<DetailItem> meals,
    required List<DetailItem> health,
    required List<DetailItem> diapers,
    required List<DetailItem> sleep,
    required List<DetailItem> growth,
    required List<DetailItem> feeding,
    required List<DetailItem> milestones,
    required List<DetailItem> calendar,
  }) async {
    final font = pw.Font.ttf(
      await rootBundle.load("assets/fonts/RobotoSlab-Regular.ttf"),
    );
    final fontBold = pw.Font.ttf(
      await rootBundle.load("assets/fonts/RobotoSlab-Bold.ttf"),
    );
    final pdf = pw.Document();

    pdf.addPage(
      pw.MultiPage(
        theme: pw.ThemeData.withFont(base: font, bold: fontBold),
        build: (context) => [
          pw.Text(
            'Izvještaj o bebi',
            style: pw.TextStyle(fontSize: 24, fontWeight: pw.FontWeight.bold),
          ),

          pw.SizedBox(height: 20),

          pw.Text('Korisnica: $userName'),

          pw.SizedBox(height: 20),

          _section("Hrana", meals),
          _section("Zdravlje", health),
          _section("Pelene", diapers),
          _section("San bebe", sleep),
          _section("Rast bebe", growth),
          _section("Hranjenje", feeding),
          _section("Dostignuća", milestones),
          _section("Događaji", calendar),
        ],
      ),
    );

    final dir = await getApplicationDocumentsDirectory();
    final file = File(
      '${dir.path}/baby_report_${DateTime.now().millisecondsSinceEpoch}.pdf',
    );
    await file.writeAsBytes(await pdf.save());

    return file;
  }

  pw.Widget _section(String title, List<DetailItem> items) {
    if (items.isEmpty) {
      return pw.Column(
        crossAxisAlignment: pw.CrossAxisAlignment.start,
        children: [
          pw.Text(
            title,
            style: pw.TextStyle(fontSize: 18, fontWeight: pw.FontWeight.bold),
          ),
          pw.Text("Nema podataka"),
          pw.SizedBox(height: 20),
        ],
      );
    }

    return pw.Column(
      crossAxisAlignment: pw.CrossAxisAlignment.start,
      children: [
        pw.Text(
          title,
          style: pw.TextStyle(fontSize: 18, fontWeight: pw.FontWeight.bold),
        ),

        pw.SizedBox(height: 10),

        ...items.map(
          (e) => pw.Container(
            margin: const pw.EdgeInsets.only(bottom: 8),
            child: pw.Column(
              crossAxisAlignment: pw.CrossAxisAlignment.start,
              children: [
                pw.Text(
                  e.title,
                  style: pw.TextStyle(fontWeight: pw.FontWeight.bold),
                ),
                pw.Text(e.subtitle),
                pw.Text(e.meta),
              ],
            ),
          ),
        ),

        pw.SizedBox(height: 20),
      ],
    );
  }
}
