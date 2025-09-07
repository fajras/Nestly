using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
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

        public HealthEntry Create(CreateHealthEntryDto dto)
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

            if (dto.EntryDate == default)
            {
                throw new ArgumentException("EntryDate is required.", nameof(dto.EntryDate));
            }

            var entity = new HealthEntry
            {
                BabyId = dto.BabyId,
                EntryDate = dto.EntryDate,
                TemperatureC = dto.TemperatureC,
                Medicines = string.IsNullOrWhiteSpace(dto.Medicines) ? null : dto.Medicines.Trim(),
                DoctorVisit = string.IsNullOrWhiteSpace(dto.DoctorVisit) ? null : dto.DoctorVisit.Trim()
            };

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
