using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QaQuestionController : ControllerBase
    {
        private readonly IQaQuestionService _service;
        public QaQuestionController(IQaQuestionService service) => _service = service;

        // GET /api/qaquestion?...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QaQuestion>>> Get([FromQuery] QaQuestionSearchObject? search)
        {
            var result = await _service.Get(search);
            return Ok(result);
        }

        // GET /api/qaquestion/{id}
        [HttpGet("{id:long}")]
        public async Task<ActionResult<QaQuestion>> GetById(long id)
        {
            var q = await _service.GetById(id);
            return q is null ? NotFound() : Ok(q);
        }

        // GET /api/qaquestion/user/{userId}
        [HttpGet("user/{userId:long}")]
        public async Task<ActionResult<IEnumerable<QaQuestion>>> GetByUser(long userId)
        {
            var result = await _service.GetByUserAsync(userId);

            if (result == null || !result.Any())
            {
                return NotFound(new { message = $"No questions found for user id {userId}." });
            }

            return Ok(result);
        }

        // POST /api/qaquestion
        [HttpPost]
        public async Task<ActionResult<QaQuestion>> Create([FromBody] CreateQaQuestionDto request)
        {
            var created = await _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH /api/qaquestion/{id}
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<QaQuestion>> Patch(long id, [FromBody] QaQuestionPatchDto patch)
        {
            try
            {
                var updated = await _service.Patch(id, patch);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/qaquestion/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
            => await _service.Delete(id) ? NoContent() : NotFound();

        // POST /api/qaquestion/{questionId}/answers
        [HttpPost("{questionId:long}/answers")]
        public async Task<ActionResult<QaAnswer>> CreateAnswer(long questionId, [FromBody] QaAnswer request)
        {
            try
            {
                var created = await _service.CreateAnswer(questionId, request);
                return CreatedAtAction(nameof(QaAnswerController.GetById),
                    "QaAnswer", new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/qaquestion/{questionId}/answers
        [HttpGet("{questionId:long}/answers")]
        public async Task<ActionResult<IEnumerable<QaAnswer>>> GetAnswers(long questionId)
        {
            var answers = await _service.GetAnswers(questionId);
            return Ok(answers);
        }
    }
}
