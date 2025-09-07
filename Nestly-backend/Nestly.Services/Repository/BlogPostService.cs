using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
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

            return q.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public BlogPost? GetById(long id)
        {
            return _db.BlogPosts
                      .Include(p => p.Author)
                      .Include(p => p.BlogPostCategories)
                      .FirstOrDefault(p => p.Id == id);
        }

        public BlogPost Create(BlogPost entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(entity.Content))
            {
                throw new ArgumentException("Content is required.");
            }

            if (entity.AuthorId is not null &&
                !_db.AppUsers.Any(u => u.Id == entity.AuthorId.Value))
            {
                throw new ArgumentException("Author does not exist.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            _db.BlogPosts.Add(entity);
            _db.SaveChanges();

            return entity;
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
    }
}
