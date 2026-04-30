using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BabyProfileController : ControllerBase
    {
        private readonly IBabyProfileService _service;

        public BabyProfileController(IBabyProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PagedResult<BabyProfileSummaryDto>> Get([FromQuery] BabyProfileSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id:long}")]
        public ActionResult<BabyProfileSummaryDto> GetById(long id)
        {
            var result = _service.GetById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public ActionResult<BabyProfileSummaryDto> Create([FromBody] CreateBabyProfileDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<BabyProfileSummaryDto> Patch(long id, [FromBody] BabyProfilePatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("latest-by-parent/{parentProfileId:long}")]
        public ActionResult<BabyProfileSummaryDto> GetLatestByParent(long parentProfileId)
        {
            var result = _service.GetLatestByParent(parentProfileId);
            return result is null ? NotFound() : Ok(result);
        }

    }
}
