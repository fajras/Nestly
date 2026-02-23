using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogRecommendationService
    {
        Task<List<BlogPostResponseDto>> GetRecommendations(long userId, int take);
        Task LogInteraction(long userId, LogBlogInteractionRequest request);
    }
}
