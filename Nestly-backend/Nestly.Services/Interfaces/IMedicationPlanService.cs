using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMedicationPlanService
    {
        List<MedicationPlan> Get(MedicationPlanSearchObject? search);
        MedicationPlan? GetById(long id);
        MedicationPlan Create(MedicationPlan entity);
        MedicationPlan? Patch(long id, MedicationPlanPatchDto patch);
        bool Delete(long id);
    }
}
