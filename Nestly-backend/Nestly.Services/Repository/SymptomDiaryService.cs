using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Extensions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class SymptomDiaryService : ISymptomDiaryService
    {
        private readonly NestlyDbContext _db;
        private readonly ICurrentUserService _currentUserService;

        public SymptomDiaryService(
    NestlyDbContext db,
    ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        public async Task<PagedResult<SymptomDiaryResponseDto>> Get(SymptomDiarySearchObject search)
        {
            IQueryable<SymptomDiary> q = _db.SymptomDiaries.AsNoTracking();


            if (search.DateFrom is not null)
            {
                q = q.Where(s => s.Date >= search.DateFrom.Value.Date);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(s => s.Date <= search.DateTo.Value.Date);
            }

            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var totalCount = await q.CountAsync();

            var entities = await q
                .OrderByDescending(s => s.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(ToDto).ToList();

            return new PagedResult<SymptomDiaryResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
        private static SymptomDiaryResponseDto ToDto(SymptomDiary s) => new()
        {
            Id = s.Id,
            Date = s.Date,
            Nausea = s.Nausea,
            Fatigue = s.Fatigue,
            Headache = s.Headache,
            Heartburn = s.Heartburn,
            LegSwelling = s.LegSwelling
        };

        public async Task<SymptomDiaryResponseDto> Create(CreateSymptomDiaryDto dto)
        {
            var parent = _currentUserService
                .GetCurrentParentProfile();

            if (parent == null)
            {
                throw new NotFoundException(
                    "Parent profile not found.");
            }

            var date = (dto.Date ?? DateTime.Today).Date;

            if (DateValidation.IsFutureDate(date))
            {
                throw new BusinessException(
                    "Diary date cannot be in the future.");
            }

            if (await _db.SymptomDiaries.AnyAsync(s =>
                s.ParentProfileId == parent.Id &&
                s.Date == date))
            {
                throw new BusinessException(
                    "Diary entry already exists for this date.");
            }

            ValidateRange(dto.Nausea, nameof(dto.Nausea));
            ValidateRange(dto.Fatigue, nameof(dto.Fatigue));
            ValidateRange(dto.Headache, nameof(dto.Headache));
            ValidateRange(dto.Heartburn, nameof(dto.Heartburn));
            ValidateRange(dto.LegSwelling, nameof(dto.LegSwelling));

            var entity = new SymptomDiary
            {
                ParentProfileId = parent.Id,
                Date = date,
                Nausea = dto.Nausea,
                Fatigue = dto.Fatigue,
                Headache = dto.Headache,
                Heartburn = dto.Heartburn,
                LegSwelling = dto.LegSwelling
            };

            _db.SymptomDiaries.Add(entity);

            await _db.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<PagedResult<SymptomDiaryResponseDto>> GetByParent(long parentProfileId, SymptomDiarySearchObject search)
        {
            var query = _db.SymptomDiaries
                .AsNoTracking()
                .Where(s => s.ParentProfileId == parentProfileId);

            if (search.DateFrom is not null)
            {
                query = query.Where(s => s.Date >= search.DateFrom.Value.Date);
            }

            if (search.DateTo is not null)
            {
                query = query.Where(s => s.Date <= search.DateTo.Value.Date);
            }

            var totalCount = await query.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await query
                .OrderByDescending(s => s.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(ToDto).ToList();

            return new PagedResult<SymptomDiaryResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<SymptomDiaryResponseDto> GetByDate(long parentProfileId, DateTime date)
        {
            var entity = await _db.SymptomDiaries
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.ParentProfileId == parentProfileId &&
                    s.Date == date.Date);

            if (entity == null)
            {
                throw new NotFoundException("Symptom diary entry not found.");
            }

            return ToDto(entity);
        }

        public async Task<SymptomDiaryResponseDto?> Patch(long id, SymptomDiaryPatchDto patch)
        {
            var entity = await _db.SymptomDiaries.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
            {
                throw new NotFoundException("Symptom diary entry not found.");
            }

            if (patch.Nausea.HasValue)
            {
                ValidateRange(patch.Nausea.Value, nameof(patch.Nausea));
                entity.Nausea = patch.Nausea.Value;
            }

            if (patch.Fatigue.HasValue)
            {
                ValidateRange(patch.Fatigue.Value, nameof(patch.Fatigue));
                entity.Fatigue = patch.Fatigue.Value;
            }

            if (patch.Headache.HasValue)
            {
                ValidateRange(patch.Headache.Value, nameof(patch.Headache));
                entity.Headache = patch.Headache.Value;
            }

            if (patch.Heartburn.HasValue)
            {
                ValidateRange(patch.Heartburn.Value, nameof(patch.Heartburn));
                entity.Heartburn = patch.Heartburn.Value;
            }

            if (patch.LegSwelling.HasValue)
            {
                ValidateRange(patch.LegSwelling.Value, nameof(patch.LegSwelling));
                entity.LegSwelling = patch.LegSwelling.Value;
            }

            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _db.SymptomDiaries.FirstOrDefaultAsync(s => s.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Symptom diary entry not found.");
            }

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<DateTime>> GetMarkedDays(long parentProfileId, SymptomDiarySearchObject search)
        {
            var query = _db.SymptomDiaries
                .AsNoTracking()
                .Where(s => s.ParentProfileId == parentProfileId)
                .Select(s => s.Date)
                .Distinct();

            var totalCount = await query.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = await query
                .OrderByDescending(d => d)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<DateTime>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        private static void ValidateRange(int? value, string field)
        {
            if (value.HasValue && (value.Value < 1 || value.Value > 5))
            {
                throw new BusinessException($"{field} must be between 1 and 5.");
            }
        }


    }
}
