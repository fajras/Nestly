import 'dart:io';

import 'package:path_provider/path_provider.dart';
import 'package:pdf/pdf.dart';
import 'package:pdf/widgets.dart' as pw;
import 'package:printing/printing.dart';

import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class _PdfPalette {
  static const roseDark = PdfColor.fromInt(0xFFA82859);
  static const babyBlue = PdfColor.fromInt(0xFFA2D2FF);
  static const seed = PdfColor.fromInt(0xFF00A6A6);
  static const amber = PdfColor.fromInt(0xFFE8A33D);
  static const bg = PdfColor.fromInt(0xFFFFFBEA);
  static const textPrimary = PdfColor.fromInt(0xFF0F172A);
  static const textSecondary = PdfColor.fromInt(0xFF475569);
  static const divider = PdfColor.fromInt(0xFFE8E1D5);
}

/// A single (x-label, y-value) sample for a [ReportChart] series.
class ChartPoint {
  final String label;
  final double value;
  const ChartPoint(this.label, this.value);
}

class ReportChartSeries {
  final String legend;
  final PdfColor? color;
  final List<ChartPoint> points;
  const ReportChartSeries({
    required this.legend,
    required this.points,
    this.color,
  });
}

enum ReportChartType { line, bar, stackedBar }

/// A trend chart for one measurable parameter (e.g. weight over time). Every
/// series in [series] is expected to share the same x-axis labels/ordering.
class ReportChart {
  final String title;
  final String? unit;
  final List<ReportChartSeries> series;
  final ReportChartType type;

  /// Overrides the auto-computed Y range - use for a chart with a known
  /// fixed scale (e.g. a 0-5 symptom severity rating) instead of padding
  /// around whatever the data happens to contain.
  final double? yMin;
  final double? yMax;

  /// Formats Y-axis ticks as whole numbers - for counts and fixed integer
  /// scales, where a decimal (from dividing the range into steps) would be
  /// misleading.
  final bool integerTicks;

  /// Snaps Y-axis ticks to a round multiple (e.g. 100 for a "ml/g" scale)
  /// instead of whatever the auto-computed step happens to divide out to.
  final double? tickStep;

  const ReportChart({
    required this.title,
    required this.series,
    this.unit,
    this.type = ReportChartType.line,
    this.yMin,
    this.yMax,
    this.integerTicks = false,
    this.tickStep,
  });
}

/// A compact data table for a non-chartable (or supplementary) category.
class ReportTable {
  final String title;
  final List<String> headers;
  final List<List<String>> rows;
  const ReportTable({
    required this.title,
    required this.headers,
    required this.rows,
  });
}

class AdminPdfService {
  static const _seriesColors = [
    _PdfPalette.seed,
    _PdfPalette.roseDark,
    _PdfPalette.amber,
    PdfColors.purple,
    PdfColors.brown,
  ];

  // A table at or under this many rows is short enough to always fit on one
  // page, so its title is bundled with it into one non-spanning widget
  // instead of risking the two ending up split across a page break. Row
  // count alone isn't a safe proxy for "fits on one page" though - a table
  // with long text cells (e.g. a day's worth of feedings joined into one
  // cell) wraps to multiple lines per row and can blow past a page even at
  // a handful of rows, which is exactly the TooManyPagesException this was
  // meant to avoid in the first place. So a table only gets bundled when
  // it's short AND every cell is short enough to render on one line.
  static const _bundleRowThreshold = 25;
  static const _bundleCellCharThreshold = 45;

  bool _canBundleTable(ReportTable table) {
    if (table.rows.length > _bundleRowThreshold) return false;
    for (final row in table.rows) {
      for (final cell in row) {
        if (cell.length > _bundleCellCharThreshold) return false;
      }
    }
    return true;
  }

