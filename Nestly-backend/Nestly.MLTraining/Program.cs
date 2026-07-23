using System.Globalization;
using Microsoft.ML;
using Nestly.ML;

// Generates the synthetic, WHO-standards-grounded training datasets for the
// five deviation-monitoring models (growth/feeding/sleep/diaper/fever),
// trains a LightGBM multiclass classifier for each, evaluates it on a held
// -out split, and writes both the CSV datasets and the trained model .zip
// files that Nestly.Services loads at runtime (see
// Nestly.Services/MachineLearning/PediatricMLPredictionService.cs).
//
// Run from Nestly-backend with: dotnet run --project Nestly.MLTraining

const int SampleCount = 20000;

var baseDir = AppContext.BaseDirectory;
var backendDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
var datasetsDir = Path.Combine(backendDir, "Nestly.MLTraining", "datasets");
var modelsDir = Path.Combine(backendDir, "Nestly.Services", "MachineLearning", "MLModels");

Directory.CreateDirectory(datasetsDir);
Directory.CreateDirectory(modelsDir);

var mlContext = new MLContext(seed: 42);
var report = new List<string>
{
    "# Izvještaj o treniranju ML modela za praćenje odstupanja",
    "",
    $"Generisano: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC, {SampleCount} uzoraka po modelu (80/20 train/test split), LightGBM multiklasni klasifikator.",
    ""
};

// --- Growth ---
var growthSamples = SyntheticPediatricDatasetGenerator.GenerateGrowthSamples(SampleCount).ToList();
WriteCsv(Path.Combine(datasetsDir, "growth.csv"),
    new[] { "AgeMonths", "IsFemale", "WeightZ", "HeightZ", "HeadZ", "WeightZDelta", "Label" },
    growthSamples.Select(s => new object[] { s.AgeMonths, s.IsFemale, s.WeightZ, s.HeightZ, s.HeadZ, s.WeightZDelta, s.Label }));
TrainAndSave(mlContext, growthSamples,
    new[] { nameof(GrowthSample.AgeMonths), nameof(GrowthSample.IsFemale), nameof(GrowthSample.WeightZ), nameof(GrowthSample.HeightZ), nameof(GrowthSample.HeadZ), nameof(GrowthSample.WeightZDelta) },
    Path.Combine(modelsDir, "growth-model.zip"), "Rast (Growth)", report);

// --- Feeding ---
var feedingSamples = SyntheticPediatricDatasetGenerator.GenerateFeedingSamples(SampleCount).ToList();
WriteCsv(Path.Combine(datasetsDir, "feeding.csv"),
    new[] { "AgeMonths", "RelativeDeviation", "FeedsStdDev", "PctDaysOutOfRange", "Label" },
    feedingSamples.Select(s => new object[] { s.AgeMonths, s.RelativeDeviation, s.FeedsStdDev, s.PctDaysOutOfRange, s.Label }));
TrainAndSave(mlContext, feedingSamples,
    new[] { nameof(FeedingSample.AgeMonths), nameof(FeedingSample.RelativeDeviation), nameof(FeedingSample.FeedsStdDev), nameof(FeedingSample.PctDaysOutOfRange) },
    Path.Combine(modelsDir, "feeding-model.zip"), "Hranjenje (Feeding)", report);

// --- Sleep ---
var sleepSamples = SyntheticPediatricDatasetGenerator.GenerateSleepSamples(SampleCount).ToList();
WriteCsv(Path.Combine(datasetsDir, "sleep.csv"),
    new[] { "AgeMonths", "RelativeDeviation", "StdDevMinutes", "PctDaysOutOfRange", "Direction", "Label" },
    sleepSamples.Select(s => new object[] { s.AgeMonths, s.RelativeDeviation, s.StdDevMinutes, s.PctDaysOutOfRange, s.Direction, s.Label }));
TrainAndSave(mlContext, sleepSamples,
    new[] { nameof(SleepSample.AgeMonths), nameof(SleepSample.RelativeDeviation), nameof(SleepSample.StdDevMinutes), nameof(SleepSample.PctDaysOutOfRange), nameof(SleepSample.Direction) },
    Path.Combine(modelsDir, "sleep-model.zip"), "Spavanje (Sleep)", report);

// --- Diaper ---
var diaperSamples = SyntheticPediatricDatasetGenerator.GenerateDiaperSamples(SampleCount).ToList();
WriteCsv(Path.Combine(datasetsDir, "diaper.csv"),
    new[] { "AgeMonths", "AvgWetPerDay", "MinWetGuideline", "PctDaysBelowMin", "Label" },
    diaperSamples.Select(s => new object[] { s.AgeMonths, s.AvgWetPerDay, s.MinWetGuideline, s.PctDaysBelowMin, s.Label }));
TrainAndSave(mlContext, diaperSamples,
    new[] { nameof(DiaperSample.AgeMonths), nameof(DiaperSample.AvgWetPerDay), nameof(DiaperSample.MinWetGuideline), nameof(DiaperSample.PctDaysBelowMin) },
    Path.Combine(modelsDir, "diaper-model.zip"), "Pelene (Diaper)", report);

// --- Fever ---
var feverSamples = SyntheticPediatricDatasetGenerator.GenerateFeverSamples(SampleCount).ToList();
WriteCsv(Path.Combine(datasetsDir, "fever.csv"),
    new[] { "AgeMonths", "MaxTempC", "FeverEntryCount", "IsYoungInfant", "Label" },
    feverSamples.Select(s => new object[] { s.AgeMonths, s.MaxTempC, s.FeverEntryCount, s.IsYoungInfant, s.Label }));
