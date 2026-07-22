using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;

namespace Nestly.Services.Repository
{
    public class MilestoneService : IMilestoneService
    {
        private const int MaxTitleLength = 200;

        private readonly NestlyDbContext _db;
        public MilestoneService(NestlyDbContext db) => _db = db;

        public async Task<PagedResult<MilestoneResponseDto>> Get(MilestoneSearchObject search)
        {
            IQueryable<Milestone> q = _db.Milestones.AsNoTracking();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x => x.AchievedDate >= search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x => x.AchievedDate <= search.DateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(search.Title))
            {
                q = q.Where(x => x.Title.Contains(search.Title));
            }

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await q
                .OrderBy(x => x.AchievedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(ToDto).ToList();

            return new PagedResult<MilestoneResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<MilestoneResponseDto> GetById(long id)
        {
            var entity = await _db.Milestones
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Milestone not found.");
            }

            return ToDto(entity);
        }


        public async Task<MilestoneResponseDto> Create(CreateMilestoneDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (!await _db.BabyProfiles.AnyAsync(b => b.Id == dto.BabyId))
            {
                throw new NotFoundException("Baby profile not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new BusinessException("Title is required.");
            }

            var title = dto.Title.Trim();

            if (title.Length > MaxTitleLength)
            {
                throw new BusinessException($"Title cannot exceed {MaxTitleLength} characters.");
            }

            if (dto.AchievedDate.Date > DateTime.UtcNow.Date)
            {
                throw new BusinessException("Achieved date cannot be in the future.");
            }

            bool duplicate = await _db.Milestones.AnyAsync(x =>
                x.BabyId == dto.BabyId &&
                x.AchievedDate.Date == dto.AchievedDate.Date &&
                x.Title.ToLower() == title.ToLower());

            if (duplicate)
            {
                throw new BusinessException(
                    "This milestone was already recorded for this baby on this date.");
            }

            var entity = new Milestone
            {
                BabyId = dto.BabyId,
                Title = title,
                AchievedDate = dto.AchievedDate,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _db.Milestones.Add(entity);
            await _db.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<MilestoneResponseDto> Patch(long id, MilestonePatchDto patch)
        {
            var entity = await _db.Milestones.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Milestone not found.");
            }

            if (patch.Title is not null)
            {
                if (string.IsNullOrWhiteSpace(patch.Title))
                {
                    throw new BusinessException("Title cannot be empty.");
                }

                var title = patch.Title.Trim();

                if (title.Length > MaxTitleLength)
                {
                    throw new BusinessException($"Title cannot exceed {MaxTitleLength} characters.");
                }

                entity.Title = title;
            }

            if (patch.AchievedDate is not null)
            {
                if (patch.AchievedDate.Value.Date > DateTime.UtcNow.Date)
                {
                    throw new BusinessException("Achieved date cannot be in the future.");
                }

                entity.AchievedDate = patch.AchievedDate.Value;
            }

            if (patch.Notes is not null)
            {
                entity.Notes = string.IsNullOrWhiteSpace(patch.Notes)
                    ? null
                    : patch.Notes.Trim();
            }

            await _db.SaveChangesAsync();

            return ToDto(entity);
        }
        public async Task Delete(long id)
        {
            var dbEntity = await _db.Milestones.FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Milestone not found.");
            }

            _db.Milestones.Remove(dbEntity);
            await _db.SaveChangesAsync();
        }

        private static MilestoneResponseDto ToDto(Milestone m) => new()
        {
            Id = m.Id,
            BabyId = m.BabyId,
            Title = m.Title,
            AchievedDate = m.AchievedDate,
            Notes = m.Notes,
            CreatedAt = m.CreatedAt
        };

        public async Task<PagedResult<MilestoneResponseDto>> GetByParent(
    long parentProfileId,
    MilestoneSearchObject search)
        {
            IQueryable<Milestone> q = _db.Milestones
                .AsNoTracking()
                .Where(x =>
                    x.Baby.ParentProfileId ==
                    parentProfileId);

            if (search.BabyId is not null)
            {
                q = q.Where(x =>
                    x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x =>
                    x.AchievedDate >=
                    search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x =>
                    x.AchievedDate <=
                    search.DateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(search.Title))
            {
                q = q.Where(x =>
                    x.Title.Contains(search.Title));
            }

            var totalCount = await q.CountAsync();

            int page = search.Page < 1
                ? 1
                : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var entities = await q
                .OrderByDescending(x => x.AchievedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(ToDto).ToList();

            return new PagedResult<MilestoneResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
