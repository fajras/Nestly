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

        public MedicationPlanService(NestlyDbContext db)
        {
            _db = db;
        }

        private static MedicationPlanResponseDto ToDto(MedicationPlan m) => new()
        {
            Id = m.Id,
            ParentProfileId = m.ParentProfileId,
            MedicineName = m.MedicineName,
            Dose = m.Dose,
            StartDate = m.StartDate,
            EndDate = m.EndDate,
            IntakeTimes = m.Times.Select(t => t.IntakeTime).ToList()
        };

        public IEnumerable<MedicationPlanResponseDto> Get(MedicationPlanSearchObject? search)
        {
            var q = _db.MedicationPlans
                .Include(x => x.Times)
                .AsNoTracking()
                .AsQueryable();

            if (search?.ParentProfileId != null)
            {
                q = q.Where(x => x.ParentProfileId == search.ParentProfileId);
            }

            return q.Select(ToDto).ToList();
        }

        public MedicationPlanResponseDto? GetById(long id)
        {
            var m = _db.MedicationPlans
                .Include(x => x.Times)
                .FirstOrDefault(x => x.Id == id);

            return m == null ? null : ToDto(m);
        }

        public MedicationPlanResponseDto Create(CreateMedicationPlanDto dto)
        {
            if (!_db.ParentProfiles.Any(p => p.Id == dto.ParentProfileId))
            {
                throw new ArgumentException("Parent profile not found.");
            }

            if (dto.StartDate > dto.EndDate)
            {
                throw new ArgumentException("Invalid date range.");
            }

            if (!dto.IntakeTimes.Any())
            {
                throw new ArgumentException("At least one intake time required.");
            }

            var entity = new MedicationPlan
            {
                ParentProfileId = dto.ParentProfileId,
                MedicineName = dto.MedicineName.Trim(),
                Dose = dto.Dose.Trim(),
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            foreach (var time in dto.IntakeTimes)
            {
                entity.Times.Add(new MedicationScheduleTime
                {
                    IntakeTime = time
                });
            }

            _db.MedicationPlans.Add(entity);
            _db.SaveChanges();

            GenerateIntakeLogs(entity);

            _db.SaveChanges();

            return ToDto(entity);
        }

        private void GenerateIntakeLogs(MedicationPlan plan)
        {
            var currentDate = plan.StartDate.Date;

            while (currentDate <= plan.EndDate.Date)
            {
                foreach (var time in plan.Times)
                {
                    _db.MedicationIntakeLogs.Add(new MedicationIntakeLog
                    {
                        PlanId = plan.Id,
                        ScheduledDate = currentDate,
                        IntakeTime = time.IntakeTime,
                        Taken = false
                    });
                }

                currentDate = currentDate.AddDays(1);
            }
        }

        public void MarkAsTaken(long intakeLogId)
        {
            var log = _db.MedicationIntakeLogs.FirstOrDefault(x => x.Id == intakeLogId);
            if (log == null)
            {
                throw new ArgumentException("Log not found.");
            }

            log.Taken = true;
            log.TakenAt = DateTime.UtcNow;

            _db.SaveChanges();
        }

        public MedicationPlanResponseDto? Patch(long id, MedicationPlanPatchDto patch)
        {
            var m = _db.MedicationPlans.FirstOrDefault(x => x.Id == id);
            if (m == null)
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
            if (m == null)
            {
                return false;
            }

            _db.MedicationPlans.Remove(m);
            _db.SaveChanges();
            return true;
        }
        public IEnumerable<MedicationIntakeLogDto> GetLogsForDay(long parentProfileId, DateTime date)
        {
            var day = date.Date;

            return _db.MedicationIntakeLogs
                .Include(x => x.Plan)
                .Where(x =>
                    x.Plan.ParentProfileId == parentProfileId &&
                    x.ScheduledDate == day)
                .Select(x => new MedicationIntakeLogDto
                {
                    IntakeLogId = x.Id,
                    PlanId = x.PlanId,
                    MedicineName = x.Plan.MedicineName,
                    Dose = x.Plan.Dose,
                    ScheduledDate = x.ScheduledDate,
                    IntakeTime = x.IntakeTime,
                    Taken = x.Taken
                })
                .ToList();
        }

    }
}
