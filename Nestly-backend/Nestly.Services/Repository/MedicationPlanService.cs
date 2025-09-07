using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
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

        public MedicationPlan Create(MedicationPlan entity)
        {
            if (entity.UserId <= 0)
            {
                throw new ArgumentException("UserId is required.");
            }

            if (!_db.AppUsers.Any(u => u.Id == entity.UserId))
            {
                throw new ArgumentException("User does not exist.");
            }

            if (string.IsNullOrWhiteSpace(entity.MedicineName))
            {
                throw new ArgumentException("MedicineName is required.");
            }

            if (string.IsNullOrWhiteSpace(entity.Dose))
            {
                throw new ArgumentException("Dose is required.");
            }

            if (entity.StartDate >= entity.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

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
