using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationPlanService
    {
        PagedResult<MedicationPlanResponseDto> Get(MedicationPlanSearchObject search);
        MedicationPlanResponseDto GetById(long id);
        MedicationPlanResponseDto Create(CreateMedicationPlanDto dto);
        MedicationPlanResponseDto Patch(long id, MedicationPlanPatchDto patch);
        void Delete(long id);
        PagedResult<MedicationIntakeLogDto> GetLogsForDay(MedicationIntakeLogSearchObject search);
        void MarkAsTaken(long intakeLogId);
    }
}
