using Nestly.ML;
using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    /// <summary>
    /// <paramref name="IsDeviation"/> is false when the model predicted the
    /// "Normal" class - in that case <paramref name="Severity"/> should be
    /// ignored and no alert should be raised.
    /// </summary>
    public record MLSeverityPrediction(bool IsDeviation, HealthAlertSeverity Severity, float Confidence);

    /// <summary>
    /// Runs the five trained LightGBM multiclass models (see
    /// Nestly.MLTraining) that classify a feature vector into
    /// Normal/Warning/Critical for each monitored parameter. The evaluators
    /// in <see cref="IBabyHealthMonitoringService"/> compute the feature
    /// vectors (z-scores, relative deviation from WHO/pediatric reference
    /// ranges, consistency over the observation window) and hand them to
    /// this service instead of applying hand-written thresholds directly.
    /// </summary>
    public interface IPediatricMLPredictionService
    {
        MLSeverityPrediction PredictGrowth(GrowthSample sample);
        MLSeverityPrediction PredictFeeding(FeedingSample sample);
        MLSeverityPrediction PredictSleep(SleepSample sample);
        MLSeverityPrediction PredictDiaper(DiaperSample sample);
        MLSeverityPrediction PredictFever(FeverSample sample);
    }
}