  Future<Uint8List> generateReportBytes({
    required String reportTitle,
    required String userName,
    List<ReportChart> charts = const [],
    List<ReportTable> tables = const [],
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
        // Charts are single bounded-height widgets (safe as a top-level
        // MultiPage item, and title+chart are one Column so they can never
        // be split across a page break). Tables use TableHelper.fromTextArray,
        // a SpanningWidget that paginates itself across pages when too long.
        // A short table's title+table are bundled into one non-spanning
        // Column instead (same "always fits together" guarantee as charts) -
        // a title can only end up alone on a page if it's a separate
        // top-level item from what follows it, so a genuinely large table
        // (which does need to span pages on its own) keeps its title
        // separate; that's the only case where an orphan is still possible.
        build: (context) => [
          if (charts.isNotEmpty) ...[
            _groupLabel('Pregled kroz vrijeme'),
            pw.SizedBox(height: 10),
            for (final chart in charts) ...[
              _chartBlock(chart),
              pw.SizedBox(height: 14),
            ],
          ],
          if (tables.isNotEmpty) ...[
            _groupLabel('Detaljni podaci'),
            pw.SizedBox(height: 10),
          ],
          for (final table in tables) ...[
            if (_canBundleTable(table))
              pw.Column(
                crossAxisAlignment: pw.CrossAxisAlignment.start,
                children: [
                  _tableTitle(table.title, table.rows.length),
                  pw.SizedBox(height: 6),
                  table.rows.isEmpty ? _sectionEmpty() : _dataTable(table),
                ],
              )
            else ...[
              _tableTitle(table.title, table.rows.length),
              pw.SizedBox(height: 6),
              _dataTable(table),
            ],
            pw.SizedBox(height: 14),
          ],
        ],
      ),
    );

