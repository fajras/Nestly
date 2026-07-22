using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<PagedResult<BlogPostResponseDto>> Get(BlogPostSearchObject search);
        Task<BlogPostResponseDto> GetById(long id);
        Task<BlogPostResponseDto> Create(CreateBlogPostDto dto, long authorId);
        Task<BlogPostResponseDto> Patch(long id, BlogPostPatchDto patch, long currentUserId);
        Task Delete(long id, long currentUserId);
        Task<PagedResult<BlogPostResponseDto>> GetByCategoryId(int categoryId, int page, int pageSize);
    }
}
