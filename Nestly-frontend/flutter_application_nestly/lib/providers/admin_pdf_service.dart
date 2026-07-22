import 'dart:io';

import 'package:path_provider/path_provider.dart';
import 'package:pdf/pdf.dart';
import 'package:pdf/widgets.dart' as pw;
import 'package:printing/printing.dart';

import '../screens/user_detail_screen.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class _PdfPalette {
  static const roseDark = PdfColor.fromInt(0xFFA82859);
  static const babyBlue = PdfColor.fromInt(0xFFA2D2FF);
  static const seed = PdfColor.fromInt(0xFF00A6A6);
  static const bg = PdfColor.fromInt(0xFFFFFBEA);
  static const textPrimary = PdfColor.fromInt(0xFF0F172A);
  static const textSecondary = PdfColor.fromInt(0xFF475569);
  static const divider = PdfColor.fromInt(0xFFE8E1D5);
}

class AdminPdfService {
  Future<Uint8List> generateMotherPdfBytes({
    required String userName,
    required List<DetailItem> therapy,
    required List<DetailItem> symptoms,
    required List<DetailItem> questions,
  }) async {
    return _buildReport(
      reportTitle: 'Izvještaj o majci',
      userName: userName,
      sections: {
        'Terapija': therapy,
        'Simptomi': symptoms,
        'Pitanja': questions,
      },
    );
  }

