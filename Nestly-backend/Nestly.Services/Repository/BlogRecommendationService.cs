using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using System.Text.Json;

namespace Nestly.Services.Repository
{
    public class BlogRecommendationService : IBlogRecommendationService
    {
        private readonly NestlyDbContext _db;

        public BlogRecommendationService(NestlyDbContext db)
        {
            _db = db;
        }

        public async Task<List<BlogPostRecommendationDto>> GetRecommendations(long userId, int take)
        {
            if (take <= 0)
            {
                take = 5;
            }

            var context = await BuildUserContext(userId);
            var weights = await GetWeights();

            var posts = await _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p =>
                    p.Phase == context.Phase &&
                    p.WeekFrom.HasValue &&
                    p.WeekTo.HasValue &&
                    context.WeekValue >= p.WeekFrom.Value - 2 &&
                    context.WeekValue <= p.WeekTo.Value + 2
                )
                .ToListAsync();

            if (!posts.Any())
            {
                posts = await _db.BlogPosts
                    .Include(p => p.BlogPostCategories)
                    .ThenInclude(pc => pc.Category)
                    .Where(p => p.Phase == context.Phase)
                    .OrderByDescending(p => p.Id)
                    .Take(20)
                    .ToListAsync();
            }

            if (!posts.Any())
            {
                posts = await _db.BlogPosts
                    .Include(p => p.BlogPostCategories)
                    .ThenInclude(pc => pc.Category)
                    .OrderByDescending(p => p.Id)
                    .Take(20)
                    .ToListAsync();
            }

            var scored = new List<(BlogPost post, double score, List<RecommendationScoreBreakdown> breakdown)>();

            foreach (var post in posts)
            {
                double weekScore = ComputeWeekScore(context.WeekValue, post.WeekFrom, post.WeekTo);
                double categoryAffinity = Math.Min(1.0, await ComputeCategoryAffinity(userId, post.Id));
                double popularity = Math.Min(1.0, await ComputePopularity(post.Id));

                var features = new Dictionary<string, double>
        {
            { "bias", 1.0 },
            { "weekScore", weekScore },
            { "categoryAffinity", categoryAffinity },
            { "popularity", popularity }
        };

                double rawScore = Dot(weights, features);

                if (weekScore < 0.3)
                {
                    rawScore -= 1.5;
                }

                double score = Sigmoid(rawScore);

                var breakdown = new List<RecommendationScoreBreakdown>
        {
            new()
            {
                FactorName = "weekScore",
                Value = weekScore,
                Weight = weights["weekScore"],
                Explanation = GetWeekScoreExplanation(weekScore, context.WeekValue, post.WeekFrom, post.WeekTo)
            },
            new()
            {
                FactorName = "categoryAffinity",
                Value = categoryAffinity,
                Weight = weights["categoryAffinity"],
                Explanation = GetCategoryAffinityExplanation(categoryAffinity)
            },
            new()
            {
                FactorName = "popularity",
                Value = popularity,
                Weight = weights["popularity"],
                Explanation = GetPopularityExplanation(popularity)
            }
        };

                scored.Add((post, score, breakdown));
            }

            return scored
                .OrderByDescending(x => x.score)
                .Take(take)
                .Select(x => new BlogPostRecommendationDto
                {
                    Id = x.post.Id,
                    Title = x.post.Title,
                    Content = x.post.Content,
                    ImageUrl = x.post.ImageUrl,
                    Phase = (int)x.post.Phase,
                    WeekFrom = x.post.WeekFrom,
                    WeekTo = x.post.WeekTo,
                    FinalScore = x.score,
                    ScoreBreakdown = x.breakdown,
                    MainReason = GenerateMainReason(x.breakdown)
                })
                .ToList();
        }
        private string GenerateMainReason(List<RecommendationScoreBreakdown> breakdown)
        {
            var top = breakdown
                .Where(x => x.Value > 0.3)
                .OrderByDescending(x => x.Value * x.Weight)
                .FirstOrDefault();

            if (top == null)
            {
                return "Preporuka na osnovu opšte popularnosti";
            }

            return $"Preporučeno jer: {top.Explanation}";
        }

        private string GetWeekScoreExplanation(double score, int userWeek, int? from, int? to)
        {
            if (score == 1.0)
            {
                return $"Idealno za vašu trenutnu sedmicu ({userWeek}.)";
            }

            if (score >= 0.5)
            {
                return $"Blizu vašoj trenutnoj sedmici ({userWeek}.)";
            }

            return "Manje relevantno";
        }

        private string GetCategoryAffinityExplanation(double score)
        {
            if (score >= 0.8)
            {
                return "Često čitate ovu kategoriju";
            }

            if (score >= 0.4)
            {
                return "Ponekad čitate ovu kategoriju";
            }

            return "Rijetko čitate ovu kategoriju";
        }

        private string GetPopularityExplanation(double score)
        {
            if (score >= 0.8)
            {
                return "Veoma popularan sadržaj";
            }

            if (score >= 0.4)
            {
                return "Umjereno popularan";
            }

            return "Nova ili manje popularna objava";
        }

