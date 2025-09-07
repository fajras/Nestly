using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class DiaperLogService : IDiaperLogService
    {
        private readonly NestlyDbContext _db;

        public DiaperLogService(NestlyDbContext db) => _db = db;

        public List<DiaperLog> Get(DiaperLogSearchObject? search)
        {
            IQueryable<DiaperLog> q = _db.DiaperLogs
                                         .Include(x => x.Baby)
                                         .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.ChangeDate >= search.DateFrom.Value);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.ChangeDate <= search.DateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(search?.DiaperState))
            {
                q = q.Where(x => x.DiaperState == search.DiaperState);
            }

            // možda je zgodno sortirati po datumu i vremenu
            q = q.OrderByDescending(x => x.ChangeDate).ThenByDescending(x => x.ChangeTime);

            return q.ToList();
        }

        public DiaperLog? GetById(long id)
        {
            return _db.DiaperLogs
                      .Include(x => x.Baby)
                      .FirstOrDefault(x => x.Id == id);
        }

        public DiaperLog Create(DiaperLog entity)
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

            _db.DiaperLogs.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public DiaperLog? Patch(long id, DiaperLogPatchDto patch)
        {
            var dbEntity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.ChangeDate is not null)
            {
                dbEntity.ChangeDate = patch.ChangeDate.Value;
            }

            if (patch.ChangeTime is not null)
            {
                dbEntity.ChangeTime = patch.ChangeTime.Value;
            }

            if (patch.DiaperState is not null)
            {
                dbEntity.DiaperState = patch.DiaperState;
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
            var dbEntity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.DiaperLogs.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }
}
