using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class HealthEntryService : IHealthEntryService
    {
        private readonly NestlyDbContext _db;

        public HealthEntryService(NestlyDbContext db)
        {
            _db = db;
        }

        public PagedResult<HealthEntryResponseDto> Get(HealthEntrySearchObject search)
        {
            IQueryable<HealthEntry> q = _db.HealthEntries.AsQueryable();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x => x.EntryDate >= search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x => x.EntryDate <= search.DateTo.Value);
            }

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(x => x.EntryDate)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResult<HealthEntryResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public HealthEntryResponseDto GetById(long id)
        {
            var entity = _db.HealthEntries
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health entry not found.");
            }

            return MapToDto(entity);
        }

        public HealthEntryResponseDto Create(CreateHealthEntryDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (dto.BabyId <= 0)
            {
                throw new BusinessException("Baby is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new NotFoundException("Baby profile not found.");
            }

            if (dto.EntryDate == default)
            {
                throw new BusinessException("Entry date is required.");
            }

            var entity = new HealthEntry
            {
                BabyId = dto.BabyId,
                EntryDate = dto.EntryDate,
                TemperatureC = dto.TemperatureC,
                Medicines = string.IsNullOrWhiteSpace(dto.Medicines)
                    ? null
                    : dto.Medicines.Trim(),
                DoctorVisit = string.IsNullOrWhiteSpace(dto.DoctorVisit)
                    ? null
                    : dto.DoctorVisit.Trim()
            };

            _db.HealthEntries.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public HealthEntryResponseDto Patch(long id, HealthEntryPatchDto patch)
        {
            var entity = _db.HealthEntries.FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health entry not found.");
            }

            if (patch.EntryDate is not null)
            {
                entity.EntryDate = patch.EntryDate.Value;
            }

            if (patch.TemperatureC is not null)
            {
                entity.TemperatureC = patch.TemperatureC.Value;
            }

            if (patch.Medicines is not null)
            {
                entity.Medicines = string.IsNullOrWhiteSpace(patch.Medicines)
                    ? null
                    : patch.Medicines.Trim();
            }

            if (patch.DoctorVisit is not null)
            {
                entity.DoctorVisit = string.IsNullOrWhiteSpace(patch.DoctorVisit)
                    ? null
                    : patch.DoctorVisit.Trim();
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public void Delete(long id)
        {
            var entity = _db.HealthEntries.FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health entry not found.");
            }

            _db.HealthEntries.Remove(entity);
            _db.SaveChanges();
        }

        private static HealthEntryResponseDto MapToDto(HealthEntry x)
        {
            return new HealthEntryResponseDto
            {
                Id = x.Id,
                BabyId = x.BabyId,
                EntryDate = x.EntryDate,
                TemperatureC = x.TemperatureC,
                Medicines = x.Medicines,
                DoctorVisit = x.DoctorVisit
            };
        }
    }
}
