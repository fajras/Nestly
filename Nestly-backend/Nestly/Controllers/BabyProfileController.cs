using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BabyProfileController : ControllerBase
    {
        private readonly IBabyProfileService _service;

        public BabyProfileController(IBabyProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BabyProfile>> Get([FromQuery] BabyProfileSearchObject? search)
        {
            var result = _service.Get(search);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public ActionResult<BabyProfile> GetById([FromRoute] long id)
        {
            var entity = _service.GetById(id);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public ActionResult<BabyProfile> Create([FromBody] BabyProfile request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH api/BabyProfile/5
        [HttpPatch("{id:long}")]
        public ActionResult<BabyProfile> Patch([FromRoute] long id, [FromBody] BabyProfilePatchDto patch)
        {
            try
            {
                var updated = _service.Patch(id, patch);
                if (updated is null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete([FromRoute] long id)
        {
            var ok = _service.Delete(id);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

