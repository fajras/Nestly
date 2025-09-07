using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class SleepLogService : ISleepLogService
    {
        private readonly NestlyDbContext _db;

        public SleepLogService(NestlyDbContext db) => _db = db;

        public List<SleepLog> Get(SleepLogSearchObject? search)
        {
            IQueryable<SleepLog> q = _db.SleepLogs
                                        .Include(x => x.Baby)
                                        .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.SleepDate >= search.DateFrom.Value);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.SleepDate <= search.DateTo.Value);
            }

            return q.OrderByDescending(x => x.SleepDate).ToList();
        }

        public SleepLog? GetById(long id)
        {
            return _db.SleepLogs
                      .Include(x => x.Baby)
                      .FirstOrDefault(x => x.Id == id);
        }

        public SleepLog Create(SleepLog entity)
        {
            if (entity.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == entity.BabyId))
            {
                throw new ArgumentException("Baby does not exist.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            _db.SleepLogs.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public SleepLog? Patch(long id, SleepLogPatchDto patch)
        {
            var dbEntity = _db.SleepLogs.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.SleepDate is not null)
            {
                dbEntity.SleepDate = patch.SleepDate.Value;
            }

            if (patch.StartTime is not null)
            {
                dbEntity.StartTime = patch.StartTime.Value;
            }

            if (patch.EndTime is not null)
            {
                dbEntity.EndTime = patch.EndTime.Value;
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
            var dbEntity = _db.SleepLogs.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.SleepLogs.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }


}
