using Nestly.Model.DTOObjects;

public interface ICalendarEventService
{
    Task<PagedResult<CalendarEventResponseDto>> Get(CalendarEventSearchObject search);
    Task<CalendarEventResponseDto> GetById(long id);
    Task<CalendarEventResponseDto> Create(CreateCalendarEventDto entity, long currentUserId);
    Task<CalendarEventResponseDto> Patch(long id, CalendarEventPatchDto patch);
    Task Delete(long id);
    Task<PagedResult<CalendarEventResponseDto>> GetByParent(long parentProfileId, CalendarEventSearchObject search);
}
