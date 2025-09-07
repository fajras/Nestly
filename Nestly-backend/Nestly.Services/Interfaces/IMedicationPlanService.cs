using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationPlanService
    {
        List<MedicationPlan> Get(MedicationPlanSearchObject? search);
        MedicationPlan? GetById(long id);
        MedicationPlan Create(CreateMedicationPlanDto entity);
        MedicationPlan? Patch(long id, MedicationPlanPatchDto patch);
        bool Delete(long id);
    }
}