TrainAndSave(mlContext, feverSamples,
    new[] { nameof(FeverSample.AgeMonths), nameof(FeverSample.MaxTempC), nameof(FeverSample.FeverEntryCount), nameof(FeverSample.IsYoungInfant) },
    Path.Combine(modelsDir, "fever-model.zip"), "Temperatura (Fever)", report);

File.WriteAllLines(Path.Combine(backendDir, "..", "ml-training-report.md"), report);
Console.WriteLine();
Console.WriteLine($"Datasets: {datasetsDir}");
Console.WriteLine($"Models:   {modelsDir}");
Console.WriteLine("Report:   ml-training-report.md");

// Post-training sanity check: reload the exact .zip files that get shipped
// to the api/worker containers and run one real prediction through each,
// so a serialization mistake can't silently ship a broken model.
Console.WriteLine();
Console.WriteLine("Verifying saved models load and predict correctly...");

VerifyModel(mlContext, Path.Combine(modelsDir, "growth-model.zip"),
    new GrowthSample { AgeMonths = 6, IsFemale = 0, WeightZ = -3.2f, HeightZ = -0.5f, HeadZ = 0, WeightZDelta = 0 },
    (GrowthPrediction p) => p.PredictedLabel);

VerifyModel(mlContext, Path.Combine(modelsDir, "feeding-model.zip"),
    new FeedingSample { AgeMonths = 3, RelativeDeviation = 1.8f, FeedsStdDev = 0.2f, PctDaysOutOfRange = 1.0f },
    (FeedingPrediction p) => p.PredictedLabel);

VerifyModel(mlContext, Path.Combine(modelsDir, "sleep-model.zip"),
    new SleepSample { AgeMonths = 5, RelativeDeviation = -1.2f, StdDevMinutes = 20, PctDaysOutOfRange = 1.0f, Direction = -1 },
    (SleepPrediction p) => p.PredictedLabel);

VerifyModel(mlContext, Path.Combine(modelsDir, "diaper-model.zip"),
    new DiaperSample { AgeMonths = 4, AvgWetPerDay = 1, MinWetGuideline = 4, PctDaysBelowMin = 1.0f },
    (DiaperPrediction p) => p.PredictedLabel);

VerifyModel(mlContext, Path.Combine(modelsDir, "fever-model.zip"),
    new FeverSample { AgeMonths = 1, MaxTempC = 39.5f, FeverEntryCount = 2, IsYoungInfant = 1 },
    (FeverPrediction p) => p.PredictedLabel);

static void VerifyModel<TSample, TPrediction>(
    MLContext mlContext, string modelPath, TSample clearlyAbnormalSample, Func<TPrediction, string> getLabel)
    where TSample : class
    where TPrediction : class, new()
{
    var loadedModel = mlContext.Model.Load(modelPath, out _);
    var engine = mlContext.Model.CreatePredictionEngine<TSample, TPrediction>(loadedModel);
    var prediction = engine.Predict(clearlyAbnormalSample);
    var label = getLabel(prediction);

    Console.WriteLine($"  {Path.GetFileName(modelPath)}: reloaded OK, predicted '{label}' for a clearly-abnormal sample");
}

static void TrainAndSave<T>(
    MLContext mlContext,
    List<T> samples,
    string[] featureColumns,
    string modelPath,
    string reportName,
    List<string> report) where T : class
{
    var fullData = mlContext.Data.LoadFromEnumerable(samples);
    var split = mlContext.Data.TrainTestSplit(fullData, testFraction: 0.2, seed: 42);

    var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
        .Append(mlContext.Transforms.Concatenate("Features", featureColumns))
        .Append(mlContext.Transforms.NormalizeMinMax("Features"))
        .Append(mlContext.MulticlassClassification.Trainers.LightGbm(labelColumnName: "Label", featureColumnName: "Features"))
        .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

    var model = pipeline.Fit(split.TrainSet);
    var predictions = model.Transform(split.TestSet);
    var metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "Label");

    Console.WriteLine(
        $"{reportName}: macro-acc={metrics.MacroAccuracy:P2} micro-acc={metrics.MicroAccuracy:P2} log-loss={metrics.LogLoss:F4}");

    report.Add($"## {reportName}");
    report.Add($"- Macro accuracy: {metrics.MacroAccuracy:P2}");
    report.Add($"- Micro accuracy: {metrics.MicroAccuracy:P2}");
    report.Add($"- Log-loss: {metrics.LogLoss:F4}");
    report.Add($"- Features: {string.Join(", ", featureColumns)}");
    report.Add("");

    mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
}

static void WriteCsv(string path, string[] header, IEnumerable<object[]> rows)
{
    using var writer = new StreamWriter(path);
    writer.WriteLine(string.Join(",", header));

    foreach (var row in rows)
    {
        writer.WriteLine(string.Join(",", row.Select(FormatCsvValue)));
    }
}

static string FormatCsvValue(object value)
{
    return value switch
    {
        float f => f.ToString("G6", CultureInfo.InvariantCulture),
        double d => d.ToString("G6", CultureInfo.InvariantCulture),
        _ => value.ToString() ?? string.Empty
    };
}