  Future<Uint8List> generateBabyPdfBytes({
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
    return _buildReport(
      reportTitle: 'Izvještaj o bebi',
      userName: userName,
      sections: {
        'Hrana': meals,
        'Zdravlje': health,
        'Pelene': diapers,
        'San bebe': sleep,
        'Rast bebe': growth,
        'Hranjenje': feeding,
        'Dostignuća': milestones,
        'Događaji': calendar,
      },
    );
  }

  Future<Uint8List> _buildReport({
    required String reportTitle,
    required String userName,
    required Map<String, List<DetailItem>> sections,
  }) async {
    final font = pw.Font.ttf(
      await rootBundle.load("assets/fonts/RobotoSlab-Regular.ttf"),
    );

    final fontBold = pw.Font.ttf(
      await rootBundle.load("assets/fonts/RobotoSlab-Bold.ttf"),
    );

    final pdf = pw.Document();
    final generatedAt = DateFormat('dd.MM.yyyy HH:mm').format(DateTime.now());

    pdf.addPage(
      pw.MultiPage(
        theme: pw.ThemeData.withFont(base: font, bold: fontBold),
        margin: const pw.EdgeInsets.fromLTRB(32, 0, 32, 32),
        header: (context) => _reportHeader(reportTitle, userName, generatedAt),
        footer: (context) => _reportFooter(context),
        build: (context) => [
          for (final entry in sections.entries)
            _section(entry.key, entry.value),
        ],
      ),
    );

    return pdf.save();
  }

  Future<File> saveMotherPdf({
    required String userName,
    required List<DetailItem> therapy,
    required List<DetailItem> symptoms,
    required List<DetailItem> questions,
  }) async {
    final bytes = await generateMotherPdfBytes(
      userName: userName,
      therapy: therapy,
      symptoms: symptoms,
      questions: questions,
    );

    final dir = await getApplicationDocumentsDirectory();

    final file = File(
      '${dir.path}/mother_report_${DateTime.now().millisecondsSinceEpoch}.pdf',
    );

    await file.writeAsBytes(bytes);

    return file;
  }

  Future<File> saveBabyPdf({
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
    final bytes = await generateBabyPdfBytes(
      userName: userName,
      meals: meals,
      health: health,
      diapers: diapers,
      sleep: sleep,
      growth: growth,
      feeding: feeding,
      milestones: milestones,
      calendar: calendar,
    );

    final dir = await getApplicationDocumentsDirectory();

    final file = File(
      '${dir.path}/baby_report_${DateTime.now().millisecondsSinceEpoch}.pdf',
    );

    await file.writeAsBytes(bytes);

    return file;
  }

  Future<void> printMotherPdf({
    required String userName,
    required List<DetailItem> therapy,
    required List<DetailItem> symptoms,
    required List<DetailItem> questions,
  }) async {
    final bytes = await generateMotherPdfBytes(
      userName: userName,
      therapy: therapy,
      symptoms: symptoms,
      questions: questions,
    );

    await Printing.layoutPdf(onLayout: (format) async => bytes);
  }

  Future<void> printBabyPdf({
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
    final bytes = await generateBabyPdfBytes(
      userName: userName,
      meals: meals,
      health: health,
      diapers: diapers,
      sleep: sleep,
      growth: growth,
      feeding: feeding,
      milestones: milestones,
      calendar: calendar,
    );

    await Printing.layoutPdf(onLayout: (format) async => bytes);
  }

  pw.Widget _reportHeader(String reportTitle, String userName, String generatedAt) {
    return pw.Container(
      margin: const pw.EdgeInsets.only(bottom: 18),
      padding: const pw.EdgeInsets.fromLTRB(24, 22, 24, 18),
      decoration: const pw.BoxDecoration(
        color: _PdfPalette.roseDark,
        borderRadius: pw.BorderRadius.only(
          bottomLeft: pw.Radius.circular(14),
          bottomRight: pw.Radius.circular(14),
        ),
      ),
      child: pw.Row(
        mainAxisAlignment: pw.MainAxisAlignment.spaceBetween,
        crossAxisAlignment: pw.CrossAxisAlignment.start,
        children: [
          pw.Column(
            crossAxisAlignment: pw.CrossAxisAlignment.start,
            children: [
              pw.Text(
                'Nestly',
                style: pw.TextStyle(
                  color: PdfColors.white,
                  fontSize: 14,
                  fontWeight: pw.FontWeight.bold,
                  letterSpacing: 1.2,
                ),
              ),
              pw.SizedBox(height: 6),
              pw.Text(
                reportTitle,
                style: pw.TextStyle(
                  color: PdfColors.white,
                  fontSize: 22,
                  fontWeight: pw.FontWeight.bold,
                ),
              ),
              pw.SizedBox(height: 4),
              pw.Text(
                'Korisnica: $userName',
                style: const pw.TextStyle(color: PdfColors.white, fontSize: 11),
              ),
            ],
          ),
          pw.Column(
            crossAxisAlignment: pw.CrossAxisAlignment.end,
            children: [
              pw.Text(
                'Generisano',
                style: pw.TextStyle(
                  color: PdfColors.white.shade(0.15),
                  fontSize: 9,
                ),
              ),
              pw.Text(
                generatedAt,
                style: const pw.TextStyle(color: PdfColors.white, fontSize: 10),
              ),
            ],
          ),
        ],
      ),
    );
  }

  pw.Widget _reportFooter(pw.Context context) {
    return pw.Container(
      margin: const pw.EdgeInsets.only(top: 12),
      padding: const pw.EdgeInsets.only(top: 8),
      decoration: const pw.BoxDecoration(
        border: pw.Border(top: pw.BorderSide(color: _PdfPalette.divider, width: 0.75)),
      ),
      child: pw.Row(
        mainAxisAlignment: pw.MainAxisAlignment.spaceBetween,
        children: [
          pw.Text(
            'Nestly - povjerljiv izvještaj',
            style: const pw.TextStyle(color: _PdfPalette.textSecondary, fontSize: 8),
          ),
          pw.Text(
            'Stranica ${context.pageNumber} od ${context.pagesCount}',
            style: const pw.TextStyle(color: _PdfPalette.textSecondary, fontSize: 8),
          ),
        ],
      ),
    );
  }

  pw.Widget _section(String title, List<DetailItem> items) {
    return pw.Container(
      margin: const pw.EdgeInsets.only(bottom: 18),
      child: pw.Column(
        crossAxisAlignment: pw.CrossAxisAlignment.start,
        children: [
          pw.Row(
            children: [
              pw.Container(
                width: 5,
                height: 16,
                decoration: const pw.BoxDecoration(
                  color: _PdfPalette.seed,
                  borderRadius: pw.BorderRadius.all(pw.Radius.circular(3)),
                ),
              ),
              pw.SizedBox(width: 8),
              pw.Text(
                title,
                style: pw.TextStyle(
                  fontSize: 15,
                  fontWeight: pw.FontWeight.bold,
                  color: _PdfPalette.textPrimary,
                ),
              ),
              pw.SizedBox(width: 8),
              pw.Container(
                padding: const pw.EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                decoration: pw.BoxDecoration(
                  color: _PdfPalette.babyBlue.shade(0.35),
                  borderRadius: const pw.BorderRadius.all(pw.Radius.circular(8)),
                ),
                child: pw.Text(
                  '${items.length}',
                  style: pw.TextStyle(
                    fontSize: 9,
                    fontWeight: pw.FontWeight.bold,
                    color: _PdfPalette.textPrimary,
                  ),
                ),
              ),
            ],
          ),
          pw.SizedBox(height: 10),
          if (items.isEmpty)
            pw.Container(
              padding: const pw.EdgeInsets.all(12),
              decoration: pw.BoxDecoration(
                color: _PdfPalette.bg,
                borderRadius: const pw.BorderRadius.all(pw.Radius.circular(8)),
              ),
              child: pw.Text(
                'Nema podataka',
                style: const pw.TextStyle(color: _PdfPalette.textSecondary, fontSize: 10),
              ),
            )
          else
            pw.Column(
              children: items
                  .map(
                    (e) => pw.Container(
                      margin: const pw.EdgeInsets.only(bottom: 6),
                      padding: const pw.EdgeInsets.all(10),
                      decoration: pw.BoxDecoration(
                        color: _PdfPalette.bg,
                        borderRadius: const pw.BorderRadius.all(pw.Radius.circular(8)),
                        border: const pw.Border(
                          left: pw.BorderSide(color: _PdfPalette.roseDark, width: 2.5),
                        ),
                      ),
                      child: pw.Column(
                        crossAxisAlignment: pw.CrossAxisAlignment.start,
                        children: [
                          pw.Text(
                            e.title,
                            style: pw.TextStyle(
                              fontWeight: pw.FontWeight.bold,
                              fontSize: 11,
                              color: _PdfPalette.textPrimary,
                            ),
                          ),
                          if (e.subtitle.isNotEmpty) ...[
                            pw.SizedBox(height: 2),
                            pw.Text(
                              e.subtitle,
                              style: const pw.TextStyle(
                                fontSize: 10,
                                color: _PdfPalette.textPrimary,
                              ),
                            ),
                          ],
                          if (e.meta.isNotEmpty) ...[
                            pw.SizedBox(height: 2),
                            pw.Text(
                              e.meta,
                              style: const pw.TextStyle(
                                fontSize: 9,
                                color: _PdfPalette.textSecondary,
                              ),
                            ),
                          ],
                        ],
                      ),
                    ),
                  )
                  .toList(),
            ),
        ],
      ),
    );
  }
}
