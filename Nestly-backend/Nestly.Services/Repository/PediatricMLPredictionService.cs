using Microsoft.Extensions.ML;
using Nestly.ML;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class PediatricMLPredictionService : IPediatricMLPredictionService
    {
        private readonly PredictionEnginePool<GrowthSample, GrowthPrediction> _growthPool;
        private readonly PredictionEnginePool<FeedingSample, FeedingPrediction> _feedingPool;
        private readonly PredictionEnginePool<SleepSample, SleepPrediction> _sleepPool;
        private readonly PredictionEnginePool<DiaperSample, DiaperPrediction> _diaperPool;
        private readonly PredictionEnginePool<FeverSample, FeverPrediction> _feverPool;

        public PediatricMLPredictionService(
            PredictionEnginePool<GrowthSample, GrowthPrediction> growthPool,
            PredictionEnginePool<FeedingSample, FeedingPrediction> feedingPool,
            PredictionEnginePool<SleepSample, SleepPrediction> sleepPool,
            PredictionEnginePool<DiaperSample, DiaperPrediction> diaperPool,
            PredictionEnginePool<FeverSample, FeverPrediction> feverPool)
        {
            _growthPool = growthPool;
            _feedingPool = feedingPool;
            _sleepPool = sleepPool;
            _diaperPool = diaperPool;
            _feverPool = feverPool;
        }

        public MLSeverityPrediction PredictGrowth(GrowthSample sample)
        {
            var prediction = _growthPool.Predict(modelName: "growth", example: sample);
            return ToSeverity(prediction.PredictedLabel, prediction.Score);
        }

        public MLSeverityPrediction PredictFeeding(FeedingSample sample)
        {
            var prediction = _feedingPool.Predict(modelName: "feeding", example: sample);
            return ToSeverity(prediction.PredictedLabel, prediction.Score);
        }

        public MLSeverityPrediction PredictSleep(SleepSample sample)
        {
            var prediction = _sleepPool.Predict(modelName: "sleep", example: sample);
            return ToSeverity(prediction.PredictedLabel, prediction.Score);
        }

        public MLSeverityPrediction PredictDiaper(DiaperSample sample)
        {
            var prediction = _diaperPool.Predict(modelName: "diaper", example: sample);
            return ToSeverity(prediction.PredictedLabel, prediction.Score);
        }

        public MLSeverityPrediction PredictFever(FeverSample sample)
        {
            var prediction = _feverPool.Predict(modelName: "fever", example: sample);
            return ToSeverity(prediction.PredictedLabel, prediction.Score);
        }

        private static MLSeverityPrediction ToSeverity(string predictedLabel, float[] scores)
        {
            var confidence = scores.Length > 0 ? scores.Max() : 0f;

            return predictedLabel switch
            {
                SeverityLabel.Critical => new MLSeverityPrediction(true, HealthAlertSeverity.Critical, confidence),
                SeverityLabel.Warning => new MLSeverityPrediction(true, HealthAlertSeverity.Warning, confidence),
                _ => new MLSeverityPrediction(false, HealthAlertSeverity.Info, confidence)
            };
        }
    }
}
