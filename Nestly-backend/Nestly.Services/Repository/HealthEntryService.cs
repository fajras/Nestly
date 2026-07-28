using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Extensions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class HealthEntryService : IHealthEntryService
    {
        private const decimal MinTemperatureC = 30m;
        private const decimal MaxTemperatureC = 45m;

        private readonly NestlyDbContext _db;

        public HealthEntryService(NestlyDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<HealthEntryResponseDto>> Get(HealthEntrySearchObject search)
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

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await q
                .OrderByDescending(x => x.EntryDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<HealthEntryResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<HealthEntryResponseDto> GetById(long id)
        {
            var entity = await _db.HealthEntries
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health entry not found.");
            }

            return MapToDto(entity);
        }

        public async Task<HealthEntryResponseDto> Create(CreateHealthEntryDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (dto.BabyId <= 0)
            {
                throw new BusinessException("Baby is required.");
            }

            if (!await _db.BabyProfiles.AnyAsync(b => b.Id == dto.BabyId))
            {
                throw new NotFoundException("Baby profile not found.");
            }

            if (dto.EntryDate == default)
            {
                throw new BusinessException("Entry date is required.");
            }

            if (DateValidation.IsFutureDateTime(dto.EntryDate))
            {
                throw new BusinessException("Entry date cannot be in the future.");
            }

            ValidateTemperature(dto.TemperatureC);

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
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<HealthEntryResponseDto> Patch(long id, HealthEntryPatchDto patch)
        {
            var entity = await _db.HealthEntries.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health entry not found.");
            }

            if (patch.EntryDate is not null)
            {
                if (DateValidation.IsFutureDateTime(patch.EntryDate.Value))
                {
                    throw new BusinessException("Entry date cannot be in the future.");
                }

                entity.EntryDate = patch.EntryDate.Value;
            }

            if (patch.TemperatureC is not null)
            {
                ValidateTemperature(patch.TemperatureC);
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

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _db.HealthEntries.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health entry not found.");
            }

            _db.HealthEntries.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private static void ValidateTemperature(decimal? temperatureC)
        {
            if (temperatureC is not null &&
                (temperatureC < MinTemperatureC || temperatureC > MaxTemperatureC))
            {
                throw new BusinessException(
                    $"Temperature must be between {MinTemperatureC} and {MaxTemperatureC} °C.");
            }
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

        public async Task<PagedResult<HealthEntryResponseDto>> GetByParent(
    long parentProfileId,
    HealthEntrySearchObject search)
        {
            IQueryable<HealthEntry> q = _db.HealthEntries
                .Where(x =>
                    x.Baby.ParentProfileId ==
                    parentProfileId)
                .AsQueryable();

            if (search.BabyId is not null)
            {
                q = q.Where(x =>
                    x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x =>
                    x.EntryDate >=
                    search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x =>
                    x.EntryDate <=
                    search.DateTo.Value);
            }

            var totalCount = await q.CountAsync();

            int page = search.Page < 1
                ? 1
                : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var entities = await q
                .OrderByDescending(x => x.EntryDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<HealthEntryResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
