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
                                        .Include(x => x.FoodType);

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
                q = q.Where(x => x.TriedAt >= search.From);
            }

            if (search?.To is not null)
            {
                q = q.Where(x => x.TriedAt <= search.To);
            }

            return q.OrderByDescending(x => x.TriedAt).ToList();
        }

        public MealPlan? GetById(long id)
        {
            return _db.MealPlans
                      .Include(x => x.Baby)
                      .Include(x => x.FoodType)
                      .FirstOrDefault(x => x.Id == id);
        }

        public CreateFetalDevelopmentWeekDto? GetByWeekNumber(int weekNumber)
        {
            return _db.FetalDevelopmentWeeks
                .Where(f => f.WeekNumber == weekNumber)
                .Select(f => new CreateFetalDevelopmentWeekDto
                {
                    WeekNumber = f.WeekNumber,
                    ImageUrl = f.ImageUrl,
                    BabyDevelopment = f.BabyDevelopment,
                    MotherChanges = f.MotherChanges
                })
                .FirstOrDefault();
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

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new ArgumentException("Baby does not exist.", nameof(dto.BabyId));
            }

            if (!_db.FoodTypes.Any(f => f.Id == dto.FoodTypeId))
            {
                throw new ArgumentException("FoodType does not exist.", nameof(dto.FoodTypeId));
            }

            if (dto.Rating is < 0 or > 5)
            {
                throw new ArgumentException("Rating must be between 0 and 5.", nameof(dto.Rating));
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
            return entity;
        }

        public MealPlan? Patch(long id, MealPlanPatchDto patch)
        {
            var dbEntity = _db.MealPlans.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.Rating is not null)
            {
                if (patch.Rating is < 0 or > 5)
                {
                    throw new ArgumentException("Rating must be between 0 and 5.", nameof(patch.Rating));
                }

                dbEntity.Rating = patch.Rating;
            }

            if (patch.TriedAt is not null)
            {
                dbEntity.TriedAt = patch.TriedAt.Value;
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
