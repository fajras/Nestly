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

        public List<MedicationPlan> Get(MedicationPlanSearchObject? search)
        {
            IQueryable<MedicationPlan> q = _db.MedicationPlans
                                              .Include(p => p.Times)
                                              .Include(p => p.IntakeLogs)
                                              .Include(p => p.User);

            if (search?.UserId is not null)
            {
                q = q.Where(p => p.UserId == search.UserId);
            }

            if (!string.IsNullOrWhiteSpace(search?.MedicineName))
            {
                q = q.Where(p => p.MedicineName.Contains(search.MedicineName));
            }

            if (search?.ActiveOn is not null)
            {
                q = q.Where(p => p.StartDate <= search.ActiveOn.Value &&
                                 p.EndDate >= search.ActiveOn.Value);
            }

            return q.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public MedicationPlan? GetById(long id)
        {
            return _db.MedicationPlans
                      .Include(p => p.Times)
                      .Include(p => p.IntakeLogs)
                      .Include(p => p.User)
                      .FirstOrDefault(p => p.Id == id);
        }

        public MedicationPlan Create(CreateMedicationPlanDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.UserId <= 0)
            {
                throw new ArgumentException("UserId (ParentProfileId) is required.", nameof(dto.UserId));
            }

            bool parentExists = _db.ParentProfiles.Any(p => p.Id == dto.UserId);
            if (!parentExists)
            {
                throw new ArgumentException("Parent profile does not exist.", nameof(dto.UserId));
            }

            if (string.IsNullOrWhiteSpace(dto.MedicineName))
            {
                throw new ArgumentException("MedicineName is required.", nameof(dto.MedicineName));
            }

            if (string.IsNullOrWhiteSpace(dto.Dose))
            {
                throw new ArgumentException("Dose is required.", nameof(dto.Dose));
            }

            if (dto.StartDate == default)
            {
                throw new ArgumentException("StartDate is required.", nameof(dto.StartDate));
            }

            if (dto.EndDate == default)
            {
                throw new ArgumentException("EndDate is required.", nameof(dto.EndDate));
            }

            if (dto.StartDate >= dto.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate.", nameof(dto.EndDate));
            }

            bool overlaps = _db.MedicationPlans.Any(p =>
                p.UserId == dto.UserId &&
                p.MedicineName == dto.MedicineName &&
                p.EndDate > dto.StartDate &&
                p.StartDate < dto.EndDate
            );
            if (overlaps)
            {
                throw new InvalidOperationException("Overlapping plan for the same medicine already exists for this user.");
            }

            var entity = new MedicationPlan
            {
                UserId = dto.UserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MedicineName = dto.MedicineName.Trim(),
                Dose = dto.Dose.Trim()
            };

            _db.MedicationPlans.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public MedicationPlan? Patch(long id, MedicationPlanPatchDto patch)
        {
            var plan = _db.MedicationPlans.FirstOrDefault(p => p.Id == id);
            if (plan is null)
            {
                return null;
            }

            if (patch.StartDate is not null)
            {
                plan.StartDate = patch.StartDate.Value;
            }

            if (patch.EndDate is not null)
            {
                plan.EndDate = patch.EndDate.Value;
            }

            if (patch.MedicineName is not null)
            {
                plan.MedicineName = patch.MedicineName;
            }

            if (patch.Dose is not null)
            {
                plan.Dose = patch.Dose;
            }

            if (plan.StartDate >= plan.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate.");
            }

            plan.CreatedAt = plan.CreatedAt == default ? DateTime.UtcNow : plan.CreatedAt;

            _db.SaveChanges();
            return plan;
        }

        public bool Delete(long id)
        {
            var plan = _db.MedicationPlans.FirstOrDefault(p => p.Id == id);
            if (plan is null)
            {
                return false;
            }

            _db.MedicationPlans.Remove(plan);
            _db.SaveChanges();
            return true;
        }
    }
}
