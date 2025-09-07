using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class HealthEntryService : IHealthEntryService
    {
        private readonly NestlyDbContext _db;
        public HealthEntryService(NestlyDbContext db) => _db = db;

        public List<HealthEntry> Get(HealthEntrySearchObject? search)
        {
            IQueryable<HealthEntry> q = _db.HealthEntries
                                           .Include(x => x.Baby)
                                           .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.EntryDate >= search.DateFrom.Value);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.EntryDate <= search.DateTo.Value);
            }

            return q.OrderByDescending(x => x.EntryDate).ToList();
        }

        public HealthEntry? GetById(long id)
        {
            return _db.HealthEntries
                      .Include(x => x.Baby)
                      .FirstOrDefault(x => x.Id == id);
        }

        public HealthEntry Create(HealthEntry entity)
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

            _db.HealthEntries.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public HealthEntry? Patch(long id, HealthEntryPatchDto patch)
        {
            var dbEntity = _db.HealthEntries.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.EntryDate is not null)
            {
                dbEntity.EntryDate = patch.EntryDate.Value;
            }

            if (patch.TemperatureC is not null)
            {
                dbEntity.TemperatureC = patch.TemperatureC.Value;
            }

            if (patch.Medicines is not null)
            {
                dbEntity.Medicines = patch.Medicines;
            }

            if (patch.DoctorVisit is not null)
            {
                dbEntity.DoctorVisit = patch.DoctorVisit;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.HealthEntries.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.HealthEntries.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }

    }
}
