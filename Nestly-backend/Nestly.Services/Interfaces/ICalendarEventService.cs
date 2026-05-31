using Nestly.Model.DTOObjects;

public interface ICalendarEventService
{
    PagedResult<CalendarEventResponseDto> Get(CalendarEventSearchObject search);
    CalendarEventResponseDto GetById(long id);
    CalendarEventResponseDto Create(CreateCalendarEventDto entity, long currentUserId);
    CalendarEventResponseDto Patch(long id, CalendarEventPatchDto patch);
    void Delete(long id);
    PagedResult<CalendarEventResponseDto> GetByParent(long parentProfileId, CalendarEventSearchObject search);
}
