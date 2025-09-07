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
        public MealPlanService(NestlyDbContext db) => _db = db;

        public List<MealPlan> Get(MealPlanSearchObject? search)
        {
            IQueryable<MealPlan> q = _db.MealPlans
                                        .Include(x => x.Baby)
                                        .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.WeekNumber is not null)
            {
                q = q.Where(x => x.WeekNumber == search.WeekNumber);
            }

            if (!string.IsNullOrWhiteSpace(search?.FoodItem))
            {
                q = q.Where(x => x.FoodItem.Contains(search.FoodItem));
            }

            return q.OrderBy(x => x.WeekNumber).ToList();
        }

        public MealPlan? GetById(long id)
        {
            return _db.MealPlans
                      .Include(x => x.Baby)
                      .FirstOrDefault(x => x.Id == id);
        }

        public MealPlan Create(CreateMealPlanDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.", nameof(dto.BabyId));
            }

            var babyExists = _db.BabyProfiles.Any(b => b.Id == dto.BabyId);
            if (!babyExists)
            {
                throw new ArgumentException("Baby does not exist.", nameof(dto.BabyId));
            }

            if (dto.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.", nameof(dto.WeekNumber));
            }

            if (string.IsNullOrWhiteSpace(dto.FoodItem))
            {
                throw new ArgumentException("FoodItem is required.", nameof(dto.FoodItem));
            }

            if (dto.FoodRating is < 0)
            {
                throw new ArgumentException("FoodRating cannot be negative.", nameof(dto.FoodRating));
            }

            bool exists = _db.MealPlans.Any(m => m.BabyId == dto.BabyId && m.WeekNumber == dto.WeekNumber);
            if (exists)
            {
                throw new InvalidOperationException($"Meal plan for baby {dto.BabyId} and week {dto.WeekNumber} already exists.");
            }

            var entity = new MealPlan
            {
                BabyId = dto.BabyId,
                WeekNumber = dto.WeekNumber,
                FoodItem = dto.FoodItem.Trim(),
                FoodRating = dto.FoodRating
            };

            _db.MealPlans.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public MealPlan? Patch(long id, MealPlanPatchDto patch)
        {
            var dbEntity = _db.MealPlans.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.WeekNumber is not null)
            {
                dbEntity.WeekNumber = patch.WeekNumber.Value;
            }

            if (patch.FoodItem is not null)
            {
                dbEntity.FoodItem = patch.FoodItem;
            }

            if (patch.FoodRating is not null)
            {
                dbEntity.FoodRating = patch.FoodRating;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.MealPlans.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.MealPlans.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }
}
