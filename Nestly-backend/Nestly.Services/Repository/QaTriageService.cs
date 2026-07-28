using Microsoft.Extensions.ML;
using Nestly.ML;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class QaTriageService : IQaTriageService
    {
        private readonly PredictionEnginePool<QuestionUrgencySample, QuestionUrgencyPrediction> _pool;

        public QaTriageService(PredictionEnginePool<QuestionUrgencySample, QuestionUrgencyPrediction> pool)
        {
            _pool = pool;
        }

        public QaUrgencyPrediction PredictUrgency(string questionText)
        {
            var prediction = _pool.Predict(modelName: "qa-urgency", example: new QuestionUrgencySample
            {
                QuestionText = questionText ?? string.Empty
            });

            return new QaUrgencyPrediction(prediction.IsUrgent, prediction.Probability);
        }
    }
}
