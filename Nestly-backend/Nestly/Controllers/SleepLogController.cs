using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SleepLogController : ControllerBase
    {
        private readonly ISleepLogService _service;

        public SleepLogController(ISleepLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PagedResult<SleepLogResponseDto>> Get([FromQuery] SleepLogSearchObject search)
       => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<SleepLogResponseDto> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity == null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<SleepLogResponseDto> Create([FromBody] CreateSleepLogDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<SleepLogResponseDto> Patch(long id, [FromBody] SleepLogPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}