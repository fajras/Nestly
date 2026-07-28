using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;


namespace Nestly.Services.Repository
{
    public class BabyGrowthService : IBabyGrowthService
    {
        private const decimal MinWeightKg = 0m;
        private const decimal MaxWeightKg = 60m;
        private const decimal MinHeightCm = 0m;
        private const decimal MaxHeightCm = 200m;
        private const decimal MinHeadCircumferenceCm = 0m;
        private const decimal MaxHeadCircumferenceCm = 70m;

        private readonly NestlyDbContext _db;
        private readonly IBabyHealthMonitoringService _healthMonitoring;

        public BabyGrowthService(NestlyDbContext db, IBabyHealthMonitoringService healthMonitoring)
        {
            _db = db;
            _healthMonitoring = healthMonitoring;
        }

        public async Task<PagedResult<BabyGrowthResponseDto>> Get(BabyGrowthSearchObject search)
        {
            IQueryable<BabyGrowth> q = _db.BabyGrowths.AsQueryable();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.WeekNumber is not null)
            {
                q = q.Where(x => x.WeekNumber == search.WeekNumber);
            }

            if (search.WeekFrom is not null)
            {
                q = q.Where(x => x.WeekNumber >= search.WeekFrom.Value);
            }

            if (search.WeekTo is not null)
            {
                q = q.Where(x => x.WeekNumber <= search.WeekTo.Value);
            }

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var growthEntities = await q
                .OrderBy(x => x.WeekNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = growthEntities.Select(MapToDto).ToList();

            return new PagedResult<BabyGrowthResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<BabyGrowthResponseDto> GetById(long id)
        {
            var entity = await _db.BabyGrowths.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Growth entry not found.");
            }

            return MapToDto(entity);
        }

        public async Task<BabyGrowthResponseDto> Create(CreateBabyGrowthDto dto)
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

            if (dto.WeekNumber <= 0)
            {
                throw new BusinessException("Week number must be greater than 0.");
            }

            ValidateMeasurements(dto.WeightKg, dto.HeightCm, dto.HeadCircumferenceCm);

            bool exists = await _db.BabyGrowths
                .AnyAsync(g => g.BabyId == dto.BabyId && g.WeekNumber == dto.WeekNumber);

            if (exists)
            {
                throw new BusinessException(
                    $"Growth entry for this week already exists.");
            }

            var entity = new BabyGrowth
            {
                BabyId = dto.BabyId,
                WeekNumber = dto.WeekNumber,
                WeightKg = dto.WeightKg,
                HeightCm = dto.HeightCm,
                HeadCircumferenceCm = dto.HeadCircumferenceCm
            };

            _db.BabyGrowths.Add(entity);
            await _db.SaveChangesAsync();

            await _healthMonitoring.RunCheckForBabyAsync(dto.BabyId);

            return MapToDto(entity);
        }
        public async Task<BabyGrowthResponseDto> Patch(long id, BabyGrowthPatchDto patch)
        {
            var dbEntity = await _db.BabyGrowths.FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Growth entry not found.");
            }

            ValidateMeasurements(
                patch.WeightKg ?? dbEntity.WeightKg,
                patch.HeightCm ?? dbEntity.HeightCm,
                patch.HeadCircumferenceCm ?? dbEntity.HeadCircumferenceCm);

            if (patch.WeightKg is not null)
            {
                dbEntity.WeightKg = patch.WeightKg.Value;
            }

            if (patch.HeightCm is not null)
            {
                dbEntity.HeightCm = patch.HeightCm.Value;
            }

            if (patch.HeadCircumferenceCm is not null)
            {
                dbEntity.HeadCircumferenceCm = patch.HeadCircumferenceCm.Value;
            }

            await _db.SaveChangesAsync();

            return MapToDto(dbEntity);
        }

        public async Task Delete(long id)
        {
            var dbEntity = await _db.BabyGrowths.FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Growth entry not found.");
            }

            _db.BabyGrowths.Remove(dbEntity);
            await _db.SaveChangesAsync();
        }

        private static void ValidateMeasurements(
            decimal? weightKg,
            decimal? heightCm,
            decimal? headCircumferenceCm)
        {
            if (weightKg is not null && (weightKg < MinWeightKg || weightKg > MaxWeightKg))
            {
                throw new BusinessException(
                    $"Weight must be between {MinWeightKg} and {MaxWeightKg} kg.");
            }

            if (heightCm is not null && (heightCm < MinHeightCm || heightCm > MaxHeightCm))
            {
                throw new BusinessException(
                    $"Height must be between {MinHeightCm} and {MaxHeightCm} cm.");
            }

            if (headCircumferenceCm is not null &&
                (headCircumferenceCm < MinHeadCircumferenceCm || headCircumferenceCm > MaxHeadCircumferenceCm))
            {
                throw new BusinessException(
                    $"Head circumference must be between {MinHeadCircumferenceCm} and {MaxHeadCircumferenceCm} cm.");
            }
        }

        private static BabyGrowthResponseDto MapToDto(BabyGrowth entity)
        {
            return new BabyGrowthResponseDto
            {
                Id = entity.Id,
                BabyId = entity.BabyId,
                WeekNumber = entity.WeekNumber,
                WeightKg = entity.WeightKg,
                HeightCm = entity.HeightCm,
                HeadCircumferenceCm = entity.HeadCircumferenceCm
            };
        }

        public async Task<PagedResult<BabyGrowthResponseDto>> GetByParent(
    long parentProfileId,
    BabyGrowthSearchObject search)
        {
            IQueryable<BabyGrowth> q =
                _db.BabyGrowths
                    .Where(x =>
                        x.Baby.ParentProfileId ==
                        parentProfileId);

            if (search.BabyId is not null)
            {
                q = q.Where(x =>
                    x.BabyId == search.BabyId);
            }

            if (search.WeekNumber is not null)
            {
                q = q.Where(x =>
                    x.WeekNumber == search.WeekNumber);
            }

            if (search.WeekFrom is not null)
            {
                q = q.Where(x =>
                    x.WeekNumber >= search.WeekFrom.Value);
            }

            if (search.WeekTo is not null)
            {
                q = q.Where(x =>
                    x.WeekNumber <= search.WeekTo.Value);
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

            var growthEntities = await q
                .OrderBy(x => x.WeekNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = growthEntities.Select(MapToDto).ToList();

            return new PagedResult<BabyGrowthResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
