using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogPostService
    {
        PagedResult<BlogPostResponseDto> Get(BlogPostSearchObject search);
        BlogPostResponseDto GetById(long id);
        BlogPostResponseDto Create(CreateBlogPostDto dto, long authorId);
        BlogPostResponseDto Patch(long id, BlogPostPatchDto patch, long currentUserId);
        void Delete(long id, long currentUserId);
        PagedResult<BlogPostResponseDto> GetByCategoryId(int categoryId, int page, int pageSize);
    }
}
