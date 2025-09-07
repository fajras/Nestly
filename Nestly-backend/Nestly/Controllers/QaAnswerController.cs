using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QaAnswerController : ControllerBase
    {
        private readonly IQaAnswerService _service;
        public QaAnswerController(IQaAnswerService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<QaAnswer>> Get([FromQuery] QaAnswerSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<QaAnswer> GetById(long id)
            => _service.GetById(id) is { } a ? Ok(a) : NotFound();

        [HttpPost]
        public ActionResult<QaAnswer> Create([FromBody] CreateQaAnswerDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<QaAnswer> Patch(long id, [FromBody] QaAnswerPatchDto patch)
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
