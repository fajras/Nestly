using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogCategoryService
    {
        PagedResult<BlogCategoryDto> Get(BlogCategorySearchObject search);
        BlogCategoryDto? GetById(int id);
        BlogCategoryDto Create(BlogCategoryInsertDto request);
        BlogCategoryDto? Update(int id, BlogCategoryUpdateDto request);
        bool Delete(int id);
    }
}
