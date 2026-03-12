using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FeedingLogController : ControllerBase
    {
        private readonly IFeedingLogService _service;

        public FeedingLogController(IFeedingLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FeedingLogResponseDto>> Get([FromQuery] FeedingLogSearchObject? search)
     => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<FeedingLogResponseDto> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<FeedingLogResponseDto> Create([FromBody] CreateFeedingLogDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<FeedingLogResponseDto> Patch(long id, [FromBody] FeedingLogPatchDto patch)
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
        {
            var deleted = _service.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
