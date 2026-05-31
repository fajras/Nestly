using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
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
        public PagedResult<SymptomDiaryResponseDto> Get(SymptomDiarySearchObject search)
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

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(s => s.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToDto)
                .ToList();

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

        public SymptomDiaryResponseDto Create(CreateSymptomDiaryDto dto)
        {
            var parent = _currentUserService
                .GetCurrentParentProfile();

            if (parent == null)
            {
                throw new NotFoundException(
                    "Parent profile not found.");
            }

            var date = (dto.Date ?? DateTime.Today).Date;

            if (_db.SymptomDiaries.Any(s =>
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

            _db.SaveChanges();

            return ToDto(entity);
        }

        public PagedResult<SymptomDiaryResponseDto> GetByParent(long parentProfileId, SymptomDiarySearchObject search)
        {
            var query = _db.SymptomDiaries
                .AsNoTracking()
                .Where(s => s.ParentProfileId == parentProfileId);

            var totalCount = query.Count();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = query
                .OrderByDescending(s => s.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToDto)
                .ToList();

            return new PagedResult<SymptomDiaryResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public SymptomDiaryResponseDto GetByDate(long parentProfileId, DateTime date)
        {
            var entity = _db.SymptomDiaries
                .AsNoTracking()
                .FirstOrDefault(s =>
                    s.ParentProfileId == parentProfileId &&
                    s.Date == date.Date);

            if (entity == null)
            {
                throw new NotFoundException("Symptom diary entry not found.");
            }

            return ToDto(entity);
        }

        public SymptomDiaryResponseDto? Patch(long id, SymptomDiaryPatchDto patch)
        {
            var entity = _db.SymptomDiaries.FirstOrDefault(s => s.Id == id);
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

            _db.SaveChanges();
            return ToDto(entity);
        }

        public void Delete(long id)
        {
            var entity = _db.SymptomDiaries.FirstOrDefault(s => s.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Symptom diary entry not found.");
            }

            _db.SymptomDiaries.Remove(entity);
            _db.SaveChanges();
        }

        public PagedResult<DateTime> GetMarkedDays(long parentProfileId, SymptomDiarySearchObject search)
        {
            var query = _db.SymptomDiaries
                .AsNoTracking()
                .Where(s => s.ParentProfileId == parentProfileId)
                .Select(s => s.Date)
                .Distinct();

            var totalCount = query.Count();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = query
                .OrderByDescending(d => d)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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
