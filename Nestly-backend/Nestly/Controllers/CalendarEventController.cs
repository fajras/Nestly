using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalendarEventController : ControllerBase
    {
        private readonly ICalendarEventService _service;
        public CalendarEventController(ICalendarEventService service) => _service = service;

        [HttpGet]
        public ActionResult<PagedResult<CalendarEventResponseDto>> Get([FromQuery] CalendarEventSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id:long}")]
        public ActionResult<CalendarEventResponseDto> GetById(long id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<CalendarEventResponseDto> Create([FromBody] CreateCalendarEventDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<CalendarEventResponseDto> Patch(long id, [FromBody] CalendarEventPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }

    }
}
