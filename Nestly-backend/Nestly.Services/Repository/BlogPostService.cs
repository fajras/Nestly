using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class BlogPostService : IBlogPostService
    {
        private readonly NestlyDbContext _db;
        public BlogPostService(NestlyDbContext db) => _db = db;

        public List<BlogPost> Get(BlogPostSearchObject? search)
        {
            IQueryable<BlogPost> q = _db.BlogPosts
                                        .Include(p => p.Author)
                                        .Include(p => p.BlogPostCategories)
                                        .AsQueryable();

            if (search?.AuthorId is not null)
            {
                q = q.Where(p => p.AuthorId == search.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(search?.Title))
            {
                q = q.Where(p => p.Title.Contains(search.Title));
            }

            if (search?.CreatedFrom is not null)
            {
                q = q.Where(p => p.CreatedAt >= search.CreatedFrom.Value);
            }

            if (search?.CreatedTo is not null)
            {
                q = q.Where(p => p.CreatedAt <= search.CreatedTo.Value);
            }
            if (search.CategoryId.HasValue)
            {
                var catId = search.CategoryId.Value;
                q = q.Where(p => p.BlogPostCategories
                    .Any(c => c.CategoryId == catId));
            }

            return q.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public BlogPost? GetById(long id)
        {
            return _db.BlogPosts
                      .Include(p => p.Author)
                      .Include(p => p.BlogPostCategories)
                      .FirstOrDefault(p => p.Id == id);
        }

        public BlogPost Create(CreateBlogPostDto dto)
        {
            var post = new BlogPost
            {
                Title = dto.Title.Trim(),
                Content = dto.Content,
                AuthorId = dto.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            _db.BlogPosts.Add(post);
            _db.SaveChanges();

            if (dto.CategoryIds.Any())
            {
                _db.BlogPostCategories.AddRange(
                    dto.CategoryIds.Select(cid => new BlogPostCategory
                    {
                        PostId = post.Id,
                        CategoryId = cid
                    })
                );
                _db.SaveChanges();
            }

            return post;
        }



        public BlogPost? Patch(long id, BlogPostPatchDto patch)
        {
            var post = _db.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (post is null)
            {
                return null;
            }

            if (patch.Title is not null)
            {
                post.Title = patch.Title;
            }

            if (patch.Content is not null)
            {
                post.Content = patch.Content;
            }

            if (patch.AuthorId is not null && patch.AuthorId != post.AuthorId)
            {
                if (!_db.AppUsers.Any(u => u.Id == patch.AuthorId.Value))
                {
                    throw new ArgumentException("Author does not exist.");
                }

                post.AuthorId = patch.AuthorId.Value;
            }

            post.UpdatedAt = DateTime.UtcNow;

            _db.SaveChanges();
            return post;
        }

        public bool Delete(long id)
        {
            var post = _db.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (post is null)
            {
                return false;
            }

            _db.BlogPosts.Remove(post);
            _db.SaveChanges();
            return true;
        }

        public async Task<List<BlogCategoryDto>> GetAllAsync()
        {
            return await _db.BlogCategories
                .AsNoTracking()
                .Select(c => new BlogCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public List<BlogPost> GetByCategoryId(int categoryId)
        {
            return _db.BlogPosts
                .Include(p => p.Author)
                .Include(p => p.BlogPostCategories)
                .Where(p => p.BlogPostCategories.Any(c => c.CategoryId == categoryId))
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

    }
}
