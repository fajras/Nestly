using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QaQuestionController : ControllerBase
    {
        private readonly IQaQuestionService _service;

        public QaQuestionController(IQaQuestionService service)
        {
            _service = service;
        }

        // GET /api/qaquestion
        // Admin pregled, moze filter, moze samo neodgovorena
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QaQuestionWithLatestAnswerDto>>> Get(
            [FromQuery] QaQuestionSearchObject? search,
            CancellationToken ct)
        {
            var result = await _service.GetAllWithLatestAnswer(search, ct);
            return Ok(result);
        }

        // GET /api/qaquestion/{id}
        [HttpGet("{id:long}")]
        public async Task<ActionResult<QaQuestionDto>> GetById(long id, CancellationToken ct)
        {
            var dto = await _service.GetById(id, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        // GET /api/qaquestion/user/{parentProfileId}
        [HttpGet("user/{parentProfileId:long}")]
        public async Task<ActionResult<IEnumerable<QaQuestionDto>>> GetByUser(long parentProfileId, CancellationToken ct)
        {
            var result = await _service.GetByUserAsync(parentProfileId, ct);
            return Ok(result);
        }

        // GET /api/qaquestion/my?AskedById=123
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<QaQuestionWithLatestAnswerDto>>> GetMy(
            [FromQuery] QaQuestionSearchObject search,
            CancellationToken ct)
        {
            if (search.AskedById is null || search.AskedById.Value <= 0)
            {
                return BadRequest(new { message = "AskedById is required." });
            }

            var result = await _service.GetWithLatestAnswerForUser(search, ct);
            return Ok(result);
        }

        // POST /api/qaquestion
        [HttpPost]
        public async Task<ActionResult<QaQuestionDto>> Create([FromBody] CreateQaQuestionDto request, CancellationToken ct)
        {
            var created = await _service.Create(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH /api/qaquestion/{id}
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<QaQuestionDto>> Patch(long id, [FromBody] QaQuestionPatchDto patch, CancellationToken ct)
        {
            try
            {
                var updated = await _service.Patch(id, patch, ct);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/qaquestion/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var ok = await _service.Delete(id, ct);
            return ok ? NoContent() : NotFound();
        }

        // GET /api/qaquestion/{questionId}/answers
        [HttpGet("{questionId:long}/answers")]
        public async Task<ActionResult<IEnumerable<QaAnswerDto>>> GetAnswers(long questionId, CancellationToken ct)
        {
            var answers = await _service.GetAnswers(questionId, ct);
            return Ok(answers);
        }

        // POST /api/qaquestion/{questionId}/answers
        [HttpPost("{questionId:long}/answers")]
        public async Task<ActionResult<QaAnswerDto>> CreateAnswer(
            long questionId,
            [FromBody] CreateQaAnswerDto request,
            CancellationToken ct)
        {
            try
            {
                var created = await _service.CreateAnswer(questionId, request, ct);
                return CreatedAtAction(nameof(GetAnswers), new { questionId = questionId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
