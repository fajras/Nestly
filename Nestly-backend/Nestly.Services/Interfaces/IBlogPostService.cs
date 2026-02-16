using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogPostService
    {
        IEnumerable<BlogPostResponseDto> Get(BlogPostSearchObject? search);
        BlogPostResponseDto? GetById(long id);
        BlogPostResponseDto Create(CreateBlogPostDto entity);
        BlogPostResponseDto? Patch(long id, BlogPostPatchDto patch);
        bool Delete(long id);

        Task<List<BlogCategoryDto>> GetAllAsync();
        IEnumerable<BlogPostResponseDto> GetByCategoryId(int categoryId);
    }
}
