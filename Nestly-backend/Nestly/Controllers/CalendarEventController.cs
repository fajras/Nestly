using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CalendarEventController : ControllerBase
    {
        private readonly ICalendarEventService _service;
        private readonly ICurrentUserService _currentUserService;

        public CalendarEventController(
            ICalendarEventService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<CalendarEventResponseDto>> Get(
            [FromQuery] CalendarEventSearchObject search)
        {
            return Ok(
                _service.Get(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<CalendarEventResponseDto>>> GetMy(
            [FromQuery] CalendarEventSearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            if (search.BabyId.HasValue)
            {
                await _currentUserService
                    .EnsureBabyOwnershipAsync(
                        search.BabyId.Value);
            }

            return Ok(
                _service.GetByParent(
                    parent.Id,
                    search));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CalendarEventResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureCalendarEventOwnershipAsync(id);

            var result =
                _service.GetById(id);

            return Ok(result);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<CalendarEventResponseDto>> Create(
            [FromBody] CreateCalendarEventDto request)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(
                    request.BabyId);

            var userId =
                _currentUserService
                    .GetCurrentAppUserId();

            var created =
                _service.Create(
                    request,
                    userId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<CalendarEventResponseDto>> Patch(
            long id,
            [FromBody] CalendarEventPatchDto patch)
        {
            await _currentUserService
                .EnsureCalendarEventOwnershipAsync(id);

            var updated =
                _service.Patch(id, patch);

            return Ok(updated);
        }

        [Authorize(Roles = "Parent")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id)
        {
            await _currentUserService
                .EnsureCalendarEventOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }
    }
}