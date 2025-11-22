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

        private static SymptomDiaryResponseDto ToDto(SymptomDiary s) => new SymptomDiaryResponseDto
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

            var date = (dto.Date ?? DateTime.UtcNow.Date).Date;

            var exists = _db.SymptomDiaries
                .Any(s => s.ParentProfileId == dto.ParentProfileId && s.Date == date);

            if (exists)
            {
                throw new InvalidOperationException("Diary entry for this parent and date already exists.");
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
                .Where(s => s.ParentProfileId == parentProfileId)
                .OrderByDescending(s => s.Date)
                .Select(ToDto)
                .ToList();
        }

        public SymptomDiaryResponseDto? GetByDate(long parentProfileId, DateTime date)
        {
            var d = date.Date;

            var entity = _db.SymptomDiaries
                .FirstOrDefault(s => s.ParentProfileId == parentProfileId && s.Date == d);

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
                ValidateRange(patch.Nausea, nameof(patch.Nausea));
                entity.Nausea = patch.Nausea;
            }
            if (patch.Fatigue.HasValue)
            {
                ValidateRange(patch.Fatigue, nameof(patch.Fatigue));
                entity.Fatigue = patch.Fatigue;
            }
            if (patch.Headache.HasValue)
            {
                ValidateRange(patch.Headache, nameof(patch.Headache));
                entity.Headache = patch.Headache;
            }
            if (patch.Heartburn.HasValue)
            {
                ValidateRange(patch.Heartburn, nameof(patch.Heartburn));
                entity.Heartburn = patch.Heartburn;
            }
            if (patch.LegSwelling.HasValue)
            {
                ValidateRange(patch.LegSwelling, nameof(patch.LegSwelling));
                entity.LegSwelling = patch.LegSwelling;
            }

            _db.SaveChanges();
            return ToDto(entity);
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
