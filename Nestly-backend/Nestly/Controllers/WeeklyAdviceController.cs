using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeeklyAdviceController : ControllerBase
    {
        private readonly IWeeklyAdviceService _service;
        public WeeklyAdviceController(IWeeklyAdviceService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<WeeklyAdvice>> Get()
            => Ok(_service.Get());

        [HttpGet("{id:int}")]
        public ActionResult<WeeklyAdvice> GetById(int id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpGet("week/{weekNumber:int}")]
        public ActionResult<GetWeeklyAdviceDto> GetByWeek(short weekNumber)
        {
            var entity = _service.GetByWeek(weekNumber);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<WeeklyAdvice> Create([FromBody] CreateWeeklyAdviceDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}")]
        public ActionResult<WeeklyAdvice> Patch(int id, [FromBody] WeeklyAdvicePatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
