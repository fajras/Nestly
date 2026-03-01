using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MilestoneController : ControllerBase
    {
        private readonly IMilestoneService _service;
        public MilestoneController(IMilestoneService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<MilestoneResponseDto>> Get([FromQuery] MilestoneSearchObject? search)
     => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<MilestoneResponseDto> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<MilestoneResponseDto> Create([FromBody] CreateMilestoneDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<MilestoneResponseDto> Patch(long id, [FromBody] MilestonePatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();

    }
}
