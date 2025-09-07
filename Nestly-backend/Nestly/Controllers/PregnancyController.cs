using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PregnancyController : ControllerBase
    {
        private readonly IPregnancyService _service;
        public PregnancyController(IPregnancyService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<Pregnancy>> Get([FromQuery] PregnancySearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<Pregnancy> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<Pregnancy> Create([FromBody] CreatePregnancyDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<Pregnancy> Patch(long id, [FromBody] PregnancyPatchDto patch)
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
