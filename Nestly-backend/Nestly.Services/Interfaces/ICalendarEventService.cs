using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface ICalendarEventService
    {
        List<CalendarEvent> Get(CalendarEventSearchObject? search);
        CalendarEvent? GetById(long id);
        CalendarEvent Create(CalendarEvent entity);
        CalendarEvent? Patch(long id, CalendarEventPatchDto patch);
        bool Delete(long id);
    }
}
