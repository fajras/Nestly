using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMealPlanService
    {
        List<MealPlanResponseDto> Get(MealPlanSearchObject? search);
        MealPlanResponseDto? GetById(long id);
        MealPlanResponseDto Create(CreateMealPlanDto entity);
        MealPlanResponseDto? Patch(long id, MealPlanPatchDto patch);
        bool Delete(long id);

        List<MealRecommendationDto> Get(MealRecommendationSearchObject? search);
        MealRecommendationDto? GetRecommendationById(long id);
    }
}
