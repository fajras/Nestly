using Microsoft.ML.Data;

namespace Nestly.ML
{
    public class QuestionUrgencySample
    {
        [LoadColumn(0)] public string QuestionText { get; set; } = default!;
        [LoadColumn(1)] public bool Label { get; set; }
    }

    public class QuestionUrgencyPrediction
    {
        [ColumnName("PredictedLabel")] public bool IsUrgent { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
