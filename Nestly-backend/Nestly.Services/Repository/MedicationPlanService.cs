using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class MedicationPlanService : IMedicationPlanService
    {
        private readonly NestlyDbContext _db;
        public MedicationPlanService(NestlyDbContext db) => _db = db;

        private static MedicationPlanResponseDto ToDto(MedicationPlan m) => new()
        {
            Id = m.Id,
            UserId = m.UserId,
            MedicineName = m.MedicineName,
            Dose = m.Dose,
            StartDate = m.StartDate,
            EndDate = m.EndDate
        };

        public IEnumerable<MedicationPlanResponseDto> Get(MedicationPlanSearchObject? search)
        {
            IQueryable<MedicationPlan> q = _db.MedicationPlans.AsNoTracking();

            if (search?.UserId is not null)
            {
                q = q.Where(x => x.UserId == search.UserId);
            }

            if (search?.From is not null)
            {
                q = q.Where(x => x.StartDate >= search.From);
            }

            if (search?.To is not null)
            {
                q = q.Where(x => x.EndDate <= search.To);
            }

            return q
                .OrderBy(x => x.StartDate)
                .Select(ToDto)
                .ToList();
        }

        public MedicationPlanResponseDto? GetById(long id)
        {
            var m = _db.MedicationPlans.AsNoTracking().FirstOrDefault(x => x.Id == id);
            return m is null ? null : ToDto(m);
        }

        public MedicationPlanResponseDto Create(CreateMedicationPlanDto dto)
        {
            if (!_db.AppUsers.Any(u => u.Id == dto.UserId))
            {
                throw new ArgumentException("User not found.", nameof(dto.UserId));
            }

            var entity = new MedicationPlan
            {
                UserId = dto.UserId,
                MedicineName = dto.MedicineName.Trim(),
                Dose = dto.Dose.Trim(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _db.MedicationPlans.Add(entity);
            _db.SaveChanges();
            return ToDto(entity);
        }

        public MedicationPlanResponseDto? Patch(long id, MedicationPlanPatchDto patch)
        {
            var m = _db.MedicationPlans.FirstOrDefault(x => x.Id == id);
            if (m is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(patch.MedicineName))
            {
                m.MedicineName = patch.MedicineName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(patch.Dose))
            {
                m.Dose = patch.Dose.Trim();
            }

            if (patch.StartDate.HasValue)
            {
                m.StartDate = patch.StartDate.Value;
            }

            if (patch.EndDate.HasValue)
            {
                m.EndDate = patch.EndDate.Value;
            }

            _db.SaveChanges();
            return ToDto(m);
        }

        public bool Delete(long id)
        {
            var m = _db.MedicationPlans.FirstOrDefault(x => x.Id == id);
            if (m is null)
            {
                return false;
            }

            _db.MedicationPlans.Remove(m);
            _db.SaveChanges();
            return true;
        }
    }
}
