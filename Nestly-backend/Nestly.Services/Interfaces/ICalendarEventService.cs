using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface ICalendarEventService
    {
        List<CalendarEvent> Get(CalendarEventSearchObject? search);
        CalendarEvent? GetById(long id);
        CalendarEvent Create(CreateCalendarEventDto entity);
        CalendarEvent? Patch(long id, CalendarEventPatchDto patch);
        bool Delete(long id);
    }
}
