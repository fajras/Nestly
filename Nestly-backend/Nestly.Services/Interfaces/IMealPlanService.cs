using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMealPlanService
    {
        Task<MealPlanResponseDto> GetById(long id);
        Task<MealPlanResponseDto> Create(CreateMealPlanDto entity);
        Task<MealPlanResponseDto> Patch(long id, MealPlanPatchDto patch);
        Task Delete(long id);
        Task<PagedResult<MealPlanResponseDto>> GetMealPlans(MealPlanSearchObject search);
        Task<PagedResult<MealRecommendationDto>> GetMealRecommendations(MealRecommendationSearchObject search);
        Task<MealRecommendationDto> GetRecommendationById(long id);
        Task<MealRecommendationDto> CreateRecommendation(CreateMealRecommendationDto request);
        Task<List<FoodTypeDto>> GetFoodTypesWithoutRecommendation();
        Task<MealRecommendationDto> UpdateRecommendation(long id, CreateMealRecommendationDto dto);
        Task DeleteRecommendation(long id);
        Task<PagedResult<MealPlanResponseDto>> GetMealPlansByParent(long parentProfileId, MealPlanSearchObject search);

    }
}
