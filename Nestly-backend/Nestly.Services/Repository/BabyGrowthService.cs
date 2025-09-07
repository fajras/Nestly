using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
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
        public BabyGrowth Create(CreateBabyGrowthDto dto)
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

            if (dto.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.", nameof(dto.WeekNumber));
            }

            bool exists = _db.BabyGrowths.Any(g => g.BabyId == dto.BabyId && g.WeekNumber == dto.WeekNumber);
            if (exists)
            {
                throw new InvalidOperationException($"Growth entry for baby {dto.BabyId} and week {dto.WeekNumber} already exists.");
            }

            var entity = new BabyGrowth
            {
                BabyId = dto.BabyId,
                WeekNumber = dto.WeekNumber,
                WeightKg = dto.WeightKg,
                HeightCm = dto.HeightCm,
                HeadCircumferenceCm = dto.HeadCircumferenceCm
            };

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
