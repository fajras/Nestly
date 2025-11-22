using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IBlogPostService
    {
        List<BlogPost> Get(BlogPostSearchObject? search);
        BlogPost? GetById(long id);
        BlogPost Create(CreateBlogPostDto entity);
        BlogPost? Patch(long id, BlogPostPatchDto patch);
        bool Delete(long id);
        Task<List<BlogCategoryDto>> GetAllAsync();
        List<BlogPost> GetByCategoryId(int categoryId);

    }
}
