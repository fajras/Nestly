using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiaperLogController : ControllerBase
    {
        private readonly IDiaperLogService _service;
        public DiaperLogController(IDiaperLogService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<DiaperLog>> Get([FromQuery] DiaperLogSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<DiaperLog> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<DiaperLog> Create([FromBody] CreateDiaperLogDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<DiaperLog> Patch(long id, [FromBody] DiaperLogPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}

