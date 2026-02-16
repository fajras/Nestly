using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class BabyProfileService : IBabyProfileService
    {
        private readonly NestlyDbContext _db;

        public BabyProfileService(NestlyDbContext db)
        {
            _db = db;
        }

        public IEnumerable<BabyProfileSummaryDto> Get(BabyProfileSearchObject? search)
        {
            IQueryable<BabyProfile> q = _db.BabyProfiles.AsQueryable();

            if (search?.UserId is not null)
            {
                q = q.Where(x => x.ParentProfileId == search.UserId);
            }

            if (!string.IsNullOrWhiteSpace(search?.BabyName))
            {
                q = q.Where(x => x.BabyName.Contains(search.BabyName.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(search?.Gender))
            {
                q = q.Where(x => x.Gender == search.Gender.Trim());
            }

            if (search?.BirthDateFrom is not null)
            {
                q = q.Where(x => x.BirthDate >= search.BirthDateFrom.Value.Date);
            }

            if (search?.BirthDateTo is not null)
            {
                q = q.Where(x => x.BirthDate <= search.BirthDateTo.Value.Date);
            }

            return q.Select(MapToDto).ToList();
        }

        public BabyProfileSummaryDto? GetById(long id)
        {
            var entity = _db.BabyProfiles.FirstOrDefault(x => x.Id == id);
            return entity is null ? null : MapToDto(entity);
        }

        public BabyProfileSummaryDto Create(CreateBabyProfileDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.ParentProfileId <= 0)
            {
                throw new ArgumentException("ParentProfileId is required.");
            }

            if (!_db.ParentProfiles.Any(p => p.Id == dto.ParentProfileId))
            {
                throw new ArgumentException("Parent profile does not exist.");
            }

            if (string.IsNullOrWhiteSpace(dto.BabyName))
            {
                throw new ArgumentException("BabyName is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new ArgumentException("Gender is required.");
            }

            if (dto.BirthDate == default)
            {
                throw new ArgumentException("BirthDate is required.");
            }

            if (dto.PregnancyId.HasValue &&
                !_db.Pregnancies.Any(x => x.Id == dto.PregnancyId.Value))
            {
                throw new ArgumentException("Pregnancy does not exist.");
            }

            var entity = new BabyProfile
            {
                ParentProfileId = dto.ParentProfileId,
                BabyName = dto.BabyName,
                Gender = dto.Gender,
                BirthDate = dto.BirthDate,
                PregnancyId = dto.PregnancyId
            };

            _db.BabyProfiles.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public BabyProfileSummaryDto? Patch(long id, BabyProfilePatchDto patch)
        {
            var dbEntity = _db.BabyProfiles.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.BabyName is not null)
            {
                dbEntity.BabyName = patch.BabyName;
            }

            if (patch.Gender is not null)
            {
                dbEntity.Gender = patch.Gender;
            }

            if (patch.BirthDate is not null)
            {
                dbEntity.BirthDate = patch.BirthDate.Value;
            }

            _db.SaveChanges();

            return MapToDto(dbEntity);
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.BabyProfiles.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.BabyProfiles.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }

        public BabyProfileSummaryDto? GetLatestByParent(long parentProfileId)
        {
            var entity = _db.BabyProfiles
                            .Where(x => x.ParentProfileId == parentProfileId)
                            .OrderByDescending(x => x.BirthDate)
                            .FirstOrDefault();

            return entity is null ? null : MapToDto(entity);
        }

        private static BabyProfileSummaryDto MapToDto(BabyProfile entity)
        {
            return new BabyProfileSummaryDto
            {
                Id = entity.Id,
                BabyName = entity.BabyName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate
            };
        }
    }
}
