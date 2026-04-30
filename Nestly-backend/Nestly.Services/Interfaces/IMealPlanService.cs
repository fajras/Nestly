using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMealPlanService
    {
        MealPlanResponseDto GetById(long id);
        MealPlanResponseDto Create(CreateMealPlanDto entity);
        MealPlanResponseDto Patch(long id, MealPlanPatchDto patch);
        void Delete(long id);
        PagedResult<MealPlanResponseDto> GetMealPlans(MealPlanSearchObject search);
        PagedResult<MealRecommendationDto> GetMealRecommendations(MealRecommendationSearchObject search);
        MealRecommendationDto GetRecommendationById(long id);
        MealRecommendationDto CreateRecommendation(CreateMealRecommendationDto request);
        List<FoodTypeDto> GetFoodTypesWithoutRecommendation();
        MealRecommendationDto UpdateRecommendation(long id, CreateMealRecommendationDto dto);
        void DeleteRecommendation(long id);
    }
}
