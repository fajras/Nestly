using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;

namespace Nestly.Services.Repository
{
    public class PregnancyService : IPregnancyService
    {
        private readonly NestlyDbContext _db;
        public PregnancyService(NestlyDbContext db) => _db = db;

        private const int GestationDays = 280;

        public PagedResult<PregnancyResponseDto> Get(PregnancySearchObject search)
        {
            IQueryable<Pregnancy> q = _db.Pregnancies.AsNoTracking();

            if (search.UserId is not null)
            {
                q = q.Where(p => p.ParentProfileId == search.UserId);
            }

            if (search.LmpFrom is not null)
            {
                q = q.Where(p => p.LmpDate >= search.LmpFrom.Value);
            }

            if (search.LmpTo is not null)
            {
                q = q.Where(p => p.LmpDate <= search.LmpTo.Value);
            }

            if (search.DueFrom is not null)
            {
                q = q.Where(p => p.DueDate >= search.DueFrom.Value);
            }

            if (search.DueTo is not null)
            {
                q = q.Where(p => p.DueDate <= search.DueTo.Value);
            }

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(p => p.LmpDate ?? DateTime.MinValue)
                .ThenByDescending(p => p.Id)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(ToDto)
                .ToList();

            return new PagedResult<PregnancyResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public PregnancyResponseDto? GetById(long id)
        {
            var entity = _db.Pregnancies
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            return entity is null ? null : ToDto(entity);
        }

        public PregnancyResponseDto? GetByParentProfileId(long parentProfileId)
        {
            var entity = _db.Pregnancies
                .AsNoTracking()
                .Where(p => p.ParentProfileId == parentProfileId)
                .OrderByDescending(p => p.LmpDate ?? p.DueDate)
                .FirstOrDefault();

            return entity is null ? null : ToDto(entity);
        }

        public PregnancyResponseDto Create(CreatePregnancyDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (!_db.ParentProfiles.Any(p => p.Id == dto.UserId))
            {
                throw new ArgumentException("Parent profile does not exist.");
            }

            var (lmp, due) = NormalizeDates(dto.LmpDate, dto.DueDate);

            if (lmp.HasValue && due.HasValue && due < lmp)
            {
                throw new ArgumentException("DueDate cannot be before LmpDate.");
            }

            if (dto.CycleLengthDays.HasValue &&
                (dto.CycleLengthDays < 20 || dto.CycleLengthDays > 40))
            {
                throw new ArgumentException("Cycle length must be between 20 and 40.");
            }

            var entity = new Pregnancy
            {
                ParentProfileId = dto.UserId,
                LmpDate = lmp,
                DueDate = due,
                CycleLengthDays = dto.CycleLengthDays
            };

            _db.Pregnancies.Add(entity);
            _db.SaveChanges();

            return ToDto(entity);
        }

        public PregnancyResponseDto? Patch(long id, PregnancyPatchDto patch)
        {
            var entity = _db.Pregnancies.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            if (patch.UserId is not null && patch.UserId != entity.ParentProfileId)
            {
                if (!_db.ParentProfiles.Any(p => p.Id == patch.UserId))
                {
                    throw new ArgumentException("Parent profile does not exist.");
                }

                entity.ParentProfileId = patch.UserId.Value;
            }

            var newLmp = patch.LmpDate ?? entity.LmpDate;
            var newDue = patch.DueDate ?? entity.DueDate;

            (newLmp, newDue) = NormalizeDates(newLmp, newDue);

            if (newLmp.HasValue && newDue.HasValue && newDue < newLmp)
            {
                throw new ArgumentException("DueDate cannot be before LmpDate.");
            }

            entity.LmpDate = newLmp;
            entity.DueDate = newDue;

            if (patch.CycleLengthDays is not null)
            {
                if (!newLmp.HasValue)
                {
                    throw new ArgumentException("Cycle length requires LMP.");
                }

                if (patch.CycleLengthDays < 20 || patch.CycleLengthDays > 40)
                {
                    throw new ArgumentException("Cycle length must be between 20 and 40.");
                }

                entity.CycleLengthDays = patch.CycleLengthDays;
            }

            _db.SaveChanges();
            return ToDto(entity);
        }

        public bool Delete(long id)
        {
            var entity = _db.Pregnancies.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            _db.Pregnancies.Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public PregnancyStatusDto? GetStatus(long parentProfileId)
        {
            var pregnancy = _db.Pregnancies
                .Where(p => p.ParentProfileId == parentProfileId)
                .OrderByDescending(p => p.LmpDate ?? p.DueDate)
                .FirstOrDefault();

            if (pregnancy == null)
            {
                return null;
            }

            var (lmp, due) = NormalizeDates(pregnancy.LmpDate, pregnancy.DueDate);

            if (!lmp.HasValue || !due.HasValue)
            {
                return null;
            }

            var today = DateTime.UtcNow.Date;

            var ageDays = Math.Max(0, (today - lmp.Value).Days);
            var week = Math.Max(1, (ageDays / 7) + 1);
            var remaining = Math.Max(0, (due.Value - today).Days);

            return new PregnancyStatusDto
            {
                ParentProfileId = parentProfileId,
                LmpDate = lmp,
                DueDate = due,
                GestationalWeek = week,
                DaysRemaining = remaining
            };
        }

        private static (DateTime? lmp, DateTime? due) NormalizeDates(DateTime? lmp, DateTime? due)
        {
            if (lmp.HasValue && !due.HasValue)
            {
                return (lmp, lmp.Value.AddDays(GestationDays));
            }

            if (!lmp.HasValue && due.HasValue)
            {
                return (due.Value.AddDays(-GestationDays), due);
            }

            return (lmp, due);
        }

        private static PregnancyResponseDto ToDto(Pregnancy p) => new()
        {
            Id = p.Id,
            ParentProfileId = p.ParentProfileId,
            LmpDate = p.LmpDate,
            DueDate = p.DueDate,
            CycleLengthDays = p.CycleLengthDays
        };
    }
}