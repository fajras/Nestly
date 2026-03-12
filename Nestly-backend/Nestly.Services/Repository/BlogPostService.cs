using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;
namespace Nestly.Services.Repository
{
    public class BlogPostService : IBlogPostService
    {
        private readonly NestlyDbContext _db;
        private readonly RabbitMqPublisher _publisher;
        public BlogPostService(NestlyDbContext db, RabbitMqPublisher publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        public IEnumerable<BlogPostResponseDto> Get(BlogPostSearchObject? search)
        {
            IQueryable<BlogPost> q = _db.BlogPosts
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

            if (search?.CategoryId is not null)
            {
                q = q.Where(p => p.BlogPostCategories
                                 .Any(c => c.CategoryId == search.CategoryId));
            }

            return q.OrderByDescending(p => p.CreatedAt)
                    .Select(MapToDto)
                    .ToList();
        }

        public BlogPostResponseDto? GetById(long id)
        {
            var post = _db.BlogPosts
                          .Include(p => p.BlogPostCategories)
                          .FirstOrDefault(p => p.Id == id);

            return post is null ? null : MapToDto(post);
        }

        public BlogPostResponseDto Create(CreateBlogPostDto dto)
        {
            var post = new BlogPost
            {
                Title = dto.Title.Trim(),
                Content = dto.Content,
                AuthorId = dto.AuthorId,
                CreatedAt = DateTime.UtcNow,
                Phase = (UserPhase)dto.Phase,
                WeekFrom = dto.WeekFrom,
                WeekTo = dto.WeekTo
            };

            _db.BlogPosts.Add(post);
            _db.SaveChanges();

            if (dto.CategoryIds?.Any() == true)
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
            var parentIds = _db.AppUsers
                .Where(u => u.ParentProfile != null)
                .Select(u => u.Id)
                .ToList();

            foreach (var parentId in parentIds)
            {
                _publisher.Publish(new NotificationEvent
                {
                    UserId = parentId,
                    Title = "Novi blog članak",
                    Message = $"Objavljen je novi članak: {post.Title}"
                });
            }
            return MapToDto(post);
        }

        public BlogPostResponseDto? Patch(long id, BlogPostPatchDto patch)
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

            return MapToDto(post);
        }

        public bool Delete(long id)
        {
            if (id <= 12)
            {
                return false;
            }
            var post = _db.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (post is null)
            {
                return false;
            }

            _db.BlogPosts.Remove(post);
            _db.SaveChanges();
            return true;
        }
        public IEnumerable<BlogPostResponseDto> GetByCategoryId(int categoryId)
        {
            return _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .Where(p => p.BlogPostCategories.Any(c => c.CategoryId == categoryId))
                .OrderByDescending(p => p.CreatedAt)
                .Select(MapToDto)
                .ToList();
        }

        private static BlogPostResponseDto MapToDto(BlogPost post)
        {
            return new BlogPostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                AuthorId = post.AuthorId,
                Phase = post.Phase,
                WeekFrom = post.WeekFrom,
                WeekTo = post.WeekTo,
                CategoryIds = post.BlogPostCategories
            .Select(c => c.CategoryId)
            .ToList()
            };
        }
    }
}
