using Nestly.Model.WhoStandards;

namespace Nestly.ML
{
    /// <summary>
    /// Generates the training datasets for the five WHO-standards deviation
    /// models. There is no real dataset of infant care logs available (that
    /// would be private medical data), so this uses weak/programmatic
    /// supervision: feature vectors are sampled from realistic distributions
    /// centered on <see cref="PediatricStandards"/> reference ranges, and
    /// each row's label is assigned by a domain-rule labeling function that
    /// encodes the WHO/AAP/AASM/IMCI thresholds described in
    /// who-standards-dataset.md - including feature *interactions*
    /// (e.g. two simultaneous mild deviations, or persistence across days)
    /// that a single hand-written per-parameter if/else check would miss.
    /// The trained classifier then learns a generalized decision boundary
    /// over these continuous features, rather than replaying the thresholds
    /// verbatim, and can be re-trained if the reference ranges change.
    ///
    /// A small amount of random label noise (~4%) is injected near class
    /// boundaries to avoid a degenerate, trivially-separable dataset and to
    /// mimic real-world labeling uncertainty.
    /// </summary>
    public static class SyntheticPediatricDatasetGenerator
    {
        private const double LabelNoiseRate = 0.04;

        public static IEnumerable<GrowthSample> GenerateGrowthSamples(int count, int seed = 1)
        {
            var rng = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                var ageMonths = (float)(rng.NextDouble() * 24);
                var isFemale = rng.Next(2);

                var weightZ = (float)SampleDeviationZ(rng);
                var heightZ = (float)SampleDeviationZ(rng);
                var headZ = (float)SampleDeviationZ(rng);

                // Faltering-growth event: an occasional large drop versus the
                // previous measurement, loosely correlated with a currently
                // low weight-for-age z-score.
                var falteringChance = weightZ < -1 ? 0.35 : 0.08;
                var weightZDelta = rng.NextDouble() < falteringChance
                    ? (float)Math.Max(0, NextGaussian(rng, 1.3, 0.6))
                    : (float)Math.Max(0, NextGaussian(rng, 0.1, 0.3));

                var label = LabelGrowth(weightZ, heightZ, headZ, weightZDelta);
                label = MaybeFlipLabel(rng, label);

                yield return new GrowthSample
                {
                    AgeMonths = ageMonths,
                    IsFemale = isFemale,
                    WeightZ = weightZ,
                    HeightZ = heightZ,
                    HeadZ = headZ,
                    WeightZDelta = weightZDelta,
                    Label = label
                };
            }
        }

