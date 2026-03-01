using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeeklyAdviceController : ControllerBase
    {
        private readonly IWeeklyAdviceService _service;

        public WeeklyAdviceController(IWeeklyAdviceService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeeklyAdviceResponseDto>> Get()
            => Ok(_service.Get());

        [HttpGet("{id:int}")]
        public ActionResult<WeeklyAdviceResponseDto> GetById(int id)
        {
            var dto = _service.GetById(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpGet("week/{weekNumber:int}")]
        public ActionResult<WeeklyAdviceResponseDto> GetByWeek(short weekNumber)
        {
            var dto = _service.GetByWeek(weekNumber);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public ActionResult<WeeklyAdviceResponseDto> Create([FromBody] CreateWeeklyAdviceDto request)
        {
            try
            {
                var dto = _service.Create(request);
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
        public ActionResult<WeeklyAdviceResponseDto> Patch(int id, [FromBody] WeeklyAdvicePatchDto patch)
        {
            try
            {
                var dto = _service.Patch(id, patch);
                return dto is null ? NotFound() : Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
