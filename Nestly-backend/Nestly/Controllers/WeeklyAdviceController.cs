using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeeklyAdviceController : ControllerBase
    {
        private readonly IWeeklyAdviceService _service;

        public WeeklyAdviceController(IWeeklyAdviceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<WeeklyAdviceResponseDto>>> Get([FromQuery] WeeklyAdviceSearchObject search)
    => Ok(await _service.Get(search));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WeeklyAdviceResponseDto>> GetById(int id)
        {
            var dto = await _service.GetById(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpGet("week/{weekNumber:int}")]
        public async Task<ActionResult<WeeklyAdviceResponseDto>> GetByWeek(short weekNumber)
        {
            var dto = await _service.GetByWeek(weekNumber);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<WeeklyAdviceResponseDto>> Create([FromBody] CreateWeeklyAdviceDto request)
        {
            try
            {
                var dto = await _service.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<WeeklyAdviceResponseDto>> Patch(int id, [FromBody] WeeklyAdvicePatchDto patch)
        {
            try
            {
                var dto = await _service.Patch(id, patch);
                return dto is null ? NotFound() : Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
