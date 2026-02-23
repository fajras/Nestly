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

        public async Task<List<BlogPostResponseDto>> GetRecommendations(long userId, int take)
        {
            var context = await BuildUserContext(userId);
            var weights = await GetWeights();

            var posts = await _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.Phase == context.Phase)
                .ToListAsync();

            var scored = new List<(BlogPost post, double score)>();

            foreach (var post in posts)
            {
                double weekScore = ComputeWeekScore(context.WeekValue, post.WeekFrom, post.WeekTo);
                double categoryAffinity = await ComputeCategoryAffinity(userId, post.Id);
                double popularity = await ComputePopularity(post.Id);

                var features = new Dictionary<string, double>
        {
            { "bias", 1.0 },
            { "weekScore", weekScore },
            { "categoryAffinity", categoryAffinity },
            { "popularity", popularity }
        };

                double score = Sigmoid(Dot(weights, features));

                scored.Add((post, score));
            }

            return scored
                .OrderByDescending(x => x.score)
                .Take(take)
                .Select(x => new BlogPostResponseDto
                {
                    Id = x.post.Id,
                    Title = x.post.Title,
                    Content = x.post.Content,
                    ImageUrl = x.post.ImageUrl,
                    AuthorId = x.post.AuthorId,
                    Phase = x.post.Phase,
                    WeekFrom = x.post.WeekFrom,
                    WeekTo = x.post.WeekTo
                })
                .ToList();
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

            bool isPositive = request.EventType == 2 && request.SpentSeconds >= 15;

            await Train(userId, request.PostId, isPositive);
        }

        private async Task Train(long userId, long postId, bool positive)
        {
            var weights = await GetWeights();
            var context = await BuildUserContext(userId);
            var post = await _db.BlogPosts.FirstAsync(p => p.Id == postId);

            double weekScore = ComputeWeekScore(context.WeekValue, post.WeekFrom, post.WeekTo);
            double categoryAffinity = await ComputeCategoryAffinity(userId, post.Id);
            double popularity = await ComputePopularity(post.Id);

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

            return interactions / 10.0;
        }

        private async Task<double> ComputePopularity(long postId)
        {
            int count = await _db.BlogPostInteractions
                .CountAsync(i => i.PostId == postId);

            return count / 50.0;
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
    }
}
