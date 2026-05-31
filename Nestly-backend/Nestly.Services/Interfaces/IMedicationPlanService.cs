using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationPlanService
    {
        PagedResult<MedicationPlanResponseDto> Get(MedicationPlanSearchObject search);
        MedicationPlanResponseDto GetById(long id);
        MedicationPlanResponseDto Create(long parentProfileId, CreateMedicationPlanDto dto);
        MedicationPlanResponseDto Patch(long id, MedicationPlanPatchDto patch);
        void Delete(long id);
        PagedResult<MedicationIntakeLogDto> GetLogsForDay(MedicationIntakeLogSearchObject search);
        void MarkAsTaken(long intakeLogId);
        PagedResult<MedicationPlanResponseDto> GetByParent(long parentProfileId, MedicationPlanSearchObject search);
        PagedResult<MedicationIntakeLogDto> GetLogsForDayByParent(long parentProfileId, MedicationIntakeLogSearchObject search);
    }
}
