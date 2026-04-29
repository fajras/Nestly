using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class SymptomDiaryService : ISymptomDiaryService
    {
        private readonly NestlyDbContext _db;

        public SymptomDiaryService(NestlyDbContext db)
        {
            _db = db;
        }
        public IEnumerable<SymptomDiaryResponseDto> Get(SymptomDiarySearchObject? search)
        {
            IQueryable<SymptomDiary> q = _db.SymptomDiaries.AsNoTracking();

            if (search?.ParentProfileId is not null)
            {
                q = q.Where(s => s.ParentProfileId == search.ParentProfileId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(s => s.Date >= search.DateFrom.Value.Date);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(s => s.Date <= search.DateTo.Value.Date);
            }

            return q
                .OrderByDescending(s => s.Date)
                .Select(ToDto)
                .ToList();
        }
        private static SymptomDiaryResponseDto ToDto(SymptomDiary s) => new()
        {
            Id = s.Id,
            ParentProfileId = s.ParentProfileId,
            Date = s.Date,
            Nausea = s.Nausea,
            Fatigue = s.Fatigue,
            Headache = s.Headache,
            Heartburn = s.Heartburn,
            LegSwelling = s.LegSwelling
        };

        public SymptomDiaryResponseDto Create(CreateSymptomDiaryDto dto)
        {
            if (!_db.ParentProfiles.Any(p => p.Id == dto.ParentProfileId))
            {
                throw new ArgumentException("ParentProfile not found.", nameof(dto.ParentProfileId));
            }

            var date = (dto.Date ?? DateTime.Today).Date;

            if (_db.SymptomDiaries.Any(s =>
                s.ParentProfileId == dto.ParentProfileId &&
                s.Date == date))
            {
                throw new InvalidOperationException("Diary entry already exists for this date.");
            }

            ValidateRange(dto.Nausea, nameof(dto.Nausea));
            ValidateRange(dto.Fatigue, nameof(dto.Fatigue));
            ValidateRange(dto.Headache, nameof(dto.Headache));
            ValidateRange(dto.Heartburn, nameof(dto.Heartburn));
            ValidateRange(dto.LegSwelling, nameof(dto.LegSwelling));

            var entity = new SymptomDiary
            {
                ParentProfileId = dto.ParentProfileId,
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

        public IEnumerable<SymptomDiaryResponseDto> GetByParent(long parentProfileId)
        {
            return _db.SymptomDiaries
                .AsNoTracking()
                .Where(s => s.ParentProfileId == parentProfileId)
                .OrderByDescending(s => s.Date)
                .Select(ToDto)
                .ToList();
        }

        public SymptomDiaryResponseDto? GetByDate(long parentProfileId, DateTime date)
        {
            var entity = _db.SymptomDiaries
                .AsNoTracking()
                .FirstOrDefault(s =>
                    s.ParentProfileId == parentProfileId &&
                    s.Date == date.Date);

            return entity is null ? null : ToDto(entity);
        }

        public SymptomDiaryResponseDto? Patch(long id, SymptomDiaryPatchDto patch)
        {
            var entity = _db.SymptomDiaries.FirstOrDefault(s => s.Id == id);
            if (entity is null)
            {
                return null;
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

        public bool Delete(long id)
        {
            var entity = _db.SymptomDiaries.FirstOrDefault(s => s.Id == id);
            if (entity is null)
            {
                return false;
            }

            _db.SymptomDiaries.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<DateTime> GetMarkedDays(long parentProfileId)
        {
            return _db.SymptomDiaries
                .AsNoTracking()
                .Where(s => s.ParentProfileId == parentProfileId)
                .Select(s => s.Date)
                .Distinct()
                .ToList();
        }

        private static void ValidateRange(int? value, string field)
        {
            if (value.HasValue && (value.Value < 1 || value.Value > 5))
            {
                throw new ArgumentOutOfRangeException(field, "Value must be between 1 and 5.");
            }
        }


    }
}
