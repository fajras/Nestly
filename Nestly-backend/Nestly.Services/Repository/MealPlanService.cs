using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
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

        public PagedResult<MealPlanResponseDto> GetMealPlans(MealPlanSearchObject search)
        {
            IQueryable<MealPlan> q = _db.MealPlans
                .Include(x => x.FoodType)
                .AsQueryable();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.FoodTypeId is not null)
            {
                q = q.Where(x => x.FoodTypeId == search.FoodTypeId);
            }

            if (search.From is not null)
            {
                q = q.Where(x => x.TriedAt >= search.From.Value);
            }

            if (search.To is not null)
            {
                q = q.Where(x => x.TriedAt <= search.To.Value);
            }

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(x => x.TriedAt)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResult<MealPlanResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public MealPlanResponseDto GetById(long id)
        {
            var entity = _db.MealPlans
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Meal plan not found.");
            }

            return MapToDto(entity);
        }

        public MealPlanResponseDto Create(CreateMealPlanDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (dto.BabyId <= 0)
            {
                throw new BusinessException("Baby is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new NotFoundException("Baby profile not found.");
            }

            if (!_db.FoodTypes.Any(f => f.Id == dto.FoodTypeId))
            {
                throw new NotFoundException("Food type not found.");
            }

            if (dto.Rating is < 0 or > 5)
            {
                throw new BusinessException("Rating must be between 0 and 5.");
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

            var created = _db.MealPlans
                .Include(x => x.FoodType)
                .First(x => x.Id == entity.Id);

            return MapToDto(created);
        }
        public MealPlanResponseDto Patch(long id, MealPlanPatchDto patch)
        {
            var entity = _db.MealPlans
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Meal plan not found.");
            }

            if (patch.Rating is not null)
            {
                if (patch.Rating is < 0 or > 5)
                {
                    throw new BusinessException("Rating must be between 0 and 5.");
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
        public void Delete(long id)
        {
            var entity = _db.MealPlans.FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Meal plan not found.");
            }

            _db.MealPlans.Remove(entity);
            _db.SaveChanges();
        }
        public PagedResult<MealRecommendationDto> GetMealRecommendations(MealRecommendationSearchObject search)
        {
            IQueryable<MealRecommendation> q = _db.MealRecommendations
                .Include(x => x.FoodType);

            if (search.WeekNumber is not null)
            {
                q = q.Where(x => x.WeekNumber == search.WeekNumber.Value);
            }

            var totalCount = q.Count();

            var items = q
                .OrderBy(x => x.WeekNumber)
                .ThenBy(x => x.FoodType.Name)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(x => new MealRecommendationDto
                {
                    Id = x.Id,
                    WeekNumber = x.WeekNumber,
                    FoodTypeId = x.FoodTypeId,
                    FoodName = x.FoodType.Name
                })
                .ToList();

            return new PagedResult<MealRecommendationDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public MealRecommendationDto GetRecommendationById(long id)
        {
            var entity = _db.MealRecommendations
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Recommendation not found.");
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
                FoodName = x.FoodType!.Name,
                Rating = x.Rating,
                TriedAt = x.TriedAt
            };
        }

        public List<FoodTypeDto> GetFoodTypesWithoutRecommendation()
        {
            var recommendedIds = _db.MealRecommendations
                .Select(x => x.FoodTypeId)
                .Distinct();

            return _db.FoodTypes
                .Where(x => !recommendedIds.Contains(x.Id))
                .Select(x => new FoodTypeDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToList();
        }
        public MealRecommendationDto CreateRecommendation(CreateMealRecommendationDto dto)
        {
            if (!_db.FoodTypes.Any(x => x.Id == dto.FoodTypeId))
            {
                throw new NotFoundException("Food type not found.");
            }

            var entity = new MealRecommendation
            {
                WeekNumber = dto.WeekNumber,
                FoodTypeId = dto.FoodTypeId
            };

            _db.MealRecommendations.Add(entity);
            _db.SaveChanges();

            var created = _db.MealRecommendations
                .Include(x => x.FoodType)
                .First(x => x.Id == entity.Id);

            return new MealRecommendationDto
            {
                Id = created.Id,
                WeekNumber = created.WeekNumber,
                FoodTypeId = created.FoodTypeId,
                FoodName = created.FoodType.Name
            };
        }
        public MealRecommendationDto? UpdateRecommendation(long id, CreateMealRecommendationDto dto)
        {
            var entity = _db.MealRecommendations
                .Include(x => x.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Recommendation not found.");
            }

            if (!_db.FoodTypes.Any(x => x.Id == dto.FoodTypeId))
            {
                throw new NotFoundException("Food type not found.");
            }

            var existing = _db.MealRecommendations
                .FirstOrDefault(x => x.FoodTypeId == dto.FoodTypeId && x.Id != id);

            if (existing != null)
            {
                _db.MealRecommendations.Remove(existing);
            }

            entity.WeekNumber = dto.WeekNumber;
            entity.FoodTypeId = dto.FoodTypeId;

            _db.SaveChanges();

            var updated = _db.MealRecommendations
                .Include(x => x.FoodType)
                .First(x => x.Id == id);

            return new MealRecommendationDto
            {
                Id = updated.Id,
                WeekNumber = updated.WeekNumber,
                FoodTypeId = updated.FoodTypeId,
                FoodName = updated.FoodType.Name
            };
        }

        public void DeleteRecommendation(long id)
        {
            var entity = _db.MealRecommendations.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Recommendation not found.");
            }

            _db.MealRecommendations.Remove(entity);
            _db.SaveChanges();
        }
    }
}
