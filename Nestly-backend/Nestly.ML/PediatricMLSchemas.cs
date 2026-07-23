using Microsoft.ML.Data;

namespace Nestly.ML
{
    // Multiclass severity label shared by all five models.
    public static class SeverityLabel
    {
        public const string Normal = "Normal";
        public const string Warning = "Warning";
        public const string Critical = "Critical";
    }

    public class GrowthSample
    {
        [LoadColumn(0)] public float AgeMonths { get; set; }
        [LoadColumn(1)] public float IsFemale { get; set; }
        [LoadColumn(2)] public float WeightZ { get; set; }
        [LoadColumn(3)] public float HeightZ { get; set; }
        [LoadColumn(4)] public float HeadZ { get; set; }
        [LoadColumn(5)] public float WeightZDelta { get; set; }
        [LoadColumn(6)] public string Label { get; set; } = default!;
    }

    public class GrowthPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; } = default!;
        public float[] Score { get; set; } = Array.Empty<float>();
    }

    public class FeedingSample
    {
        [LoadColumn(0)] public float AgeMonths { get; set; }
        [LoadColumn(1)] public float RelativeDeviation { get; set; }
        [LoadColumn(2)] public float FeedsStdDev { get; set; }
        [LoadColumn(3)] public float PctDaysOutOfRange { get; set; }
        [LoadColumn(4)] public string Label { get; set; } = default!;
    }

    public class FeedingPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; } = default!;
        public float[] Score { get; set; } = Array.Empty<float>();
    }

    public class SleepSample
    {
        [LoadColumn(0)] public float AgeMonths { get; set; }
        [LoadColumn(1)] public float RelativeDeviation { get; set; }
        [LoadColumn(2)] public float StdDevMinutes { get; set; }
        [LoadColumn(3)] public float PctDaysOutOfRange { get; set; }
        [LoadColumn(4)] public float Direction { get; set; } // -1 = sleeping less than range, +1 = more
        [LoadColumn(5)] public string Label { get; set; } = default!;
    }

    public class SleepPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; } = default!;
        public float[] Score { get; set; } = Array.Empty<float>();
    }

    public class DiaperSample
    {
        [LoadColumn(0)] public float AgeMonths { get; set; }
        [LoadColumn(1)] public float AvgWetPerDay { get; set; }
        [LoadColumn(2)] public float MinWetGuideline { get; set; }
        [LoadColumn(3)] public float PctDaysBelowMin { get; set; }
        [LoadColumn(4)] public string Label { get; set; } = default!;
    }

    public class DiaperPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; } = default!;
        public float[] Score { get; set; } = Array.Empty<float>();
    }

    public class FeverSample
    {
        [LoadColumn(0)] public float AgeMonths { get; set; }
        [LoadColumn(1)] public float MaxTempC { get; set; }
        [LoadColumn(2)] public float FeverEntryCount { get; set; }
        [LoadColumn(3)] public float IsYoungInfant { get; set; }
        [LoadColumn(4)] public string Label { get; set; } = default!;
    }

    public class FeverPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; } = default!;
        public float[] Score { get; set; } = Array.Empty<float>();
    }
}
