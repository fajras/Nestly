using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class MedicationScheduleTimeService : IMedicationScheduleTimeService
    {
        private readonly NestlyDbContext _context;

        public MedicationScheduleTimeService(NestlyDbContext context)
        {
            _context = context;
        }

        public async Task<MedicationScheduleTime?> GetByIdAsync(int id)
        {
            return await _context.MedicationScheduleTimes.FindAsync(id);
        }

        public async Task<IEnumerable<MedicationScheduleTime>> GetAllAsync()
        {
            return await _context.MedicationScheduleTimes.ToListAsync();
        }

        public async Task<IEnumerable<MedicationScheduleTime>> GetByMedicationPlanIdAsync(int medicationPlanId)
        {
            return await _context.MedicationScheduleTimes
                .Where(mst => mst.PlanId == medicationPlanId)
                .ToListAsync();
        }

        public async Task<MedicationScheduleTime> CreateAsync(MedicationScheduleTime medicationScheduleTime)
        {
            _context.MedicationScheduleTimes.Add(medicationScheduleTime);
            await _context.SaveChangesAsync();
            return medicationScheduleTime;
        }

        public async Task<MedicationScheduleTime> UpdateAsync(MedicationScheduleTime medicationScheduleTime)
        {
            _context.MedicationScheduleTimes.Update(medicationScheduleTime);
            await _context.SaveChangesAsync();
            return medicationScheduleTime;
        }

        public async Task DeleteAsync(int id)
        {
            var medicationScheduleTime = await _context.MedicationScheduleTimes.FindAsync(id);
            if (medicationScheduleTime != null)
            {
                _context.MedicationScheduleTimes.Remove(medicationScheduleTime);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.MedicationScheduleTimes.AnyAsync(mst => mst.Id == id);
        }
    }
}
