using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarEventController : ControllerBase
    {
        private readonly ICalendarEventService _service;
        public CalendarEventController(ICalendarEventService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<CalendarEvent>> Get([FromQuery] CalendarEventSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<CalendarEvent> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<CalendarEvent> Create([FromBody] CalendarEvent request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<CalendarEvent> Patch(long id, [FromBody] CalendarEventPatchDto patch)
        {
            try
            {
                var updated = _service.Patch(id, patch);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
