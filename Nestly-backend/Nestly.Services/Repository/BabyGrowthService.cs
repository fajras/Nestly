using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class BabyGrowthService : IBabyGrowthService
    {
        private readonly NestlyDbContext _db;
        public BabyGrowthService(NestlyDbContext db) => _db = db;

        public List<BabyGrowth> Get(BabyGrowthSearchObject? search)
        {
            IQueryable<BabyGrowth> q = _db.BabyGrowths
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

            if (search?.WeekFrom is not null)
            {
                q = q.Where(x => x.WeekNumber >= search.WeekFrom.Value);
            }

            if (search?.WeekTo is not null)
            {
                q = q.Where(x => x.WeekNumber <= search.WeekTo.Value);
            }

            return q.OrderBy(x => x.WeekNumber).ToList();
        }

        public BabyGrowth? GetById(long id)
        {
            return _db.BabyGrowths
                      .Include(x => x.Baby)
                      .FirstOrDefault(x => x.Id == id);
        }

        public BabyGrowth Create(BabyGrowth entity)
        {
            if (entity.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == entity.BabyId))
            {
                throw new ArgumentException("Baby does not exist.");
            }

            if (entity.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.");
            }

            _db.BabyGrowths.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public BabyGrowth? Patch(long id, BabyGrowthPatchDto patch)
        {
            var dbEntity = _db.BabyGrowths.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.WeekNumber is not null)
            {
                if (patch.WeekNumber.Value <= 0)
                {
                    throw new ArgumentException("WeekNumber must be > 0.");
                }

                dbEntity.WeekNumber = patch.WeekNumber.Value;
            }

            if (patch.WeightKg is not null)
            {
                dbEntity.WeightKg = patch.WeightKg.Value;
            }

            if (patch.HeightCm is not null)
            {
                dbEntity.HeightCm = patch.HeightCm.Value;
            }

            if (patch.HeadCircumferenceCm is not null)
            {
                dbEntity.HeadCircumferenceCm = patch.HeadCircumferenceCm.Value;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.BabyGrowths.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.BabyGrowths.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }


}
