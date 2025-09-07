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

        public SleepLog Create(CreateSleepLogDto dto)
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

            if (dto.SleepDate == default)
            {
                throw new ArgumentException("SleepDate is required.", nameof(dto.SleepDate));
            }

            if (dto.StartTime == default)
            {
                throw new ArgumentException("StartTime is required.", nameof(dto.StartTime));
            }

            if (dto.EndTime == default)
            {
                throw new ArgumentException("EndTime is required.", nameof(dto.EndTime));
            }

            if (dto.EndTime <= dto.StartTime)
            {
                throw new ArgumentException("EndTime must be after StartTime.", nameof(dto.EndTime));
            }

            var notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
            if (notes is not null && notes.Length > 1000)
            {
                throw new ArgumentException("Notes must be at most 1000 characters.", nameof(dto.Notes));
            }

            var entity = new SleepLog
            {
                BabyId = dto.BabyId,
                SleepDate = dto.SleepDate.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Notes = notes
            };

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
