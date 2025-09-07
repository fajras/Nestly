using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IMealPlanService
    {
        List<MealPlan> Get(MealPlanSearchObject? search);
        MealPlan? GetById(long id);
        MealPlan Create(CreateMealPlanDto entity);
        MealPlan? Patch(long id, MealPlanPatchDto patch);
        bool Delete(long id);
    }
}
