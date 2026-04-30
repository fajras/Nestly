using Nestly.Model.DTOObjects;

public interface ICalendarEventService
{
    PagedResult<CalendarEventResponseDto> Get(CalendarEventSearchObject search);
    CalendarEventResponseDto GetById(long id);
    CalendarEventResponseDto Create(CreateCalendarEventDto entity);
    CalendarEventResponseDto Patch(long id, CalendarEventPatchDto patch);
    void Delete(long id);
}
