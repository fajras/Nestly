using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FetalDevelopmentWeekController : ControllerBase
    {
        private readonly IFetalDevelopmentWeekService _service;
        public FetalDevelopmentWeekController(IFetalDevelopmentWeekService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<FetalDevelopmentWeek>> Get()
            => Ok(_service.Get());

        [HttpGet("{id:int}")]
        public ActionResult<FetalDevelopmentWeek> GetById(int id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<FetalDevelopmentWeek> Create([FromBody] CreateFetalDevelopmentWeekDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}")]
        public ActionResult<FetalDevelopmentWeek> Patch(int id, [FromBody] FetalDevelopmentWeekPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
