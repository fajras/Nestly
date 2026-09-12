using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nestly.Model.Entity;
using Nestly.Services.Data;

namespace Nestly.Worker.Messaging
{
    // Periodically decides whether the deviation-detection models are due
    // for retraining, either because enough new daily-record data has
    // accumulated since the last run, or because enough time has passed
    // regardless of volume - "periodično ponovno treniranje ... sa rastom
    // broja korisnika i količine prikupljenih podataka".
    //
    // Disabled by default (MlTraining:Enabled=false): the training pipeline
    // is still generator-based synthetic data rather than real anonymized
    // user data (see Nestly.MLTraining), so auto-triggering it on a fresh
    // deployment would just repeatedly retrain on the same kind of
    // synthetic distribution. The mechanism is wired up and ready to
    // enable - and to point at a real-data pipeline - once that data
    // becomes available.
    public class ModelRetrainingSchedulerService : BackgroundService
    {
        private const string ModelGroup = "PediatricDeviation";
        private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(6);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<ModelRetrainingSchedulerService> _logger;

        public ModelRetrainingSchedulerService(
            IServiceScopeFactory scopeFactory,
            IConfiguration config,
            ILogger<ModelRetrainingSchedulerService> logger)
        {
            _scopeFactory = scopeFactory;
            _config = config;
            _logger = logger;
        }

        private bool Enabled =>
            bool.TryParse(_config["MlTraining:Enabled"], out var enabled) && enabled;

        private long GrowthThreshold =>
            long.TryParse(_config["MlTraining:GrowthThreshold"], out var t) && t > 0 ? t : 10000;

        private int MinIntervalDays =>
            int.TryParse(_config["MlTraining:MinIntervalDays"], out var d) && d > 0 ? d : 30;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (Enabled)
                    {
                        await CheckAndRetrainIfDue(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ModelRetrainingSchedulerService check failed.");
                }

                try
                {
                    await Task.Delay(CheckInterval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        private async Task CheckAndRetrainIfDue(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();
            var trigger = scope.ServiceProvider.GetRequiredService<IMlTrainingTrigger>();

            var watermark = await db.MlRetrainingWatermarks
                .FirstOrDefaultAsync(w => w.ModelGroup == ModelGroup, ct);

            var currentCount = await CountTrainingRelevantRecords(db, ct);

            if (watermark == null)
            {
                // First run ever: establish a baseline instead of
                // immediately kicking off a multi-minute training job the
                // moment the worker starts.
                db.MlRetrainingWatermarks.Add(new MlRetrainingWatermark
                {
                    ModelGroup = ModelGroup,
                    LastTrainedAt = DateTime.UtcNow,
                    RecordCountAtLastTraining = currentCount
                });

                await db.SaveChangesAsync(ct);
                return;
            }

            var newRecords = currentCount - watermark.RecordCountAtLastTraining;
            var daysSinceLastTrained = (DateTime.UtcNow - watermark.LastTrainedAt).TotalDays;

            var due = newRecords >= GrowthThreshold || daysSinceLastTrained >= MinIntervalDays;

            if (!due)
            {
                return;
            }

            _logger.LogInformation(
                "Retraining due for {ModelGroup}: {NewRecords} new records (threshold {Threshold}), {Days:F1} days since last run (min interval {MinDays}).",
                ModelGroup, newRecords, GrowthThreshold, daysSinceLastTrained, MinIntervalDays);

            var success = await trigger.TriggerAsync(ct);

            if (success)
            {
                watermark.LastTrainedAt = DateTime.UtcNow;
                watermark.RecordCountAtLastTraining = currentCount;
                await db.SaveChangesAsync(ct);
            }
        }

        // Proxy for "količina prikupljenih podataka": total rows across the
        // daily-record tables the deviation models are trained to reason
        // about (growth, feeding, sleep, diapers, fever/health entries).
        private static async Task<long> CountTrainingRelevantRecords(NestlyDbContext db, CancellationToken ct)
        {
            var babyGrowth = await db.BabyGrowths.LongCountAsync(ct);
            var feeding = await db.FeedingLogs.LongCountAsync(ct);
            var sleep = await db.SleepLogs.LongCountAsync(ct);
            var diaper = await db.DiaperLogs.LongCountAsync(ct);
            var health = await db.HealthEntries.LongCountAsync(ct);

            return babyGrowth + feeding + sleep + diaper + health;
        }
    }
}
