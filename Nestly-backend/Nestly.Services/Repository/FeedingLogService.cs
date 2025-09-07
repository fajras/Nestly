using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class FeedingLogService : IFeedingLogService
    {
        private readonly NestlyDbContext _db;

        public FeedingLogService(NestlyDbContext db)
        {
            _db = db;
        }

        public List<FeedingLog> Get(FeedingLogSearchObject? search)
        {
            IQueryable<FeedingLog> q = _db.FeedingLogs
                                          .Include(f => f.Baby)
                                          .Include(f => f.FoodType)
                                          .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.FeedDate >= search.DateFrom.Value);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.FeedDate <= search.DateTo.Value);
            }

            return q.ToList();
        }

        public FeedingLog? GetById(long id)
        {
            return _db.FeedingLogs
                      .Include(f => f.Baby)
                      .Include(f => f.FoodType)
                      .FirstOrDefault(x => x.Id == id);
        }

        public FeedingLog Create(CreateFeedingLogDto dto)
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

            if (dto.FeedDate == default)
            {
                throw new ArgumentException("FeedDate is required.", nameof(dto.FeedDate));
            }

            if (dto.AmountMl is < 0)
            {
                throw new ArgumentException("AmountMl cannot be negative.", nameof(dto.AmountMl));
            }

            if (dto.FoodTypeId.HasValue)
            {
                var foodTypeExists = _db.FoodTypes.Any(f => f.Id == dto.FoodTypeId.Value);
                if (!foodTypeExists)
                {
                    throw new ArgumentException("FoodType does not exist.", nameof(dto.FoodTypeId));
                }
            }

            var entity = new FeedingLog
            {
                BabyId = dto.BabyId,
                FeedDate = dto.FeedDate.Date,
                FeedTime = dto.FeedTime,
                AmountMl = dto.AmountMl,
                FoodTypeId = dto.FoodTypeId,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim()
            };

            _db.FeedingLogs.Add(entity);
            _db.SaveChanges();

            return entity;
        }

        public FeedingLog? Patch(long id, FeedingLogPatchDto patch)
        {
            var dbEntity = _db.FeedingLogs.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.FeedDate is not null)
            {
                dbEntity.FeedDate = patch.FeedDate.Value;
            }

            if (patch.FeedTime is not null)
            {
                dbEntity.FeedTime = patch.FeedTime.Value;
            }

            if (patch.AmountMl is not null)
            {
                dbEntity.AmountMl = patch.AmountMl.Value;
            }

            if (patch.FoodTypeId is not null)
            {
                if (!_db.FoodTypes.Any(f => f.Id == patch.FoodTypeId.Value))
                {
                    throw new ArgumentException("FoodType does not exist.");
                }

                dbEntity.FoodTypeId = patch.FoodTypeId.Value;
            }

            if (patch.Notes is not null)
            {
                dbEntity.Notes = patch.Notes;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.FeedingLogs.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.FeedingLogs.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }
}
