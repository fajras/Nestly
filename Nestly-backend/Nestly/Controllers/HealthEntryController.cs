using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthEntryController : ControllerBase
    {
        private readonly IHealthEntryService _service;
        public HealthEntryController(IHealthEntryService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<HealthEntry>> Get([FromQuery] HealthEntrySearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<HealthEntry> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<HealthEntry> Create([FromBody] HealthEntry request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<HealthEntry> Patch(long id, [FromBody] HealthEntryPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
