using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogPostService
    {
        PagedResult<BlogPostResponseDto> Get(BlogPostSearchObject search);
        BlogPostResponseDto? GetById(long id);
        BlogPostResponseDto Create(CreateBlogPostDto entity);
        BlogPostResponseDto? Patch(long id, BlogPostPatchDto patch);
        bool Delete(long id);
        PagedResult<BlogPostResponseDto> GetByCategoryId(int categoryId, int page, int pageSize);
    }
}
