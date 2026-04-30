using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;
namespace Nestly.Services.Repository
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly NestlyDbContext _db;

        public BlogCategoryService(NestlyDbContext db)
        {
            _db = db;
        }

        private static BlogCategoryDto MapToDto(BlogCategory entity)
        {
            return new BlogCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public PagedResult<BlogCategoryDto> Get(BlogCategorySearchObject search)
        {
            IQueryable<BlogCategory> query = _db.BlogCategories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            var totalCount = query.Count();

            var items = query
                .OrderBy(x => x.Name)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResult<BlogCategoryDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public BlogCategoryDto GetById(int id)
        {
            var entity = _db.BlogCategories
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Category not found.");
            }

            return MapToDto(entity);
        }

        public BlogCategoryDto Create(BlogCategoryInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BusinessException("Category name is required.");
            }

            var entity = new BlogCategory
            {
                Name = request.Name.Trim()
            };

            _db.BlogCategories.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public BlogCategoryDto Update(int id, BlogCategoryUpdateDto request)
        {
            if (id <= 6)
            {
                throw new BusinessException("System categories cannot be edited.");
            }

            var entity = _db.BlogCategories.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Category not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entity.Name = request.Name.Trim();
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public void Delete(int id)
        {
            if (id <= 6)
            {
                throw new BusinessException("System categories cannot be deleted.");
            }

            var entity = _db.BlogCategories.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Category not found.");
            }

            bool isUsed = _db.BlogPostCategories.Any(x => x.CategoryId == id);

            if (isUsed)
            {
                throw new BusinessException("Category is used by blog posts and cannot be deleted.");
            }

            _db.BlogCategories.Remove(entity);
            _db.SaveChanges();
        }
    }
}