    return pdf.save();
  }

  Future<File> saveReportPdf({
    required String reportTitle,
    required String userName,
    required String fileNamePrefix,
    List<ReportChart> charts = const [],
    List<ReportTable> tables = const [],
  }) async {
    final bytes = await generateReportBytes(
      reportTitle: reportTitle,
      userName: userName,
      charts: charts,
      tables: tables,
    );

    final dir = await getApplicationDocumentsDirectory();

    final file = File(
      '${dir.path}/${fileNamePrefix}_${DateTime.now().millisecondsSinceEpoch}.pdf',
    );

    await file.writeAsBytes(bytes);

    return file;
  }

  Future<void> printReportPdf({
    required String reportTitle,
    required String userName,
    List<ReportChart> charts = const [],
    List<ReportTable> tables = const [],
  }) async {
    final bytes = await generateReportBytes(
      reportTitle: reportTitle,
      userName: userName,
      charts: charts,
      tables: tables,
    );

    await Printing.layoutPdf(onLayout: (format) async => bytes);
  }

  pw.Widget _reportHeader(
    String reportTitle,
    String userName,
    String generatedAt,
  ) {
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
        border: pw.Border(
          top: pw.BorderSide(color: _PdfPalette.divider, width: 0.75),
        ),
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

  pw.Widget _groupLabel(String text) {
    return pw.Text(
      text.toUpperCase(),
      style: pw.TextStyle(
        fontSize: 10,
        fontWeight: pw.FontWeight.bold,
        color: _PdfPalette.textSecondary,
        letterSpacing: 1.1,
      ),
    );
  }

  pw.Widget _tableTitle(String title, int count) {
    return pw.Row(
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
            fontSize: 13,
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
            '$count',
            style: pw.TextStyle(
              fontSize: 9,
              fontWeight: pw.FontWeight.bold,
              color: _PdfPalette.textPrimary,
            ),
          ),
        ),
      ],
    );
  }

  pw.Widget _sectionEmpty() {
    return pw.Container(
      width: double.infinity,
      padding: const pw.EdgeInsets.all(12),
      decoration: pw.BoxDecoration(
        color: _PdfPalette.bg,
        borderRadius: const pw.BorderRadius.all(pw.Radius.circular(8)),
      ),
      child: pw.Text(
        'Nema podataka',
        style: const pw.TextStyle(color: _PdfPalette.textSecondary, fontSize: 10),
      ),
    );
  }

  pw.Widget _dataTable(ReportTable table) {
    return pw.TableHelper.fromTextArray(
      headers: table.headers,
      data: table.rows,
      border: const pw.TableBorder(
        top: pw.BorderSide(color: _PdfPalette.divider),
        bottom: pw.BorderSide(color: _PdfPalette.divider),
        horizontalInside: pw.BorderSide(color: _PdfPalette.divider, width: 0.5),
      ),
      headerDecoration: const pw.BoxDecoration(color: _PdfPalette.roseDark),
      headerStyle: pw.TextStyle(
        fontSize: 9,
        fontWeight: pw.FontWeight.bold,
        color: PdfColors.white,
      ),
      headerPadding: const pw.EdgeInsets.symmetric(horizontal: 6, vertical: 6),
      // Header defaults to center while data cells are left-aligned, which
      // makes every column look shifted relative to its own title - keep
      // both aligned the same way so a column's values sit under its name.
      headerAlignment: pw.Alignment.centerLeft,
      cellStyle: const pw.TextStyle(fontSize: 8.5, color: _PdfPalette.textPrimary),
      cellPadding: const pw.EdgeInsets.symmetric(horizontal: 6, vertical: 5),
      cellAlignment: pw.Alignment.centerLeft,
      oddRowDecoration: const pw.BoxDecoration(color: _PdfPalette.bg),
    );
  }

  /// Renders a chart's title/legend header plus the chart itself, or a
  /// "not enough data" placeholder if there aren't enough points to plot.
  pw.Widget _chartBlock(ReportChart chart) {
    final hasData = chart.series.any((s) => s.points.isNotEmpty);

    return pw.Column(
      crossAxisAlignment: pw.CrossAxisAlignment.start,
      children: [
        pw.Row(
          children: [
            pw.Text(
              chart.title,
              style: pw.TextStyle(
                fontSize: 12,
                fontWeight: pw.FontWeight.bold,
                color: _PdfPalette.textPrimary,
              ),
            ),
            if (chart.unit != null) ...[
              pw.SizedBox(width: 4),
              pw.Text(
                '(${chart.unit})',
                style: const pw.TextStyle(fontSize: 9, color: _PdfPalette.textSecondary),
              ),
            ],
          ],
        ),
        pw.SizedBox(height: 6),
        if (!hasData)
          _sectionEmpty()
        else ...[
          _chartWidget(chart),
          if (chart.series.length > 1) ...[
            pw.SizedBox(height: 6),
            pw.Wrap(
              spacing: 12,
              runSpacing: 4,
              children: [
                for (var i = 0; i < chart.series.length; i++)
                  pw.Row(
                    mainAxisSize: pw.MainAxisSize.min,
                    children: [
                      pw.Container(
                        width: 8,
                        height: 8,
                        decoration: pw.BoxDecoration(
                          color: chart.series[i].color ??
                              _seriesColors[i % _seriesColors.length],
                          shape: pw.BoxShape.circle,
                        ),
                      ),
                      pw.SizedBox(width: 4),
                      pw.Text(
                        chart.series[i].legend,
                        style: const pw.TextStyle(
                          fontSize: 8,
                          color: _PdfPalette.textSecondary,
                        ),
                      ),
                    ],
                  ),
              ],
            ),
          ],
        ],
      ],
    );
  }

  pw.Widget _chartWidget(ReportChart chart) {
    final labels = chart.series
        .firstWhere((s) => s.points.isNotEmpty)
        .points
        .map((p) => p.label)
        .toList();

    double yMin;
    double yMax;
    if (chart.yMin != null && chart.yMax != null) {
      yMin = chart.yMin!;
      yMax = chart.yMax!;
    } else if (chart.type == ReportChartType.stackedBar) {
      // A stacked bar's height is the sum of every series at that point, not
      // any single series' own max - using the per-series max here would
      // clip the top of the stack off the chart.
      final pointCount = chart.series.fold<int>(
        0,
        (m, s) => s.points.length > m ? s.points.length : m,
      );
      var maxTotal = 0.0;
      for (var j = 0; j < pointCount; j++) {
        var total = 0.0;
        for (final s in chart.series) {
          if (j < s.points.length) total += s.points[j].value;
        }
        if (total > maxTotal) maxTotal = total;
      }
      yMin = 0;
      yMax = chart.yMax ?? (maxTotal <= 0 ? 1 : maxTotal * 1.15);
    } else {
      final values = chart.series.expand((s) => s.points.map((p) => p.value));
      yMin = chart.yMin ?? values.reduce((a, b) => a < b ? a : b);
      yMax = chart.yMax ?? values.reduce((a, b) => a > b ? a : b);
      if (yMin == yMax) {
        yMin -= 1;
        yMax += 1;
      }
      if (chart.yMax == null) yMax += (yMax - yMin) * 0.15;
      // A bar's baseline reads 0, not "padded just below the shortest bar" -
      // padding the bottom here left a sliver of empty axis (e.g. "-6")
      // below every bar instead of them sitting flush on the zero line.
      if (chart.type == ReportChartType.bar) {
        yMin = 0;
      } else if (chart.yMin == null) {
        yMin -= (yMax - yMin) * 0.15;
      }
    }

    // Long/many date labels overlap horizontally at this width - angling
    // them gives each one its own diagonal lane instead of colliding.
    final angled = labels.length > 4;

    final xAxis = pw.FixedAxis.fromStrings(
      labels,
      marginStart: 24,
      marginEnd: 24,
      angle: angled ? -0.6 : 0,
      textStyle: const pw.TextStyle(fontSize: 7, color: _PdfPalette.textSecondary),
    );

    final yAxis = pw.FixedAxis<double>(
      chart.tickStep != null
          ? _steppedTicks(yMin, yMax, chart.tickStep!)
          : chart.integerTicks
              ? _integerTicks(yMin, yMax)
              : _yTicks(yMin, yMax),
      divisions: true,
      divisionsColor: _PdfPalette.divider,
      // Ticks are plain doubles from dividing a float range, which prints
      // ugly binary-float noise (e.g. 3.6499999999999995) without an
      // explicit format rounding it for display.
      format: (v) => chart.integerTicks
          ? v.round().toString()
          : v.toStringAsFixed((yMax - yMin) < 20 ? 1 : 0),
      textStyle: const pw.TextStyle(fontSize: 6.5, color: _PdfPalette.textSecondary),
    );

    return pw.Container(
      height: angled ? 175 : 150,
      width: double.infinity,
      padding: const pw.EdgeInsets.only(top: 6, right: 10),
      child: pw.Chart(
        grid: pw.CartesianGrid(xAxis: xAxis, yAxis: yAxis),
        datasets: chart.type == ReportChartType.bar
            ? _barDatasets(chart)
            : chart.type == ReportChartType.stackedBar
                ? _stackedBarDatasets(chart)
                : _lineDatasets(chart),
      ),
    );
  }

  List<pw.Dataset> _lineDatasets(ReportChart chart) {
    return [
      for (var i = 0; i < chart.series.length; i++)
        if (chart.series[i].points.isNotEmpty)
          pw.LineDataSet(
            legend: chart.series[i].legend,
            color: chart.series[i].color ?? _seriesColors[i % _seriesColors.length],
            isCurved: true,
            pointSize: 2.2,
            lineWidth: 1.6,
            drawSurface: chart.series.length == 1,
            surfaceOpacity: 0.15,
            data: [
              for (var j = 0; j < chart.series[i].points.length; j++)
                pw.PointChartValue(j.toDouble(), chart.series[i].points[j].value),
            ],
          ),
    ];
  }

  /// Renders each series as its own bar per category, offset sideways so
  /// same-day bars sit side by side instead of overlapping - the pdf
  /// package has no native "grouped bar" widget, so the grouping is done
  /// by hand via each BarDataSet's `width`/`offset`.
  List<pw.Dataset> _barDatasets(ReportChart chart) {
    final active = [
      for (var i = 0; i < chart.series.length; i++)
        if (chart.series[i].points.isNotEmpty) i,
    ];
    final count = active.length;
    const groupWidth = 20.0;
    final barWidth = groupWidth / (count == 0 ? 1 : count);

    return [
      for (var k = 0; k < active.length; k++)
        pw.BarDataSet(
          legend: chart.series[active[k]].legend,
          color: chart.series[active[k]].color ??
              _seriesColors[active[k] % _seriesColors.length],
          width: barWidth - 1.5,
          offset: (k - (count - 1) / 2) * barWidth,
          data: [
            for (var j = 0; j < chart.series[active[k]].points.length; j++)
              pw.PointChartValue(
                j.toDouble(),
                chart.series[active[k]].points[j].value,
              ),
          ],
        ),
    ];
  }

  /// Draws a genuinely stacked bar: same x-position bars painted largest
  /// (full running total) first and smallest (first series alone) last, each
  /// in its own series' color. Since a bar always paints solid from the
  /// baseline up, each later (shorter) bar overpaints the lower portion of
  /// the previous one, leaving a band of the earlier color visible above it
  /// - the net effect is a proper stacked segment per series, using only
  /// this package's plain "solid bar from 0" primitive.
  List<pw.Dataset> _stackedBarDatasets(ReportChart chart) {
    final active = [
      for (var i = 0; i < chart.series.length; i++)
        if (chart.series[i].points.isNotEmpty) i,
    ];
    if (active.isEmpty) return [];

    final pointCount = active
        .map((i) => chart.series[i].points.length)
        .reduce((a, b) => a > b ? a : b);

    // cumulative[k][j] = sum of series[active[0..k]] at point j
    final cumulative = <List<double>>[];
    for (var k = 0; k < active.length; k++) {
      final row = <double>[];
      for (var j = 0; j < pointCount; j++) {
        var sum = 0.0;
        for (var m = 0; m <= k; m++) {
          final pts = chart.series[active[m]].points;
          if (j < pts.length) sum += pts[j].value;
        }
        row.add(sum);
      }
      cumulative.add(row);
    }

    final datasets = <pw.Dataset>[];
    for (var k = active.length - 1; k >= 0; k--) {
      datasets.add(
        pw.BarDataSet(
          legend: chart.series[active[k]].legend,
          color: chart.series[active[k]].color ??
              _seriesColors[active[k] % _seriesColors.length],
          width: 16,
          offset: 0,
          data: [
            for (var j = 0; j < pointCount; j++)
              pw.PointChartValue(j.toDouble(), cumulative[k][j]),
          ],
        ),
      );
    }
    return datasets;
  }

  List<double> _yTicks(double min, double max) {
    const steps = 4;
    final step = (max - min) / steps;
    return [for (var i = 0; i <= steps; i++) min + step * i];
  }

  List<double> _integerTicks(double min, double max) {
    final lo = min.floor();
    final hi = max.ceil();
    final range = hi - lo;
    if (range <= 0) return [lo.toDouble(), lo.toDouble() + 1];
    const maxSteps = 5;
    final step = range <= maxSteps ? 1 : (range / maxSteps).ceil();
    return [for (var v = lo; v <= hi; v += step) v.toDouble()];
  }

  List<double> _steppedTicks(double min, double max, double step) {
    final lo = (min / step).floor() * step;
    final hi = (max / step).ceil() * step;
    final ticks = <double>[];
    for (var v = lo; v <= hi + step * 0.001; v += step) {
      ticks.add(v);
    }
    return ticks.isEmpty ? [0, step] : ticks;
  }
}