        private async Task<(UserPhase Phase, int WeekValue)> BuildUserContext(long userId)
        {
            var user = await _db.AppUsers
                .Include(u => u.ParentProfile)
                .ThenInclude(p => p.Babies)
                .Include(u => u.ParentProfile)
                .ThenInclude(p => p.Pregnancies)
                .FirstAsync(u => u.Id == userId);

            if (user.ParentProfile?.Babies.Any() == true)
            {
                var baby = user.ParentProfile.Babies
                    .OrderByDescending(b => b.BirthDate)
                    .First();

                int weeks = (int)((DateTime.UtcNow - baby.BirthDate).TotalDays / 7);

                return (UserPhase.BabyTime, weeks);
            }

            if (user.ParentProfile?.Pregnancies.Any() == true)
            {
                var preg = user.ParentProfile.Pregnancies
                    .OrderByDescending(p => p.DueDate)
                    .First();

                int weeks = 40 - (int)((preg.DueDate.Value - DateTime.UtcNow).TotalDays / 7);

                return (UserPhase.BellyTime, weeks);
            }

            return (UserPhase.BellyTime, 1);
        }

        private double ComputeWeekScore(int userWeek, int? from, int? to)
        {
            if (!from.HasValue || !to.HasValue)
            {
                return 0.5;
            }

            if (userWeek >= from && userWeek <= to)
            {
                return 1.0;
            }

            int distance = userWeek < from ? from.Value - userWeek : userWeek - to.Value;

            return 1.0 / (1 + distance);
        }

        private async Task<double> ComputeCategoryAffinity(long userId, long postId)
        {
            var postCategories = await _db.BlogPostCategories
                .Where(pc => pc.PostId == postId)
                .Select(pc => pc.CategoryId)
                .ToListAsync();

            var interactions = await _db.BlogPostInteractions
                .Where(i => i.UserId == userId)
                .Join(_db.BlogPostCategories,
                    i => i.PostId,
                    pc => pc.PostId,
                    (i, pc) => new { i, pc })
                .Where(x => postCategories.Contains(x.pc.CategoryId))
                .CountAsync();

            return Math.Min(1.0, interactions / 10.0);
        }

        private async Task<double> ComputePopularity(long postId)
        {
            int count = await _db.BlogPostInteractions
                .CountAsync(i => i.PostId == postId);

            return Math.Min(1.0, count / 50.0);
        }

        private async Task<Dictionary<string, double>> GetWeights()
        {
            var state = await _db.RecommendationModelStates
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (state == null)
            {
                var initial = new Dictionary<string, double>
            {
                { "bias", 0 },
                { "weekScore", 1 },
                { "categoryAffinity", 1 },
                { "popularity", 0.5 }
            };

                var newState = new RecommendationModelState
                {
                    UpdatedAt = DateTime.UtcNow,
                    WeightsJson = JsonSerializer.Serialize(initial)
                };

                _db.RecommendationModelStates.Add(newState);
                await _db.SaveChangesAsync();

                return initial;
            }

            return JsonSerializer.Deserialize<Dictionary<string, double>>(state.WeightsJson);
        }

        private async Task SaveWeights(Dictionary<string, double> weights)
        {
            var state = await _db.RecommendationModelStates
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (state == null)
            {
                state = new RecommendationModelState();
                _db.RecommendationModelStates.Add(state);
            }

            state.WeightsJson = JsonSerializer.Serialize(weights);
            state.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        private double Dot(Dictionary<string, double> w, Dictionary<string, double> x)
        {
            double sum = 0;
            foreach (var f in x)
            {
                sum += w[f.Key] * f.Value;
            }

            return sum;
        }

        private double Sigmoid(double z)
        {
            return 1.0 / (1.0 + Math.Exp(-z));
        }

        public async Task LogInteraction(long userId, LogBlogInteractionRequest request)
        {
            _db.BlogPostInteractions.Add(new BlogPostInteraction
            {
                UserId = userId,
                PostId = request.PostId,
                EventType = (ArticleEventType)request.EventType,
                SpentSeconds = request.SpentSeconds,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            bool isPositive =
            request.EventType == 2 &&
            request.SpentSeconds.HasValue &&
            request.SpentSeconds.Value >= 15;

            await Train(userId, request.PostId, isPositive);
        }

        private async Task Train(long userId, long postId, bool positive)
        {
            var weights = await GetWeights();
            var context = await BuildUserContext(userId);
            var post = await _db.BlogPosts.FirstAsync(p => p.Id == postId);

            double weekScore = ComputeWeekScore(context.WeekValue, post.WeekFrom, post.WeekTo);
            double categoryAffinity = Math.Min(1.0, await ComputeCategoryAffinity(userId, post.Id));
            double popularity = Math.Min(1.0, await ComputePopularity(post.Id));

            var features = new Dictionary<string, double>
         {
             { "bias", 1.0 },
             { "weekScore", weekScore },
             { "categoryAffinity", categoryAffinity },
             { "popularity", popularity }
         };

            double prediction = Sigmoid(Dot(weights, features));
            double target = positive ? 1 : 0;
            double error = target - prediction;
            double lr = 0.1;

            foreach (var f in features)
            {
                weights[f.Key] += lr * error * f.Value;
            }

            await SaveWeights(weights);
        }

    }

}
