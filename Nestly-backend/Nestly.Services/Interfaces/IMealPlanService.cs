using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMealPlanService
    {
        List<MealPlanResponseDto> GetMealPlans(MealPlanSearchObject? search);
        MealPlanResponseDto? GetById(long id);
        MealPlanResponseDto Create(CreateMealPlanDto entity);
        MealPlanResponseDto? Patch(long id, MealPlanPatchDto patch);
        bool Delete(long id);

        List<MealRecommendationDto> GetMealRecommendations(MealRecommendationSearchObject? search);
        MealRecommendationDto? GetRecommendationById(long id);
        MealRecommendationDto CreateRecommendation(CreateMealRecommendationDto request);
        List<FoodTypeDto> GetFoodTypesWithoutRecommendation();
        MealRecommendationDto? UpdateRecommendation(long id, CreateMealRecommendationDto dto);
        bool DeleteRecommendation(long id);
    }
}
