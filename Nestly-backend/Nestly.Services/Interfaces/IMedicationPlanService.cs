using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationPlanService
    {
        IEnumerable<MedicationPlanResponseDto> Get(MedicationPlanSearchObject? search);
        MedicationPlanResponseDto? GetById(long id);
        MedicationPlanResponseDto Create(CreateMedicationPlanDto dto);
        MedicationPlanResponseDto? Patch(long id, MedicationPlanPatchDto patch);
        bool Delete(long id);
    }
}
