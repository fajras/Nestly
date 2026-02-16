using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class MealPlanService : IMealPlanService
    {
        private readonly NestlyDbContext _db;

        public MealPlanService(NestlyDbContext db)
        {
            _db = db;
        }

        public List<MealPlanResponseDto> Get(MealPlanSearchObject? search)
        {
            IQueryable<MealPlan> q = _db.MealPlans
                .Include(x => x.FoodType)
                .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.FoodTypeId is not null)
            {
                q = q.Where(x => x.FoodTypeId == search.FoodTypeId);
            }

            if (search?.From is not null)
            {
                q = q.Where(x => x.TriedAt >= search.From.Value);
            }

            if (search?.To is not null)
            {
                q = q.Where(x => x.TriedAt <= search.To.Value);
            }

            return q
                .OrderByDescending(x => x.TriedAt)
                .Select(MapToDto)
                .ToList();
        }

        public MealPlanResponseDto? GetById(long id)
        {
            var entity = _db.MealPlans
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            return entity is null ? null : MapToDto(entity);
        }

        public MealPlanResponseDto Create(CreateMealPlanDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new ArgumentException("Baby does not exist.");
            }

            if (!_db.FoodTypes.Any(f => f.Id == dto.FoodTypeId))
            {
                throw new ArgumentException("FoodType does not exist.");
            }

            if (dto.Rating is < 0 or > 5)
            {
                throw new ArgumentException("Rating must be between 0 and 5.");
            }

            var entity = new MealPlan
            {
                BabyId = dto.BabyId,
                FoodTypeId = dto.FoodTypeId,
                Rating = dto.Rating,
                TriedAt = dto.TriedAt ?? DateTime.UtcNow
            };

            _db.MealPlans.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public MealPlanResponseDto? Patch(long id, MealPlanPatchDto patch)
        {
            var entity = _db.MealPlans
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                return null;
            }

            if (patch.Rating is not null)
            {
                if (patch.Rating is < 0 or > 5)
                {
                    throw new ArgumentException("Rating must be between 0 and 5.");
                }

                entity.Rating = patch.Rating;
            }

            if (patch.TriedAt is not null)
            {
                entity.TriedAt = patch.TriedAt.Value;
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public bool Delete(long id)
        {
            var entity = _db.MealPlans.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return false;
            }

            _db.MealPlans.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public List<MealRecommendationDto> Get(MealRecommendationSearchObject? search)
        {
            IQueryable<MealRecommendation> q = _db.MealRecommendations
                .Include(x => x.FoodType);

            if (search?.WeekNumber is not null)
            {
                q = q.Where(x => x.WeekNumber == search.WeekNumber.Value);
            }

            return q
                .OrderBy(x => x.WeekNumber)
                .ThenBy(x => x.FoodType.Name)
                .Select(x => new MealRecommendationDto
                {
                    Id = x.Id,
                    WeekNumber = x.WeekNumber,
                    FoodTypeId = x.FoodTypeId,
                    FoodName = x.FoodType.Name
                })
                .ToList();
        }

        public MealRecommendationDto? GetRecommendationById(long id)
        {
            var entity = _db.MealRecommendations
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                return null;
            }

            return new MealRecommendationDto
            {
                Id = entity.Id,
                WeekNumber = entity.WeekNumber,
                FoodTypeId = entity.FoodTypeId,
                FoodName = entity.FoodType.Name
            };
        }

        private static MealPlanResponseDto MapToDto(MealPlan x)
        {
            return new MealPlanResponseDto
            {
                Id = x.Id,
                BabyId = x.BabyId,
                FoodTypeId = x.FoodTypeId,
                FoodName = x.FoodType != null ? x.FoodType.Name : string.Empty,
                Rating = x.Rating,
                TriedAt = x.TriedAt
            };
        }
    }
}
