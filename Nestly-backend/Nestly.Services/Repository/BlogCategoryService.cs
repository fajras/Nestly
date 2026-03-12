using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
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

        public List<BlogCategoryDto> Get(BlogCategorySearchObject? search)
        {
            IQueryable<BlogCategory> query = _db.BlogCategories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search?.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            return query
                .OrderBy(x => x.Name)
                .Select(x => MapToDto(x))
                .ToList();
        }

        public BlogCategoryDto? GetById(int id)
        {
            var entity = _db.BlogCategories
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            return entity == null ? null : MapToDto(entity);
        }

        public BlogCategoryDto Create(BlogCategoryInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }

            var entity = new BlogCategory
            {
                Name = request.Name
            };

            _db.BlogCategories.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public BlogCategoryDto? Update(int id, BlogCategoryUpdateDto request)
        {
            if (id <= 6)
            {
                throw new Exception("System categories cannot be edited.");
            }

            var entity = _db.BlogCategories.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entity.Name = request.Name;
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public bool Delete(int id)
        {
            if (id <= 6)
            {
                throw new Exception("System categories cannot be deleted.");
            }

            var entity = _db.BlogCategories.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            bool isUsed = _db.BlogPostCategories
                .Any(x => x.CategoryId == id);

            if (isUsed)
            {
                throw new Exception("Category cannot be deleted because it is used by existing blog posts.");
            }

            _db.BlogCategories.Remove(entity);
            _db.SaveChanges();

            return true;
        }
    }
}
