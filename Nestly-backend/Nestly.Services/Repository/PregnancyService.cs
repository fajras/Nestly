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

        public List<PregnancyResponseDto> Get(PregnancySearchObject? search)
        {
            IQueryable<Pregnancy> q = _db.Pregnancies.AsNoTracking();

            if (search?.UserId is not null)
            {
                q = q.Where(p => p.ParentProfileId == search.UserId);
            }

            if (search?.LmpFrom is not null)
            {
                q = q.Where(p => p.LmpDate >= search.LmpFrom.Value);
            }

            if (search?.LmpTo is not null)
            {
                q = q.Where(p => p.LmpDate <= search.LmpTo.Value);
            }

            if (search?.DueFrom is not null)
            {
                q = q.Where(p => p.DueDate >= search.DueFrom.Value);
            }

            if (search?.DueTo is not null)
            {
                q = q.Where(p => p.DueDate <= search.DueTo.Value);
            }

            return q
                .OrderByDescending(p => p.LmpDate ?? DateTime.MinValue)
                .ThenByDescending(p => p.DueDate ?? DateTime.MinValue)
                .ThenByDescending(p => p.Id)
                .Select(ToDto)
                .ToList();
        }

        public PregnancyResponseDto? GetById(long id)
        {
            var entity = _db.Pregnancies
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            return entity is null ? null : ToDto(entity);
        }


        public PregnancyResponseDto Create(CreatePregnancyDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (!_db.ParentProfiles.Any(p => p.Id == dto.UserId))
            {
                throw new ArgumentException("Parent profile does not exist.", nameof(dto.UserId));
            }

            if (dto.LmpDate.HasValue && dto.DueDate.HasValue && dto.DueDate.Value < dto.LmpDate.Value)
            {
                throw new ArgumentException("DueDate cannot be before LmpDate.", nameof(dto.DueDate));
            }

            var entity = new Pregnancy
            {
                ParentProfileId = dto.UserId,
                LmpDate = dto.LmpDate,
                DueDate = dto.DueDate
            };

            _db.Pregnancies.Add(entity);
            _db.SaveChanges();

            return ToDto(entity);
        }

        public PregnancyResponseDto? Patch(long id, PregnancyPatchDto patch)
        {
            var entity = _db.Pregnancies.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return null;
            }

            if (patch.UserId is not null && patch.UserId.Value != entity.ParentProfileId)
            {
                if (!_db.ParentProfiles.Any(p => p.Id == patch.UserId.Value))
                {
                    throw new ArgumentException("Parent profile does not exist.");
                }

                entity.ParentProfileId = patch.UserId.Value;
            }

            if (patch.LmpDate is not null)
            {
                entity.LmpDate = patch.LmpDate.Value;
            }

            if (patch.DueDate is not null)
            {
                entity.DueDate = patch.DueDate.Value;
            }

            _db.SaveChanges();

            return ToDto(entity);
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.Pregnancies.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.Pregnancies.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }

        private const int GestationFromLmpDays = 280;
        public PregnancyStatusDto? GetStatus(long parentProfileId)
        {
            // Uzimamo zadnju trudnoću za tog roditelja (ako ih ima više)
            var pregnancy = _db.Pregnancies
                .Where(p => p.ParentProfileId == parentProfileId)
                .OrderByDescending(p => p.LmpDate ?? p.DueDate)
                .FirstOrDefault();

            if (pregnancy == null)
            {
                return null;
            }

            if (!pregnancy.LmpDate.HasValue && !pregnancy.DueDate.HasValue)
            {
                return null;
            }

            var today = DateTime.UtcNow.Date;

            DateTime lmp;
            DateTime due;

            if (pregnancy.LmpDate.HasValue)
            {
                lmp = pregnancy.LmpDate.Value.Date;
                due = (pregnancy.DueDate ?? lmp.AddDays(GestationFromLmpDays)).Date;
            }
            else
            {
                // Nema LMP, ali ima DueDate -> izračunaj LMP unazad
                due = pregnancy.DueDate!.Value.Date;
                lmp = due.AddDays(-GestationFromLmpDays);
            }

            // gestacijska dob u danima (ne ispod 0)
            var ageDays = (today - lmp).Days;
            if (ageDays < 0)
            {
                ageDays = 0;
            }

            // sedmica trudnoće (1-based)
            var week = (ageDays / 7) + 1;
            if (week < 1)
            {
                week = 1;
            }

            // preostali dani (ne ispod 0)
            var remaining = (due - today).Days;
            if (remaining < 0)
            {
                remaining = 0;
            }

            return new PregnancyStatusDto
            {
                ParentProfileId = parentProfileId,
                LmpDate = lmp,
                DueDate = due,
                GestationalWeek = week,
                DaysRemaining = remaining
            };
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


};
