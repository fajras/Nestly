using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Extensions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class FeedingLogService : IFeedingLogService
    {
        private const decimal MaxAmountMl = 1000m;

        private readonly NestlyDbContext _db;
        private readonly IBabyHealthMonitoringService _healthMonitoring;

        public FeedingLogService(NestlyDbContext db, IBabyHealthMonitoringService healthMonitoring)
        {
            _db = db;
            _healthMonitoring = healthMonitoring;
        }

        public async Task<PagedResult<FeedingLogResponseDto>> Get(FeedingLogSearchObject search)
        {
            IQueryable<FeedingLog> q = _db.FeedingLogs
                .Include(f => f.FoodType)
                .AsQueryable();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x => x.FeedDate >= search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x => x.FeedDate <= search.DateTo.Value);
            }

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await q
                .OrderByDescending(x => x.FeedDate)
                .ThenByDescending(x => x.FeedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<FeedingLogResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<FeedingLogResponseDto> GetById(long id)
        {
            var entity = await _db.FeedingLogs
                .Include(f => f.FoodType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Feeding log not found.");
            }

            return MapToDto(entity);
        }

        public async Task<FeedingLogResponseDto> Create(CreateFeedingLogDto dto)
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

            if (dto.FeedDate == default)
            {
                throw new BusinessException("Feed date is required.");
            }

            if (DateValidation.IsFutureDate(dto.FeedDate))
            {
                throw new BusinessException("Feed date cannot be in the future.");
            }

            ValidateAmount(dto.AmountMl);
            var amountUnit = NormalizeUnit(dto.AmountUnit);

            if (dto.FoodTypeId.HasValue &&
                !await _db.FoodTypes.AnyAsync(f => f.Id == dto.FoodTypeId.Value))
            {
                throw new NotFoundException("Food type not found.");
            }

            var entity = new FeedingLog
            {
                BabyId = dto.BabyId,
                FeedDate = dto.FeedDate.Date,
                FeedTime = dto.FeedTime,
                AmountMl = dto.AmountMl,
                AmountUnit = amountUnit,
                FoodTypeId = dto.FoodTypeId,
                Notes = string.IsNullOrWhiteSpace(dto.Notes)
                    ? null
                    : dto.Notes.Trim()
            };

            _db.FeedingLogs.Add(entity);
            await _db.SaveChangesAsync();

            await _healthMonitoring.RunCheckForBabyAsync(dto.BabyId);

            return MapToDto(entity);
        }
        public async Task<FeedingLogResponseDto> Patch(long id, FeedingLogPatchDto patch)
        {
            var dbEntity = await _db.FeedingLogs
                .Include(f => f.FoodType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Feeding log not found.");
            }

            if (patch.FeedDate is not null)
            {
                if (DateValidation.IsFutureDate(patch.FeedDate.Value))
                {
                    throw new BusinessException("Feed date cannot be in the future.");
                }

                dbEntity.FeedDate = patch.FeedDate.Value.Date;
            }

            if (patch.FeedTime is not null)
            {
                dbEntity.FeedTime = patch.FeedTime.Value;
            }

            if (patch.AmountMl is not null)
            {
                ValidateAmount(patch.AmountMl.Value);

                dbEntity.AmountMl = patch.AmountMl.Value;
            }

            if (patch.AmountUnit is not null)
            {
                dbEntity.AmountUnit = NormalizeUnit(patch.AmountUnit);
            }

            if (patch.FoodTypeId is not null)
            {
                if (!await _db.FoodTypes.AnyAsync(f => f.Id == patch.FoodTypeId.Value))
                {
                    throw new NotFoundException("Food type not found.");
                }

                dbEntity.FoodTypeId = patch.FoodTypeId.Value;
            }

            if (patch.Notes is not null)
            {
                dbEntity.Notes = string.IsNullOrWhiteSpace(patch.Notes)
                    ? null
                    : patch.Notes.Trim();
            }

            await _db.SaveChangesAsync();

            return MapToDto(dbEntity);
        }
        public async Task Delete(long id)
        {
            var dbEntity = await _db.FeedingLogs.FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Feeding log not found.");
            }

            _db.FeedingLogs.Remove(dbEntity);
            await _db.SaveChangesAsync();
        }

        private static void ValidateAmount(decimal? amountMl)
        {
            if (amountMl is null)
            {
                return;
            }

            if (amountMl <= 0)
            {
                throw new BusinessException("Amount must be greater than 0.");
            }

            if (amountMl > MaxAmountMl)
            {
                throw new BusinessException($"Amount cannot exceed {MaxAmountMl} ml.");
            }
        }

        private static string NormalizeUnit(string? unit)
        {
            var trimmed = unit?.Trim().ToLowerInvariant();

            return trimmed switch
            {
                "g" => "g",
                "ml" or null or "" => "ml",
                _ => throw new BusinessException("Amount unit must be either 'ml' or 'g'.")
            };
        }

        private static FeedingLogResponseDto MapToDto(FeedingLog x)
        {
            return new FeedingLogResponseDto
            {
                Id = x.Id,
                BabyId = x.BabyId,
                FeedDate = x.FeedDate,
                FeedTime = x.FeedTime,
                AmountMl = x.AmountMl,
                AmountUnit = x.AmountUnit,
                FoodTypeId = x.FoodTypeId,
                FoodTypeName = x.FoodType != null ? x.FoodType.Name : null,
                Notes = x.Notes
            };
        }

        public async Task<PagedResult<FeedingLogResponseDto>> GetByParent(
    long parentProfileId,
    FeedingLogSearchObject search)
        {
            IQueryable<FeedingLog> q = _db.FeedingLogs
                .Include(f => f.FoodType)
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
                    x.FeedDate >=
                    search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x =>
                    x.FeedDate <=
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
                .OrderByDescending(x => x.FeedDate)
                .ThenByDescending(x => x.FeedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<FeedingLogResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
