using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationScheduleTimeService
    {
        Task<MedicationScheduleTime?> GetByIdAsync(int id);
        Task<IEnumerable<MedicationScheduleTime>> GetAllAsync();
        Task<IEnumerable<MedicationScheduleTime>> GetByMedicationPlanIdAsync(int medicationPlanId);
        Task<MedicationScheduleTime> CreateAsync(MedicationScheduleTime medicationScheduleTime);
        Task<MedicationScheduleTime> UpdateAsync(MedicationScheduleTime medicationScheduleTime);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
