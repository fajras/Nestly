using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PregnancyController : ControllerBase
    {
        private readonly IPregnancyService _service;
        public PregnancyController(IPregnancyService service) => _service = service;

        [HttpGet]
        public ActionResult<PagedResult<PregnancyResponseDto>> Get([FromQuery] PregnancySearchObject search)
     => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<PregnancyResponseDto> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpGet("by-parent/{parentProfileId:long}")]
        public ActionResult<PregnancyResponseDto> GetByParentProfileId(long parentProfileId)
        {
            var entity = _service.GetByParentProfileId(parentProfileId);

            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<PregnancyResponseDto> Create([FromBody] CreatePregnancyDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<PregnancyResponseDto> Patch(long id, [FromBody] PregnancyPatchDto patch)
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


        [HttpGet("status")]
        public ActionResult<PregnancyStatusDto> GetStatus([FromQuery] long parentProfileId)
        {
            var status = _service.GetStatus(parentProfileId);

            if (status is null)
            {
                return NotFound(new { message = "Pregnancy data not found for this parent." });
            }

            return Ok(status);
        }
    }
}
