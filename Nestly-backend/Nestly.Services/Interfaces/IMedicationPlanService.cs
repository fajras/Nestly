using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationPlanService
    {
        Task<PagedResult<MedicationPlanResponseDto>> Get(MedicationPlanSearchObject search);
        Task<MedicationPlanResponseDto> GetById(long id);
        Task<MedicationPlanResponseDto> Create(long parentProfileId, CreateMedicationPlanDto dto);
        Task<MedicationPlanResponseDto> Patch(long id, MedicationPlanPatchDto patch);
        Task Delete(long id);
        Task<PagedResult<MedicationIntakeLogDto>> GetLogsForDay(MedicationIntakeLogSearchObject search);
        Task MarkAsTaken(long intakeLogId);
        Task<PagedResult<MedicationPlanResponseDto>> GetByParent(long parentProfileId, MedicationPlanSearchObject search);
        Task<PagedResult<MedicationIntakeLogDto>> GetLogsForDayByParent(long parentProfileId, MedicationIntakeLogSearchObject search);
    }
}
