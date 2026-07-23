using System;
using System.Linq;

namespace Nestly.Model.WhoStandards
{
    /// <summary>
    /// Reference dataset for the "WHO deviation monitoring" module.
    ///
    /// Growth data: median weight/height/head-circumference-for-age landmark
    /// values (0-24 months, boys/girls) adapted from the WHO Child Growth
    /// Standards (WHO Multicentre Growth Reference Study, 2006,
    /// https://www.who.int/tools/child-growth-standards). Values between the
    /// listed months are linearly interpolated, and the standard deviation
    /// is approximated as a fixed fraction of the median (WHO publishes an
    /// exact SD/LMS column per month which is not reproduced here) - this is
    /// a simplified/adapted model suitable for early-warning screening, not
    /// a clinical diagnostic tool. Swap in the official WHO Anthro CSV
    /// tables here if exact figures are required.
    ///
    /// Feeding: WHO/UNICEF Infant and Young Child Feeding (IYCF) guidance
    /// plus commonly published age-appropriate feeding-frequency ranges.
    ///
    /// Sleep: American Academy of Sleep Medicine / National Sleep Foundation
    /// consensus recommendations (WHO does not publish detailed sleep
    /// duration norms).
    ///
    /// Diaper/hydration: American Academy of Pediatrics minimum wet-diaper
    /// guidance, used here as a simple hydration proxy.
    ///
    /// Fever: WHO IMCI (Integrated Management of Childhood Illness) danger
    /// sign thresholds.
    /// </summary>
    public static class PediatricStandards
    {
        private const double WeightSdFraction = 0.13;
        private const double HeightSdFraction = 0.04;
        private const double HeadSdFraction = 0.03;

        public readonly struct GrowthZScoreResult
        {
            public decimal? WeightZ { get; init; }
            public decimal? HeightZ { get; init; }
            public decimal? HeadZ { get; init; }
        }

        public readonly struct FeedingGuideline
        {
            public int MinFeedsPerDay { get; init; }
            public int MaxFeedsPerDay { get; init; }
        }

        public readonly struct SleepGuideline
        {
            public double MinHours { get; init; }
            public double MaxHours { get; init; }
        }

        // (AgeMonths, WeightKg, HeightCm, HeadCircumferenceCm) - WHO 50th percentile landmarks.
        private static readonly (double Months, double Weight, double Height, double Head)[] BoyGrowth =
        {
            (0, 3.3, 49.9, 34.5),
            (1, 4.5, 54.7, 37.3),
            (2, 5.6, 58.4, 39.1),
            (3, 6.4, 61.4, 40.5),
            (4, 7.0, 63.9, 41.6),
            (5, 7.5, 65.9, 42.6),
            (6, 7.9, 67.6, 43.3),
            (9, 8.9, 72.0, 45.0),
            (12, 9.6, 75.7, 46.1),
            (15, 10.3, 79.1, 47.0),
            (18, 10.9, 82.3, 47.7),
            (21, 11.5, 85.1, 48.1),
            (24, 12.2, 87.1, 48.3),
        };

        private static readonly (double Months, double Weight, double Height, double Head)[] GirlGrowth =
        {
            (0, 3.2, 49.1, 33.9),
            (1, 4.2, 53.7, 36.5),
            (2, 5.1, 57.1, 38.3),
            (3, 5.8, 59.8, 39.5),
            (4, 6.4, 62.1, 40.6),
            (5, 6.9, 64.0, 41.5),
            (6, 7.3, 65.7, 42.2),
            (9, 8.2, 70.1, 43.8),
            (12, 8.9, 74.0, 44.9),
            (15, 9.6, 77.5, 45.8),
            (18, 10.2, 80.7, 46.4),
            (21, 10.9, 83.7, 46.9),
            (24, 11.5, 85.7, 47.2),
        };

