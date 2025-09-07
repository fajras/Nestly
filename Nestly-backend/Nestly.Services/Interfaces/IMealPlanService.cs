using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMealPlanService
    {
        List<MealPlan> Get(MealPlanSearchObject? search);
        MealPlan? GetById(long id);
        MealPlan Create(MealPlan entity);
        MealPlan? Patch(long id, MealPlanPatchDto patch);
        bool Delete(long id);
    }
}
