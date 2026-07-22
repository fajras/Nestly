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
        // Categories seeded by BlogCategorySeeder (ids 1-6) are considered
        // system categories and cannot be renamed or deleted.
        private const int SystemCategoryMaxId = 6;

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

        public async Task<PagedResult<BlogCategoryDto>> Get(BlogCategorySearchObject search)
        {
            IQueryable<BlogCategory> query = _db.BlogCategories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            var totalCount = await query.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<BlogCategoryDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<BlogCategoryDto> GetById(int id)
        {
            var entity = await _db.BlogCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Category not found.");
            }

            return MapToDto(entity);
        }

        public async Task<BlogCategoryDto> Create(BlogCategoryInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BusinessException("Category name is required.");
            }

            var name = request.Name.Trim();

            bool duplicate = await _db.BlogCategories
                .AnyAsync(x => x.Name.ToLower() == name.ToLower());

            if (duplicate)
            {
                throw new BusinessException("A category with this name already exists.");
            }

            var entity = new BlogCategory
            {
                Name = name
            };

            _db.BlogCategories.Add(entity);
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<BlogCategoryDto> Update(int id, BlogCategoryUpdateDto request)
        {
            if (id <= SystemCategoryMaxId)
            {
                throw new BusinessException("System categories cannot be edited.");
            }

            var entity = await _db.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Category not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();

                bool duplicate = await _db.BlogCategories
                    .AnyAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());

                if (duplicate)
                {
                    throw new BusinessException("A category with this name already exists.");
                }

                entity.Name = name;
            }

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task Delete(int id)
        {
            if (id <= SystemCategoryMaxId)
            {
                throw new BusinessException("System categories cannot be deleted.");
            }

            var entity = await _db.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Category not found.");
            }

            bool isUsed = await _db.BlogPostCategories.AnyAsync(x => x.CategoryId == id);

            if (isUsed)
            {
                throw new BusinessException("Category is used by blog posts and cannot be deleted.");
            }

            _db.BlogCategories.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
