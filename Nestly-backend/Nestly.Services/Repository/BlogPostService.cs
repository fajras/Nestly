using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
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

        public PagedResult<BlogPostResponseDto> Get(BlogPostSearchObject search)
        {
            IQueryable<BlogPost> q = _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .AsQueryable();

            if (search.AuthorId is not null)
            {
                q = q.Where(p => p.AuthorId == search.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(search.Title))
            {
                q = q.Where(p => p.Title.Contains(search.Title));
            }

            if (search.CreatedFrom is not null)
            {
                q = q.Where(p => p.CreatedAt >= search.CreatedFrom.Value);
            }

            if (search.CreatedTo is not null)
            {
                q = q.Where(p => p.CreatedAt <= search.CreatedTo.Value);
            }

            if (search.CategoryId is not null)
            {
                q = q.Where(p => p.BlogPostCategories.Any(c => c.CategoryId == search.CategoryId));
            }

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(p => p.CreatedAt)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResult<BlogPostResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public BlogPostResponseDto GetById(long id)
        {
            var post = _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .FirstOrDefault(p => p.Id == id);

            if (post is null)
            {
                throw new NotFoundException("Blog post not found.");
            }

            return MapToDto(post);
        }

        public BlogPostResponseDto Create(CreateBlogPostDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new BusinessException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                throw new BusinessException("Content is required.");
            }

            if (!_db.AppUsers.Any(u => u.Id == dto.AuthorId))
            {
                throw new NotFoundException("Author not found.");
            }

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

            if (dto.CategoryIds?.Any() == true)
            {
                foreach (var cid in dto.CategoryIds)
                {
                    if (!_db.BlogCategories.Any(c => c.Id == cid))
                    {
                        throw new NotFoundException($"Category {cid} not found.");
                    }

                    _db.BlogPostCategories.Add(new BlogPostCategory
                    {
                        Post = post,
                        CategoryId = cid
                    });
                }
            }

            _db.SaveChanges();

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
        public BlogPostResponseDto Patch(long id, BlogPostPatchDto patch)
        {
            var post = _db.BlogPosts.FirstOrDefault(p => p.Id == id);

            if (post is null)
            {
                throw new NotFoundException("Blog post not found.");
            }

            if (patch.Title is not null)
            {
                post.Title = patch.Title.Trim();
            }

            if (patch.Content is not null)
            {
                post.Content = patch.Content;
            }

            if (patch.AuthorId is not null && patch.AuthorId != post.AuthorId)
            {
                if (!_db.AppUsers.Any(u => u.Id == patch.AuthorId.Value))
                {
                    throw new NotFoundException("Author not found.");
                }

                post.AuthorId = patch.AuthorId.Value;
            }

            post.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return MapToDto(post);
        }
        public void Delete(long id)
        {
            var post = _db.BlogPosts.FirstOrDefault(p => p.Id == id);

            if (post is null)
            {
                throw new NotFoundException("Blog post not found.");
            }

            if (id <= 12)
            {
                throw new BusinessException("System blog posts cannot be deleted.");
            }

            _db.BlogPosts.Remove(post);
            _db.SaveChanges();
        }
        public PagedResult<BlogPostResponseDto> GetByCategoryId(int categoryId, int page, int pageSize)
        {
            IQueryable<BlogPost> q = _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .Where(p => p.BlogPostCategories.Any(c => c.CategoryId == categoryId));

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResult<BlogPostResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
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
