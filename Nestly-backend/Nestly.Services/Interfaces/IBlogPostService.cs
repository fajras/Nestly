using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogPostService
    {
        List<BlogPost> Get(BlogPostSearchObject? search);
        BlogPost? GetById(long id);
        BlogPost Create(CreateBlogPostDto entity);
        BlogPost? Patch(long id, BlogPostPatchDto patch);
        bool Delete(long id);
    }
}
