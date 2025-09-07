using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QaQuestionController : ControllerBase
    {
        private readonly IQaQuestionService _service;
        public QaQuestionController(IQaQuestionService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<QaQuestion>> Get([FromQuery] QaQuestionSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<QaQuestion> GetById(long id)
            => _service.GetById(id) is { } q ? Ok(q) : NotFound();

        [HttpPost]
        public ActionResult<QaQuestion> Create([FromBody] QaQuestion request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<QaQuestion> Patch(long id, [FromBody] QaQuestionPatchDto patch)
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
        public async Task<IActionResult> Delete(long id)
            => await _service.Delete(id) ? NoContent() : NotFound();

        [HttpPost("{questionId:long}/answers")]
        public ActionResult<QaAnswer> CreateAnswer(long questionId, [FromBody] QaAnswer request)
        {
            try
            {
                var created = _service.CreateAnswer(questionId, request);
                return CreatedAtAction(nameof(QaAnswerController.GetById),
                    "QaAnswer", new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{questionId:long}/answers")]
        public ActionResult<IEnumerable<QaAnswer>> GetAnswers(long questionId)
            => Ok(_service.GetAnswers(questionId));
    }
}
