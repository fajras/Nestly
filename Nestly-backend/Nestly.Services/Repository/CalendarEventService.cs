using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;

public class CalendarEventService : ICalendarEventService
{
    private readonly NestlyDbContext _db;

    public CalendarEventService(NestlyDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<CalendarEventResponseDto>> Get(CalendarEventSearchObject search)
    {
        IQueryable<CalendarEvent> q = _db.CalendarEvents.AsQueryable();

        if (search.BabyId is not null)
        {
            q = q.Where(e => e.BabyId == search.BabyId);
        }

        if (search.From is not null)
        {
            q = q.Where(e => e.StartAt >= search.From.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Title))
        {
            q = q.Where(e => e.Title.Contains(search.Title));
        }

        var totalCount = await q.CountAsync();
        int page = search.Page < 1 ? 1 : search.Page;

        int pageSize = search.PageSize < 1
            ? 10
            : search.PageSize > 100
                ? 100
                : search.PageSize;
        var entities = await q
            .OrderBy(e => e.StartAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
            .ToListAsync();

        var items = entities.Select(MapToDto).ToList();

        return new PagedResult<CalendarEventResponseDto>
        {
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<CalendarEventResponseDto> GetById(long id)
    {
        var ev = await _db.CalendarEvents.FirstOrDefaultAsync(e => e.Id == id);

        if (ev is null)
        {
            throw new NotFoundException("Calendar event not found.");
        }

        return MapToDto(ev);
    }

    public async Task<CalendarEventResponseDto> Create(
    CreateCalendarEventDto dto,
    long parentProfileId)
    {
        if (dto.BabyId <= 0)
        {
            throw new BusinessException("Baby is required.");
        }

        if (!await _db.BabyProfiles.AnyAsync(b => b.Id == dto.BabyId))
        {
            throw new NotFoundException("Baby profile not found.");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BusinessException("Title is required.");
        }

        if (dto.StartAt == default)
        {
            throw new BusinessException("Start date is required.");
        }

        if (dto.StartAt < DateTime.UtcNow)
        {
            throw new BusinessException("Start date cannot be in the past.");
        }

        var entity = new CalendarEvent
        {
            BabyId = dto.BabyId,
            UserId = parentProfileId,
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim(),
            StartAt = dto.StartAt
        };

        _db.CalendarEvents.Add(entity);
        await _db.SaveChangesAsync();

        return MapToDto(entity);
    }
    public async Task<CalendarEventResponseDto> Patch(long id, CalendarEventPatchDto patch)
    {
        var ev = await _db.CalendarEvents.FirstOrDefaultAsync(x => x.Id == id);

        if (ev is null)
        {
            throw new NotFoundException("Calendar event not found.");
        }

        if (patch.Title is not null)
        {
            ev.Title = patch.Title.Trim();
        }

        if (patch.Description is not null)
        {
            ev.Description = string.IsNullOrWhiteSpace(patch.Description)
                ? null
                : patch.Description.Trim();
        }

        if (patch.StartAt is not null)
        {
            if (patch.StartAt.Value < DateTime.UtcNow)
            {
                throw new BusinessException("Start date cannot be in the past.");
            }

            ev.StartAt = patch.StartAt.Value;
            ev.Reminder24hSent = false;
        }

        await _db.SaveChangesAsync();

        return MapToDto(ev);
    }
    public async Task Delete(long id)
    {
        var ev = await _db.CalendarEvents.FirstOrDefaultAsync(x => x.Id == id);

        if (ev is null)
        {
            throw new NotFoundException("Calendar event not found.");
        }

        ev.IsDeleted = true;
        ev.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }


    private static CalendarEventResponseDto MapToDto(CalendarEvent ev)
    {
        return new CalendarEventResponseDto
        {
            Id = ev.Id,
            BabyId = ev.BabyId,
            UserId = ev.UserId,
            Title = ev.Title,
            Description = ev.Description,
            StartAt = ev.StartAt
        };
    }
    public async Task<PagedResult<CalendarEventResponseDto>> GetByParent(
    long parentProfileId,
    CalendarEventSearchObject search)
    {
        IQueryable<CalendarEvent> q = _db.CalendarEvents
            .Where(x =>
                x.BabyProfile.ParentProfileId ==
                parentProfileId);

        if (search.BabyId is not null)
        {
            q = q.Where(x =>
                x.BabyId == search.BabyId);
        }

        if (search.From is not null)
        {
            q = q.Where(x =>
                x.StartAt >= search.From.Value);
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
            .OrderBy(x => x.StartAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = entities.Select(MapToDto).ToList();

        return new PagedResult<CalendarEventResponseDto>
        {
            TotalCount = totalCount,
            Items = items
        };
    }
}
