using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class SleepLogService : ISleepLogService
    {
        private readonly NestlyDbContext _db;

        public SleepLogService(NestlyDbContext db)
        {
            _db = db;
        }

        public List<SleepLog> Get(SleepLogSearchObject? search)
        {
            IQueryable<SleepLog> q = _db.SleepLogs.AsNoTracking();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.SleepDate >= search.DateFrom.Value.Date);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.SleepDate <= search.DateTo.Value.Date);
            }

            return q.OrderByDescending(x => x.SleepDate)
                    .ThenByDescending(x => x.StartTime)
                    .ToList();
        }

        public SleepLog? GetById(long id)
        {
            return _db.SleepLogs.AsNoTracking()
                                .FirstOrDefault(x => x.Id == id);
        }

        public SleepLog Create(CreateSleepLogDto dto)
        {
            if (dto.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new ArgumentException("Baby does not exist.");
            }

            if (!TimeSpan.TryParse(dto.StartTime, out var start))
            {
                throw new ArgumentException("Invalid StartTime format. Use HH:mm");
            }

            if (!TimeSpan.TryParse(dto.EndTime, out var end))
            {
                throw new ArgumentException("Invalid EndTime format. Use HH:mm");
            }

            var entity = new SleepLog
            {
                BabyId = dto.BabyId,
                SleepDate = dto.SleepDate.Date,
                StartTime = start,
                EndTime = end
            };

            _db.SleepLogs.Add(entity);
            _db.SaveChanges();

            return entity;
        }

        public SleepLog? Patch(long id, SleepLogPatchDto patch)
        {
            var entity = _db.SleepLogs.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return null;
            }

            if (patch.SleepDate is not null)
            {
                entity.SleepDate = patch.SleepDate.Value.Date;
            }

            if (patch.StartTime is not null)
            {
                if (!TimeSpan.TryParse(patch.StartTime, out var start))
                {
                    throw new ArgumentException("Invalid StartTime format. Use HH:mm");
                }

                entity.StartTime = start;
            }

            if (patch.EndTime is not null)
            {
                if (!TimeSpan.TryParse(patch.EndTime, out var end))
                {
                    throw new ArgumentException("Invalid EndTime format. Use HH:mm");
                }

                entity.EndTime = end;
            }

            _db.SaveChanges();
            return entity;
        }

        public bool Delete(long id)
        {
            var entity = _db.SleepLogs.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return false;
            }

            _db.SleepLogs.Remove(entity);
            _db.SaveChanges();
            return true;
        }
    }
}
