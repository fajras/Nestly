using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nestly.ML;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Model.WhoStandards;
using Nestly.Services.Data;
using Nestly.Services.Extensions;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.Services.Repository
{
    // Deviation detection is a two-stage pipeline:
    //  1) feature engineering - aggregate the baby's recent logs and express
    //     them relative to WHO/pediatric reference ranges (z-scores,
    //     relative deviation from the guideline range, consistency across
    //     the observation window) via Nestly.Model.WhoStandards.
    //  2) classification - the feature vector is handed to a LightGBM
    //     multiclass model (trained in Nestly.MLTraining on a WHO-grounded
    //     synthetic dataset, see who-standards-dataset.md) which predicts
    //     Normal/Warning/Critical. The model, not a hand-written threshold,
    //     makes the actual severity decision.
    public class BabyHealthMonitoringService : IBabyHealthMonitoringService
    {
        private const int MaxTrackedAgeMonths = 24;

        private readonly NestlyDbContext _db;
        private readonly RabbitMqPublisher _publisher;
        private readonly IPediatricMLPredictionService _ml;
        private readonly ILogger<BabyHealthMonitoringService> _logger;

        public BabyHealthMonitoringService(
            NestlyDbContext db,
            RabbitMqPublisher publisher,
            IPediatricMLPredictionService ml,
            ILogger<BabyHealthMonitoringService> logger)
        {
            _db = db;
            _publisher = publisher;
            _ml = ml;
            _logger = logger;
        }

        public async Task RunDailyCheckAsync()
        {
            var babyIds = await _db.BabyProfiles
                .Where(b => b.BirthDate <= DateTime.UtcNow)
                .Select(b => b.Id)
                .ToListAsync();

            foreach (var babyId in babyIds)
            {
                try
                {
                    await RunCheckForBabyAsync(babyId);
                }
                catch (Exception ex)
                {
                    // One baby's bad/missing data must not stop the whole daily batch.
                    _logger.LogError(ex, "Health monitoring check failed for baby {BabyId}.", babyId);
                }
            }
        }

        public async Task<List<HealthDeviationAlertResponseDto>> RunCheckForBabyAsync(long babyId)
        {
            var baby = await _db.BabyProfiles
                .Include(b => b.ParentProfile)
                .FirstOrDefaultAsync(b => b.Id == babyId);

            if (baby is null)
            {
                return new List<HealthDeviationAlertResponseDto>();
            }

            var ageMonths = ParentStatusCalculator.CalculateBabyAgeInMonths(baby.BirthDate);

            if (ageMonths > MaxTrackedAgeMonths)
            {
                return new List<HealthDeviationAlertResponseDto>();
            }

            var results = new List<ParameterDeviationResult>();

            AddIfPresent(results, await EvaluateGrowthAsync(baby, ageMonths));
            AddIfPresent(results, await EvaluateFeedingAsync(baby, ageMonths));
            AddIfPresent(results, await EvaluateSleepAsync(baby, ageMonths));
            AddIfPresent(results, await EvaluateDiapersAsync(baby, ageMonths));
            AddIfPresent(results, await EvaluateFeverAsync(baby, ageMonths));

            var created = new List<HealthDeviationAlertResponseDto>();

            foreach (var result in results)
            {
                var alreadyActive = await _db.HealthDeviationAlerts.AnyAsync(a =>
                    a.BabyId == babyId &&
                    a.ParameterType == result.ParameterType &&
                    !a.IsResolved);

                if (alreadyActive)
                {
                    continue;
                }

                var entity = new HealthDeviationAlert
                {
                    BabyId = babyId,
                    ParameterType = result.ParameterType,
                    Severity = result.Severity,
                    Title = result.Title,
                    Message = result.Message,
                    Recommendation = result.Recommendation,
                    DetectedAt = DateTime.UtcNow,
                    PeriodFrom = result.PeriodFrom,
                    PeriodTo = result.PeriodTo,
                    IsResolved = false
                };

                _db.HealthDeviationAlerts.Add(entity);
                await _db.SaveChangesAsync();

                _publisher.Publish(new NotificationEvent
                {
                    UserId = baby.ParentProfile.UserId,
                    Title = entity.Title,
                    Message = $"{entity.Message} {entity.Recommendation}"
                });

                created.Add(MapToDto(entity, baby.BabyName));
            }

            return created;
        }

        private static void AddIfPresent(List<ParameterDeviationResult> results, ParameterDeviationResult? result)
        {
            if (result is not null)
            {
                results.Add(result);
            }
        }

        private async Task<ParameterDeviationResult?> EvaluateGrowthAsync(BabyProfile baby, int ageMonths)
        {
            var recent = await _db.BabyGrowths
                .Where(g => g.BabyId == baby.Id)
                .OrderByDescending(g => g.WeekNumber)
                .Take(2)
                .ToListAsync();

            if (recent.Count == 0)
            {
                return null;
            }

            var latest = recent[0];
            var latestAgeMonths = latest.WeekNumber / 4.345;
            var z = PediatricStandards.GetGrowthZScores(
                baby.Gender, latestAgeMonths, latest.WeightKg, latest.HeightCm, latest.HeadCircumferenceCm);

            var weightZ = (float)(z.WeightZ ?? 0);
            var heightZ = (float)(z.HeightZ ?? 0);
            var headZ = (float)(z.HeadZ ?? 0);
            var weightZDelta = 0f;

            if (recent.Count == 2 && z.WeightZ.HasValue)
            {
                var previous = recent[1];
                var previousAgeMonths = previous.WeekNumber / 4.345;
                var previousZ = PediatricStandards.GetGrowthZScores(
                    baby.Gender, previousAgeMonths, previous.WeightKg, previous.HeightCm, previous.HeadCircumferenceCm);

                if (previousZ.WeightZ.HasValue)
                {
                    weightZDelta = Math.Max(0f, (float)(previousZ.WeightZ.Value - z.WeightZ.Value));
                }
            }

            var sample = new GrowthSample
            {
                AgeMonths = (float)latestAgeMonths,
                IsFemale = string.Equals(baby.Gender?.Trim(), "Female", StringComparison.OrdinalIgnoreCase) ? 1 : 0,
                WeightZ = weightZ,
                HeightZ = heightZ,
                HeadZ = headZ,
                WeightZDelta = weightZDelta
            };

            var prediction = _ml.PredictGrowth(sample);

            if (!prediction.IsDeviation)
            {
                return null;
            }

            var mostDeviated = new (string Label, float Z)[]
            {
                ("težinu", weightZ),
                ("visinu", heightZ),
                ("obim glave", headZ)
            }.OrderByDescending(f => Math.Abs(f.Z)).First();

            var periodDate = baby.BirthDate.AddDays(latest.WeekNumber * 7);

            var message = weightZDelta >= 0.5f && Math.Abs(mostDeviated.Z) < 2
                ? "Rast bebe pokazuje usporavanje u odnosu na prethodno mjerenje."
                : $"Zadnje mjerenje pokazuje da je {mostDeviated.Label} odstupila od WHO referentnih vrijednosti za uzrast od {ageMonths} mj. " +
                  $"({(mostDeviated.Z < 0 ? "ispod" : "iznad")} prosjeka).";

            return new ParameterDeviationResult
            {
                ParameterType = PediatricParameterType.Growth,
                Severity = prediction.Severity,
                Title = "Odstupanje u rastu bebe",
                Message = AppendConfidence(message, prediction.Confidence),
                Recommendation = "Preporučujemo posjetu pedijatru radi provjere rasta i ishrane.",
                PeriodFrom = periodDate,
                PeriodTo = periodDate
            };
        }

        private async Task<ParameterDeviationResult?> EvaluateFeedingAsync(BabyProfile baby, int ageMonths)
        {
            var guideline = PediatricStandards.GetFeedingGuideline(ageMonths);

            if (guideline is null)
            {
                return null;
            }

            var since = DateTime.UtcNow.Date.AddDays(-2);

            var logs = await _db.FeedingLogs
                .Where(f => f.BabyId == baby.Id && f.FeedDate.Date >= since)
                .ToListAsync();

            var byDay = logs
                .GroupBy(f => f.FeedDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(g => g.Date)
                .ToList();

            if (byDay.Count < 3)
            {
                return null;
            }

            var midpoint = (guideline.Value.MinFeedsPerDay + guideline.Value.MaxFeedsPerDay) / 2.0;
            var halfWidth = Math.Max(0.5, (guideline.Value.MaxFeedsPerDay - guideline.Value.MinFeedsPerDay) / 2.0);
            var avgFeeds = byDay.Average(d => d.Count);
            var relativeDeviation = (float)((avgFeeds - midpoint) / halfWidth);
            var feedsStdDev = (float)StdDev(byDay.Select(d => (double)d.Count));
            var pctDaysOutOfRange = (float)byDay.Count(d => d.Count < guideline.Value.MinFeedsPerDay || d.Count > guideline.Value.MaxFeedsPerDay) / byDay.Count;

            var sample = new FeedingSample
            {
                AgeMonths = ageMonths,
                RelativeDeviation = relativeDeviation,
                FeedsStdDev = feedsStdDev,
                PctDaysOutOfRange = pctDaysOutOfRange
            };

            var prediction = _ml.PredictFeeding(sample);

            if (!prediction.IsDeviation)
            {
                return null;
            }

            var direction = relativeDeviation > 0 ? "češće" : "rjeđe";
            var advice = relativeDeviation > 0
                ? "Moguće prehranjivanje - obratite pažnju na znakove sitosti kod bebe."
                : "Moguće nedovoljno hranjenje - pratite dobijanje na težini.";

            return new ParameterDeviationResult
            {
                ParameterType = PediatricParameterType.Feeding,
                Severity = prediction.Severity,
                Title = "Odstupanje u učestalosti hranjenja",
                Message = AppendConfidence(
                    $"Posljednja {byDay.Count} dana beba je hranjena {direction} nego što je preporučeno za uzrast od {ageMonths} mj. " +
                    $"({guideline.Value.MinFeedsPerDay}-{guideline.Value.MaxFeedsPerDay}x/dan).",
                    prediction.Confidence),
                Recommendation = $"{advice} Ako se obrazac nastavi, javite se pedijatru.",
                PeriodFrom = byDay.First().Date,
                PeriodTo = byDay.Last().Date
            };
        }

        private async Task<ParameterDeviationResult?> EvaluateSleepAsync(BabyProfile baby, int ageMonths)
        {
            var guideline = PediatricStandards.GetSleepGuideline(ageMonths);

            if (guideline is null)
            {
                return null;
            }

            var since = DateTime.UtcNow.Date.AddDays(-2);

            var logs = await _db.SleepLogs
                .Where(s => s.BabyId == baby.Id && s.SleepDate.Date >= since)
                .ToListAsync();

            var byDay = logs
                .GroupBy(s => s.SleepDate.Date)
                .Select(g => new { Date = g.Key, TotalMinutes = g.Sum(x => x.DurationMinutes) })
                .OrderBy(g => g.Date)
                .ToList();

            if (byDay.Count < 3)
            {
                return null;
            }

            var midpointHours = (guideline.Value.MinHours + guideline.Value.MaxHours) / 2.0;
            var halfWidthHours = Math.Max(0.5, (guideline.Value.MaxHours - guideline.Value.MinHours) / 2.0);
            var avgHours = byDay.Average(d => d.TotalMinutes) / 60.0;
            var relativeDeviation = (float)((avgHours - midpointHours) / halfWidthHours);
            var stdDevMinutes = (float)StdDev(byDay.Select(d => (double)d.TotalMinutes));
            var pctDaysOutOfRange = (float)byDay.Count(d =>
                d.TotalMinutes < guideline.Value.MinHours * 60 || d.TotalMinutes > guideline.Value.MaxHours * 60) / byDay.Count;

            var sample = new SleepSample
            {
                AgeMonths = ageMonths,
                RelativeDeviation = relativeDeviation,
                StdDevMinutes = stdDevMinutes,
                PctDaysOutOfRange = pctDaysOutOfRange,
                Direction = relativeDeviation < 0 ? -1 : 1
            };

            var prediction = _ml.PredictSleep(sample);

            if (!prediction.IsDeviation)
            {
                return null;
            }

            var isUnder = relativeDeviation < 0;
            var direction = isUnder ? "manje" : "više";

            return new ParameterDeviationResult
            {
                ParameterType = PediatricParameterType.Sleep,
                Severity = prediction.Severity,
                Title = "Odstupanje u trajanju sna",
                Message = AppendConfidence(
                    $"Posljednja {byDay.Count} dana beba spava {direction} nego preporučeno za uzrast od {ageMonths} mj. " +
                    $"({guideline.Value.MinHours}-{guideline.Value.MaxHours}h/dan).",
                    prediction.Confidence),
                Recommendation = isUnder
                    ? "Pokušajte uvesti stabilniju rutinu spavanja; ako nedostatak sna potraje, konsultujte pedijatra."
                    : "Produženo spavanje samo po sebi obično nije zabrinjavajuće, ali obratite pažnju na letargiju ili otežano buđenje za hranjenje.",
                PeriodFrom = byDay.First().Date,
                PeriodTo = byDay.Last().Date
            };
        }

        private async Task<ParameterDeviationResult?> EvaluateDiapersAsync(BabyProfile baby, int ageMonths)
        {
            var minWet = PediatricStandards.GetMinWetDiapersPerDay(ageMonths);
            var since = DateTime.UtcNow.Date.AddDays(-1);

            var logs = await _db.DiaperLogs
                .Where(d => d.BabyId == baby.Id && d.ChangeDate.Date >= since)
                .ToListAsync();

            var byDay = logs
                .GroupBy(d => d.ChangeDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    WetCount = g.Count(x => x.DiaperState.Equals("Mokra", StringComparison.OrdinalIgnoreCase))
                })
                .OrderBy(g => g.Date)
                .ToList();

            if (byDay.Count < 2)
            {
                return null;
            }

            var avgWetPerDay = (float)byDay.Average(d => d.WetCount);
            var pctDaysBelowMin = (float)byDay.Count(d => d.WetCount < minWet) / byDay.Count;

            var sample = new DiaperSample
            {
                AgeMonths = ageMonths,
                AvgWetPerDay = avgWetPerDay,
                MinWetGuideline = minWet,
                PctDaysBelowMin = pctDaysBelowMin
            };

            var prediction = _ml.PredictDiaper(sample);

            if (!prediction.IsDeviation)
            {
                return null;
            }

            return new ParameterDeviationResult
            {
                ParameterType = PediatricParameterType.Diaper,
                Severity = prediction.Severity,
                Title = "Manje mokrih pelena nego očekivano",
                Message = AppendConfidence(
                    $"Posljednja {byDay.Count} dana zabilježeno je u prosjeku {avgWetPerDay:F1} mokrih pelena dnevno, " +
                    $"što je ispod preporučenog minimuma od {minWet} za uzrast od {ageMonths} mj.",
                    prediction.Confidence),
                Recommendation = "Ovo može biti znak nedovoljnog unosa tečnosti. Ponudite bebi više obroka/tečnosti; " +
                                   "ako se obrazac nastavi, javite se pedijatru.",
                PeriodFrom = byDay.First().Date,
                PeriodTo = byDay.Last().Date
            };
        }

        private async Task<ParameterDeviationResult?> EvaluateFeverAsync(BabyProfile baby, int ageMonths)
        {
            var since = DateTime.UtcNow.Date.AddDays(-2);

            var entries = await _db.HealthEntries
                .Where(h => h.BabyId == baby.Id && h.EntryDate.Date >= since && h.TemperatureC != null)
                .OrderBy(h => h.EntryDate)
                .ToListAsync();

            if (entries.Count == 0)
            {
                return null;
            }

            var highest = entries.Max(e => e.TemperatureC!.Value);
            var feverCount = entries.Count(e => e.TemperatureC!.Value >= PediatricStandards.FeverThresholdC);
            var isYoungInfant = ageMonths < PediatricStandards.YoungInfantUrgentAgeMonths;

            var sample = new FeverSample
            {
                AgeMonths = ageMonths,
                MaxTempC = (float)highest,
                FeverEntryCount = feverCount,
                IsYoungInfant = isYoungInfant ? 1 : 0
            };

            var prediction = _ml.PredictFever(sample);

            if (!prediction.IsDeviation)
            {
                return null;
            }

            var isUrgent = prediction.Severity == HealthAlertSeverity.Critical;

            var message = isYoungInfant && highest >= PediatricStandards.FeverThresholdC
                ? $"Zabilježena je temperatura od {highest}°C kod bebe mlađe od {PediatricStandards.YoungInfantUrgentAgeMonths} mjeseca."
                : highest >= PediatricStandards.HighFeverThresholdC
                    ? $"Zabilježena je visoka temperatura od {highest}°C."
                    : "Povišena temperatura (38°C ili više) zabilježena je više puta u posljednja 3 dana.";

            return new ParameterDeviationResult
            {
                ParameterType = PediatricParameterType.Fever,
                Severity = prediction.Severity,
                Title = "Povišena tjelesna temperatura",
                Message = AppendConfidence(message, prediction.Confidence),
                Recommendation = isUrgent
                    ? "Ovo se smatra hitnim stanjem - odmah kontaktirajte pedijatra ili hitnu pomoć."
                    : "Pratite temperaturu i opšte stanje bebe; ako temperatura ne opada, javite se ljekaru.",
                PeriodFrom = entries.First().EntryDate,
                PeriodTo = entries.Last().EntryDate
            };
        }

        private static string AppendConfidence(string message, float confidence)
        {
            return $"{message} (pouzdanost modela: {confidence:P0})";
        }

        private static double StdDev(IEnumerable<double> values)
        {
            var list = values.ToList();

            if (list.Count < 2)
            {
                return 0;
            }

            var mean = list.Average();
            var variance = list.Sum(v => Math.Pow(v - mean, 2)) / list.Count;

            return Math.Sqrt(variance);
        }

        private static HealthDeviationAlertResponseDto MapToDto(HealthDeviationAlert entity, string babyName)
        {
            return new HealthDeviationAlertResponseDto
            {
                Id = entity.Id,
                BabyId = entity.BabyId,
                BabyName = babyName,
                ParameterType = entity.ParameterType,
                Severity = entity.Severity,
                Title = entity.Title,
                Message = entity.Message,
                Recommendation = entity.Recommendation,
                DetectedAt = entity.DetectedAt,
                PeriodFrom = entity.PeriodFrom,
                PeriodTo = entity.PeriodTo,
                IsResolved = entity.IsResolved,
                ResolvedAt = entity.ResolvedAt
            };
        }
    }
}
