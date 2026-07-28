using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Nestly.ML;
using Nestly.Services.Interfaces;
using Nestly.Services.Repository;

namespace Nestly.Services.MachineLearning
{
    public static class MLServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the five trained pediatric-deviation models (see
        /// Nestly.MLTraining) as ML.NET PredictionEnginePools and the
        /// service that wraps them. Used by both the WebAPI (on-demand
        /// checks) and the Worker (daily background job).
        /// </summary>
        public static IServiceCollection AddPediatricMLModels(this IServiceCollection services)
        {
            var modelsDir = Path.Combine(AppContext.BaseDirectory, "MachineLearning", "MLModels");

            services.AddPredictionEnginePool<GrowthSample, GrowthPrediction>()
                .FromFile(modelName: "growth", filePath: Path.Combine(modelsDir, "growth-model.zip"), watchForChanges: false);

            services.AddPredictionEnginePool<FeedingSample, FeedingPrediction>()
                .FromFile(modelName: "feeding", filePath: Path.Combine(modelsDir, "feeding-model.zip"), watchForChanges: false);

            services.AddPredictionEnginePool<SleepSample, SleepPrediction>()
                .FromFile(modelName: "sleep", filePath: Path.Combine(modelsDir, "sleep-model.zip"), watchForChanges: false);

            services.AddPredictionEnginePool<DiaperSample, DiaperPrediction>()
                .FromFile(modelName: "diaper", filePath: Path.Combine(modelsDir, "diaper-model.zip"), watchForChanges: false);

            services.AddPredictionEnginePool<FeverSample, FeverPrediction>()
                .FromFile(modelName: "fever", filePath: Path.Combine(modelsDir, "fever-model.zip"), watchForChanges: false);

            services.AddScoped<IPediatricMLPredictionService, PediatricMLPredictionService>();

            return services;
        }

        /// <summary>
        /// Registers the Q&amp;A urgency-triage text classifier (see
        /// Nestly.MLTraining) used by the doctor-facing question inbox.
        /// </summary>
        public static IServiceCollection AddQaTriageModel(this IServiceCollection services)
        {
            var modelsDir = Path.Combine(AppContext.BaseDirectory, "MachineLearning", "MLModels");

            services.AddPredictionEnginePool<QuestionUrgencySample, QuestionUrgencyPrediction>()
                .FromFile(modelName: "qa-urgency", filePath: Path.Combine(modelsDir, "qa-urgency-model.zip"), watchForChanges: false);

            services.AddScoped<IQaTriageService, QaTriageService>();

            return services;
        }
    }
}
