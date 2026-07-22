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
        // Posts seeded by BlogPostSeeder (ids 1-12) are considered system
        // posts and cannot be deleted.
        private const int SystemPostMaxId = 12;

        private readonly NestlyDbContext _db;
        private readonly RabbitMqPublisher _publisher;
        public BlogPostService(NestlyDbContext db, RabbitMqPublisher publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        public async Task<PagedResult<BlogPostResponseDto>> Get(BlogPostSearchObject search)
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

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await q
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<BlogPostResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<BlogPostResponseDto> GetById(long id)
        {
            var post = await _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
            {
                throw new NotFoundException("Blog post not found.");
            }

            return MapToDto(post);
        }
        public async Task<BlogPostResponseDto> Create(CreateBlogPostDto dto, long appUserId)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new BusinessException("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                throw new BusinessException("Content is required.");
            }

            if (dto.WeekFrom is not null && dto.WeekTo is not null &&
                dto.WeekFrom.Value > dto.WeekTo.Value)
            {
                throw new BusinessException("WeekFrom cannot be greater than WeekTo.");
            }

            var doctorProfile = await _db.DoctorProfiles
                .FirstOrDefaultAsync(d => d.UserId == appUserId);

            if (doctorProfile == null)
            {
                throw new NotFoundException("Doctor profile not found.");
            }

            var post = new BlogPost
            {
                Title = dto.Title.Trim(),
                Content = dto.Content,
                AuthorId = doctorProfile.Id,
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
                    if (!await _db.BlogCategories.AnyAsync(c => c.Id == cid))
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

            await _db.SaveChangesAsync();

            var parentIds = await _db.AppUsers
                .Where(u => u.ParentProfile != null)
                .Select(u => u.Id)
                .ToListAsync();

            foreach (var parentId in parentIds)
            {
                // Best-effort notification: RabbitMqPublisher.Publish swallows
                // and logs broker errors so a messaging outage never rolls
                // back or fails an already-saved blog post.
                _publisher.Publish(new NotificationEvent
                {
                    UserId = parentId,
                    Title = "Novi blog članak",
                    Message = $"Objavljen je novi članak: {post.Title}"
                });
            }

            return MapToDto(post);
        }
        public async Task<BlogPostResponseDto> Patch(long id, BlogPostPatchDto patch, long currentUserId)
        {
            var post = await _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
            {
                throw new NotFoundException("Blog post not found.");
            }

            var doctorProfile = await _db.DoctorProfiles
                .FirstOrDefaultAsync(d => d.UserId == currentUserId);

            if (doctorProfile == null)
            {
                throw new NotFoundException("Doctor profile not found.");
            }

            if (post.AuthorId != doctorProfile.Id)
            {
                throw new BusinessException("You can only edit your own blog posts.");
            }

            if (patch.Title is not null)
            {
                post.Title = patch.Title.Trim();
            }

            if (patch.Content is not null)
            {
                post.Content = patch.Content;
            }

            if (patch.CategoryIds is not null)
            {
                foreach (var cid in patch.CategoryIds)
                {
                    if (!await _db.BlogCategories.AnyAsync(c => c.Id == cid))
                    {
                        throw new NotFoundException($"Category {cid} not found.");
                    }
                }

                _db.BlogPostCategories.RemoveRange(post.BlogPostCategories);

                foreach (var cid in patch.CategoryIds)
                {
                    _db.BlogPostCategories.Add(new BlogPostCategory
                    {
                        PostId = post.Id,
                        CategoryId = cid
                    });
                }
            }

            post.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return MapToDto(post);
        }

        public async Task Delete(long id, long currentUserId)
        {
            var post = await _db.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
            {
                throw new NotFoundException("Blog post not found.");
            }

            var doctorProfile = await _db.DoctorProfiles
                .FirstOrDefaultAsync(d => d.UserId == currentUserId);

            if (doctorProfile == null)
            {
                throw new NotFoundException("Doctor profile not found.");
            }

            if (post.AuthorId != doctorProfile.Id)
            {
                throw new BusinessException("You can only delete your own blog posts.");
            }

            if (id <= SystemPostMaxId)
            {
                throw new BusinessException("System blog posts cannot be deleted.");
            }

            _db.BlogPosts.Remove(post);

            await _db.SaveChangesAsync();
        }
        public async Task<PagedResult<BlogPostResponseDto>> GetByCategoryId(int categoryId, int page, int pageSize)
        {
            IQueryable<BlogPost> q = _db.BlogPosts
                .Include(p => p.BlogPostCategories)
                .Where(p => p.BlogPostCategories.Any(c => c.CategoryId == categoryId));

            var totalCount = await q.CountAsync();

            var entities = await q
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

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
