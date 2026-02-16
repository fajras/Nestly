using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiaperLogController : ControllerBase
    {
        private readonly IDiaperLogService _service;
        public DiaperLogController(IDiaperLogService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<DiaperLogResponseDto>> Get([FromQuery] DiaperLogSearchObject? search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id:long}")]
        public ActionResult<DiaperLogResponseDto> GetById(long id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<DiaperLogResponseDto> Create([FromBody] CreateDiaperLogDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<DiaperLogResponseDto> Patch(long id, [FromBody] DiaperLogPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            return _service.Delete(id) ? NoContent() : NotFound();
        }

    }
}

