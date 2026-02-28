using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;

public class CalendarEventService : ICalendarEventService
{
    private readonly NestlyDbContext _db;

    public CalendarEventService(NestlyDbContext db)
    {
        _db = db;
    }

    public IEnumerable<CalendarEventResponseDto> Get(CalendarEventSearchObject? search)
    {
        IQueryable<CalendarEvent> q = _db.CalendarEvents.AsQueryable();

        if (search?.BabyId is not null)
        {
            q = q.Where(e => e.BabyId == search.BabyId);
        }

        if (search?.UserId is not null)
        {
            q = q.Where(e => e.UserId == search.UserId);
        }

        if (search?.From is not null)
        {
            q = q.Where(e => e.StartAt >= search.From.Value);
        }

        if (!string.IsNullOrWhiteSpace(search?.Title))
        {
            q = q.Where(e => e.Title.Contains(search.Title));
        }

        return q.OrderBy(e => e.StartAt)
                .Select(MapToDto)
                .ToList();
    }

    public CalendarEventResponseDto? GetById(long id)
    {
        var ev = _db.CalendarEvents.FirstOrDefault(e => e.Id == id);
        return ev is null ? null : MapToDto(ev);
    }

    public CalendarEventResponseDto Create(CreateCalendarEventDto dto)
    {
        if (dto.BabyId <= 0)
        {
            throw new ArgumentException("BabyId is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException("Title is required.");
        }

        if (dto.StartAt == default)
        {
            throw new ArgumentException("StartAt is required.");
        }

        var entity = new CalendarEvent
        {
            BabyId = dto.BabyId,
            UserId = dto.UserId,
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            StartAt = dto.StartAt
        };

        _db.CalendarEvents.Add(entity);
        _db.SaveChanges();

        return MapToDto(entity);
    }

    public CalendarEventResponseDto? Patch(long id, CalendarEventPatchDto patch)
    {
        var ev = _db.CalendarEvents.FirstOrDefault(x => x.Id == id);
        if (ev is null)
        {
            return null;
        }

        if (patch.Title is not null)
        {
            ev.Title = patch.Title;
        }

        if (patch.Description is not null)
        {
            ev.Description = patch.Description;
        }

        if (patch.StartAt is not null)
        {
            ev.StartAt = patch.StartAt.Value;
            ev.Reminder24hSent = false;
        }

        _db.SaveChanges();

        return MapToDto(ev);
    }

    public bool Delete(long id)
    {
        var ev = _db.CalendarEvents.FirstOrDefault(x => x.Id == id);
        if (ev is null)
        {
            return false;
        }

        _db.CalendarEvents.Remove(ev);
        _db.SaveChanges();
        return true;
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
