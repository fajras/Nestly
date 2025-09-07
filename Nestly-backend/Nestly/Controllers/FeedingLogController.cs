using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingLogController : ControllerBase
    {
        private readonly IFeedingLogService _service;

        public FeedingLogController(IFeedingLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FeedingLog>> Get([FromQuery] FeedingLogSearchObject? search)
        {
            var result = _service.Get(search);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public ActionResult<FeedingLog> GetById(long id)
        {
            var entity = _service.GetById(id);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public ActionResult<FeedingLog> Create([FromBody] FeedingLog request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH api/FeedingLog/5
        [HttpPatch("{id:long}")]
        public ActionResult<FeedingLog> Patch(long id, [FromBody] FeedingLogPatchDto patch)
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
        public IActionResult Delete(long id)
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