        public static IEnumerable<FeedingSample> GenerateFeedingSamples(int count, int seed = 2)
        {
            var rng = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                var ageMonths = (int)(rng.NextDouble() * 24);
                var guideline = PediatricStandards.GetFeedingGuideline(ageMonths)!.Value;

                var midpoint = (guideline.MinFeedsPerDay + guideline.MaxFeedsPerDay) / 2.0;
                var halfWidth = Math.Max(0.5, (guideline.MaxFeedsPerDay - guideline.MinFeedsPerDay) / 2.0);

                var relativeDeviation = (float)SampleDeviationZ(rng, scale: 1.0);
                var avgFeeds = midpoint + relativeDeviation * halfWidth;
                avgFeeds = Math.Clamp(avgFeeds, 1, 20);
                relativeDeviation = (float)((avgFeeds - midpoint) / halfWidth);

                var feedsStdDev = (float)Math.Max(0, NextGaussian(rng, 0.6, 0.4));

                var consistencyBias = Math.Abs(relativeDeviation) > 1 ? 0.75 : 0.25;
                var pctDaysOutOfRange = (float)Math.Clamp(NextGaussian(rng, consistencyBias, 0.25), 0, 1);

                var label = LabelWithConsistency(
                    Math.Abs(relativeDeviation), pctDaysOutOfRange,
                    criticalRel: 1.2, criticalPct: 0.9,
                    warningRel: 0.4, warningPct: 0.6);
                label = MaybeFlipLabel(rng, label);

                yield return new FeedingSample
                {
                    AgeMonths = ageMonths,
                    RelativeDeviation = relativeDeviation,
                    FeedsStdDev = feedsStdDev,
                    PctDaysOutOfRange = pctDaysOutOfRange,
                    Label = label
                };
            }
        }

        public static IEnumerable<SleepSample> GenerateSleepSamples(int count, int seed = 3)
        {
            var rng = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                var ageMonths = (int)(rng.NextDouble() * 24);
                var guideline = PediatricStandards.GetSleepGuideline(ageMonths)!.Value;

                var midpoint = (guideline.MinHours + guideline.MaxHours) / 2.0;
                var halfWidth = Math.Max(0.5, (guideline.MaxHours - guideline.MinHours) / 2.0);

                var relativeDeviation = (float)SampleDeviationZ(rng, scale: 1.0);
                var avgHours = Math.Clamp(midpoint + relativeDeviation * halfWidth, 4, 20);
                relativeDeviation = (float)((avgHours - midpoint) / halfWidth);

                var stdDevMinutes = (float)Math.Max(0, NextGaussian(rng, 35, 20));

                var consistencyBias = Math.Abs(relativeDeviation) > 1 ? 0.75 : 0.25;
                var pctDaysOutOfRange = (float)Math.Clamp(NextGaussian(rng, consistencyBias, 0.25), 0, 1);

                var direction = relativeDeviation < 0 ? -1f : 1f;

                string label;
                if (direction < 0)
                {
                    label = LabelWithConsistency(
                        Math.Abs(relativeDeviation), pctDaysOutOfRange,
                        criticalRel: 999, criticalPct: 999, // sleeping too little is never auto-"Critical" on its own
                        warningRel: 0.3, warningPct: 0.6);
                }
                else
                {
                    var over = pctDaysOutOfRange >= 0.9 && relativeDeviation >= 1.5;
                    label = over ? SeverityLabel.Warning : SeverityLabel.Normal;
                }

                // Sleep deviations alone are only ever Normal/Warning by
                // design (see labeling above) - use the binary noise flip so
                // label noise doesn't manufacture a near-empty "Critical"
                // class that the model can't learn anything from.
                label = MaybeFlipBinaryLabel(rng, label);

                yield return new SleepSample
                {
                    AgeMonths = ageMonths,
                    RelativeDeviation = relativeDeviation,
                    StdDevMinutes = stdDevMinutes,
                    PctDaysOutOfRange = pctDaysOutOfRange,
                    Direction = direction,
                    Label = label
                };
            }
        }

        public static IEnumerable<DiaperSample> GenerateDiaperSamples(int count, int seed = 4)
        {
            var rng = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                var ageMonths = (int)(rng.NextDouble() * 24);
                var minGuideline = PediatricStandards.GetMinWetDiapersPerDay(ageMonths);

                var deviation = SampleDeviationZ(rng, scale: 1.5);
                var avgWetPerDay = (float)Math.Clamp(minGuideline + deviation, 0, 12);

                var belowBias = avgWetPerDay < minGuideline ? 0.8 : 0.15;
                var pctDaysBelowMin = (float)Math.Clamp(NextGaussian(rng, belowBias, 0.25), 0, 1);

                var deficit = minGuideline - avgWetPerDay;

                var label = SeverityLabel.Normal;
                if (pctDaysBelowMin >= 0.9 && deficit >= 2)
                {
                    label = SeverityLabel.Critical;
                }
                else if (pctDaysBelowMin >= 0.5 && avgWetPerDay < minGuideline)
                {
                    label = SeverityLabel.Warning;
                }

                label = MaybeFlipLabel(rng, label);

                yield return new DiaperSample
                {
                    AgeMonths = ageMonths,
                    AvgWetPerDay = avgWetPerDay,
                    MinWetGuideline = minGuideline,
                    PctDaysBelowMin = pctDaysBelowMin,
                    Label = label
                };
            }
        }

        public static IEnumerable<FeverSample> GenerateFeverSamples(int count, int seed = 5)
        {
            var rng = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                var ageMonths = (int)(rng.NextDouble() * 24);
                var isYoungInfant = ageMonths < PediatricStandards.YoungInfantUrgentAgeMonths ? 1 : 0;

                // Mixture: mostly afebrile, some low-grade, some high fever.
                var roll = rng.NextDouble();
                float maxTemp = roll switch
                {
                    < 0.55 => (float)NextGaussian(rng, 36.8, 0.4),
                    < 0.80 => (float)NextGaussian(rng, 38.3, 0.4),
                    _ => (float)NextGaussian(rng, 39.3, 0.6)
                };
                maxTemp = Math.Clamp(maxTemp, 35.5f, 41.5f);

                var feverEntryCount = maxTemp >= (float)PediatricStandards.FeverThresholdC
                    ? rng.Next(1, 4)
                    : rng.Next(0, 2);

                var label = SeverityLabel.Normal;

                if (isYoungInfant == 1 && maxTemp >= (float)PediatricStandards.FeverThresholdC)
                {
                    label = SeverityLabel.Critical;
                }
                else if (maxTemp >= (float)PediatricStandards.HighFeverThresholdC)
                {
                    label = SeverityLabel.Critical;
                }
                else if (feverEntryCount >= 2 && maxTemp >= (float)PediatricStandards.FeverThresholdC)
                {
                    label = SeverityLabel.Warning;
                }

                label = MaybeFlipLabel(rng, label);

                yield return new FeverSample
                {
                    AgeMonths = ageMonths,
                    MaxTempC = maxTemp,
                    FeverEntryCount = feverEntryCount,
                    IsYoungInfant = isYoungInfant,
                    Label = label
                };
            }
        }

        private static string LabelGrowth(float weightZ, float heightZ, float headZ, float weightZDelta)
        {
            var anyExtreme = Math.Abs(weightZ) >= 3 || Math.Abs(heightZ) >= 3 || Math.Abs(headZ) >= 3;
            var falteringTrend = weightZDelta >= 1.0;

            if (anyExtreme || falteringTrend)
            {
                return SeverityLabel.Critical;
            }

            var combinedMild =
                (weightZ <= -1.5 && heightZ <= -1.5) ||
                (weightZ <= -1.5 && headZ <= -1.5) ||
                (weightZ >= 1.5 && heightZ >= 1.5);

            var anyModerate = Math.Abs(weightZ) >= 2 || Math.Abs(heightZ) >= 2 || Math.Abs(headZ) >= 2;

            if (anyModerate || combinedMild || weightZDelta >= 0.5)
            {
                return SeverityLabel.Warning;
            }

            return SeverityLabel.Normal;
        }

        private static string LabelWithConsistency(
            double absRelativeDeviation, double pctDaysOutOfRange,
            double criticalRel, double criticalPct,
            double warningRel, double warningPct)
        {
            if (pctDaysOutOfRange >= criticalPct && absRelativeDeviation >= criticalRel)
            {
                return SeverityLabel.Critical;
            }

            if (pctDaysOutOfRange >= warningPct && absRelativeDeviation >= warningRel)
            {
                return SeverityLabel.Warning;
            }

            return SeverityLabel.Normal;
        }

        private static string MaybeFlipLabel(Random rng, string label)
        {
            if (rng.NextDouble() >= LabelNoiseRate)
            {
                return label;
            }

            return label switch
            {
                SeverityLabel.Normal => SeverityLabel.Warning,
                SeverityLabel.Warning => rng.NextDouble() < 0.5 ? SeverityLabel.Normal : SeverityLabel.Critical,
                _ => SeverityLabel.Warning
            };
        }

        private static string MaybeFlipBinaryLabel(Random rng, string label)
        {
            if (rng.NextDouble() >= LabelNoiseRate)
            {
                return label;
            }

            return label == SeverityLabel.Normal ? SeverityLabel.Warning : SeverityLabel.Normal;
        }

        // Mixture of three Gaussians so most samples are "normal" while a
        // meaningful minority are moderately/severely deviated - avoids a
        // dataset that is almost entirely one class.
        private static double SampleDeviationZ(Random rng, double scale = 1.0)
        {
            var roll = rng.NextDouble();
            var sign = rng.NextDouble() < 0.5 ? -1 : 1;

            return roll switch
            {
                < 0.70 => NextGaussian(rng, 0, 0.8) * scale,
                < 0.90 => sign * NextGaussian(rng, 2.3, 0.5) * scale,
                _ => sign * NextGaussian(rng, 3.3, 0.6) * scale
            };
        }

        private static double NextGaussian(Random rng, double mean, double stdDev)
        {
            var u1 = 1.0 - rng.NextDouble();
            var u2 = 1.0 - rng.NextDouble();
            var standardNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * standardNormal;
        }
    }
}
