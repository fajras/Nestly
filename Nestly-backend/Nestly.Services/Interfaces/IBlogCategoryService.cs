using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBlogCategoryService
    {
        Task<PagedResult<BlogCategoryDto>> Get(BlogCategorySearchObject search);
        Task<BlogCategoryDto> GetById(int id);
        Task<BlogCategoryDto> Create(BlogCategoryInsertDto request);
        Task<BlogCategoryDto> Update(int id, BlogCategoryUpdateDto request);
        Task Delete(int id);
    }
}