        public static GrowthZScoreResult GetGrowthZScores(
            string? gender,
            double ageMonths,
            decimal? weightKg,
            decimal? heightCm,
            decimal? headCircumferenceCm)
        {
            var table = IsFemale(gender) ? GirlGrowth : BoyGrowth;
            var (medianWeight, medianHeight, medianHead) = InterpolateMedians(table, ageMonths);

            return new GrowthZScoreResult
            {
                WeightZ = weightKg.HasValue
                    ? ComputeZ((double)weightKg.Value, medianWeight, medianWeight * WeightSdFraction)
                    : null,
                HeightZ = heightCm.HasValue
                    ? ComputeZ((double)heightCm.Value, medianHeight, medianHeight * HeightSdFraction)
                    : null,
                HeadZ = headCircumferenceCm.HasValue
                    ? ComputeZ((double)headCircumferenceCm.Value, medianHead, medianHead * HeadSdFraction)
                    : null,
            };
        }

        // WHO/UNICEF IYCF-derived feeding-frequency ranges by age bracket.
        public static FeedingGuideline? GetFeedingGuideline(int ageMonths)
        {
            if (ageMonths < 0 || ageMonths > 24)
            {
                return null;
            }

            if (ageMonths < 1) return new FeedingGuideline { MinFeedsPerDay = 8, MaxFeedsPerDay = 12 };
            if (ageMonths < 2) return new FeedingGuideline { MinFeedsPerDay = 7, MaxFeedsPerDay = 9 };
            if (ageMonths < 4) return new FeedingGuideline { MinFeedsPerDay = 6, MaxFeedsPerDay = 8 };
            if (ageMonths < 6) return new FeedingGuideline { MinFeedsPerDay = 5, MaxFeedsPerDay = 7 };
            if (ageMonths < 9) return new FeedingGuideline { MinFeedsPerDay = 4, MaxFeedsPerDay = 6 };
            if (ageMonths < 12) return new FeedingGuideline { MinFeedsPerDay = 3, MaxFeedsPerDay = 5 };
            return new FeedingGuideline { MinFeedsPerDay = 3, MaxFeedsPerDay = 5 };
        }

        // AASM / National Sleep Foundation consensus total-sleep-per-day ranges.
        public static SleepGuideline? GetSleepGuideline(int ageMonths)
        {
            if (ageMonths < 0 || ageMonths > 24)
            {
                return null;
            }

            if (ageMonths < 4) return new SleepGuideline { MinHours = 14, MaxHours = 17 };
            if (ageMonths < 12) return new SleepGuideline { MinHours = 12, MaxHours = 16 };
            return new SleepGuideline { MinHours = 11, MaxHours = 14 };
        }

        // AAP minimum wet-diaper-per-day guidance, used as a simple hydration proxy.
        public static int GetMinWetDiapersPerDay(int ageMonths)
        {
            return ageMonths < 1 ? 6 : 4;
        }

        // WHO IMCI danger-sign fever thresholds (degrees Celsius).
        public const decimal HighFeverThresholdC = 39.0m;
        public const decimal FeverThresholdC = 38.0m;
        public const int YoungInfantUrgentAgeMonths = 3;

        private static bool IsFemale(string? gender)
        {
            return string.Equals(gender?.Trim(), "Female", StringComparison.OrdinalIgnoreCase);
        }

        private static (double Weight, double Height, double Head) InterpolateMedians(
            (double Months, double Weight, double Height, double Head)[] table,
            double ageMonths)
        {
            if (ageMonths <= table[0].Months)
            {
                return (table[0].Weight, table[0].Height, table[0].Head);
            }

            if (ageMonths >= table[^1].Months)
            {
                return (table[^1].Weight, table[^1].Height, table[^1].Head);
            }

            var upperIndex = table.ToList().FindIndex(p => p.Months >= ageMonths);
            var lower = table[upperIndex - 1];
            var upper = table[upperIndex];

            var span = upper.Months - lower.Months;
            var t = span == 0 ? 0 : (ageMonths - lower.Months) / span;

            return (
                Lerp(lower.Weight, upper.Weight, t),
                Lerp(lower.Height, upper.Height, t),
                Lerp(lower.Head, upper.Head, t));
        }

        private static double Lerp(double from, double to, double t)
        {
            return from + (to - from) * t;
        }

        private static decimal ComputeZ(double value, double median, double sd)
        {
            if (sd <= 0)
            {
                return 0;
            }

            return (decimal)((value - median) / sd);
        }
    }
}
