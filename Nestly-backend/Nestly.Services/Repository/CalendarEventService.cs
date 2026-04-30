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

    public PagedResult<CalendarEventResponseDto> Get(CalendarEventSearchObject search)
    {
        IQueryable<CalendarEvent> q = _db.CalendarEvents.AsQueryable();

        if (search.BabyId is not null)
        {
            q = q.Where(e => e.BabyId == search.BabyId);
        }

        if (search.UserId is not null)
        {
            q = q.Where(e => e.UserId == search.UserId);
        }

        if (search.From is not null)
        {
            q = q.Where(e => e.StartAt >= search.From.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.Title))
        {
            q = q.Where(e => e.Title.Contains(search.Title));
        }

        var totalCount = q.Count();

        var items = q
            .OrderBy(e => e.StartAt)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResult<CalendarEventResponseDto>
        {
            TotalCount = totalCount,
            Items = items
        };
    }

    public CalendarEventResponseDto GetById(long id)
    {
        var ev = _db.CalendarEvents.FirstOrDefault(e => e.Id == id);

        if (ev is null)
        {
            throw new NotFoundException("Calendar event not found.");
        }

        return MapToDto(ev);
    }

    public CalendarEventResponseDto Create(CreateCalendarEventDto dto)
    {
        if (dto.BabyId <= 0)
        {
            throw new BusinessException("Baby is required.");
        }

        if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
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

        var entity = new CalendarEvent
        {
            BabyId = dto.BabyId,
            UserId = dto.UserId,
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim(),
            StartAt = dto.StartAt
        };

        _db.CalendarEvents.Add(entity);
        _db.SaveChanges();

        return MapToDto(entity);
    }
    public CalendarEventResponseDto Patch(long id, CalendarEventPatchDto patch)
    {
        var ev = _db.CalendarEvents.FirstOrDefault(x => x.Id == id);

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
            ev.StartAt = patch.StartAt.Value;
            ev.Reminder24hSent = false;
        }

        _db.SaveChanges();

        return MapToDto(ev);
    }
    public void Delete(long id)
    {
        var ev = _db.CalendarEvents.FirstOrDefault(x => x.Id == id);

        if (ev is null)
        {
            throw new NotFoundException("Calendar event not found.");
        }

        _db.CalendarEvents.Remove(ev);
        _db.SaveChanges();
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
}
