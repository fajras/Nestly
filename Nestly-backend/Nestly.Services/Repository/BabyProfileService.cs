using Microsoft.EntityFrameworkCore;
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

        public List<BabyProfile> Get(BabyProfileSearchObject? search)
        {
            IQueryable<BabyProfile> q = _db.BabyProfiles
                                           .Include(x => x.ParentProfile)
                                           .AsQueryable();

            if (search?.UserId is not null)
            {
                q = q.Where(x => x.ParentProfileId == search.UserId);
            }

            if (!string.IsNullOrWhiteSpace(search?.BabyName))
            {
                var name = search.BabyName.Trim();
                q = q.Where(x => x.BabyName.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(search?.Gender))
            {
                var g = search.Gender.Trim();
                q = q.Where(x => x.Gender == g);
            }

            if (search?.BirthDateFrom is not null)
            {
                q = q.Where(x => x.BirthDate >= search.BirthDateFrom.Value.Date);
            }

            if (search?.BirthDateTo is not null)
            {
                q = q.Where(x => x.BirthDate <= search.BirthDateTo.Value.Date);
            }

            return q.ToList();
        }

        public BabyProfile? GetById(long id)
        {
            return _db.BabyProfiles
                      .Include(x => x.ParentProfile)
                      .FirstOrDefault(x => x.Id == id);
        }

        public BabyProfile Create(CreateBabyProfileDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.ParentProfileId <= 0)
            {
                throw new ArgumentException("ParentProfileId is required.", nameof(dto.ParentProfileId));
            }

            if (!_db.ParentProfiles.Any(p => p.Id == dto.ParentProfileId))
            {
                throw new ArgumentException("Parent profile does not exist.", nameof(dto.ParentProfileId));
            }

            if (string.IsNullOrWhiteSpace(dto.BabyName))
            {
                throw new ArgumentException("BabyName is required.", nameof(dto.BabyName));
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new ArgumentException("Gender is required.", nameof(dto.Gender));
            }

            if (dto.BirthDate == default)
            {
                throw new ArgumentException("BirthDate is required.", nameof(dto.BirthDate));
            }

            if (dto.PregnancyId.HasValue &&
                !_db.Pregnancies.Any(x => x.Id == dto.PregnancyId.Value))
            {
                throw new ArgumentException("Pregnancy does not exist.", nameof(dto.PregnancyId));
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

            return entity;
        }

        public BabyProfile? Patch(long id, BabyProfilePatchDto patch)
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
            return dbEntity;
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
    }
}

