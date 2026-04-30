using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QaQuestionController : ControllerBase
    {
        private readonly IQaQuestionService _service;

        public QaQuestionController(IQaQuestionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<QaQuestionWithLatestAnswerDto>>> Get(
     [FromQuery] QaQuestionSearchObject search,
     CancellationToken ct)
        {
            var result = await _service.GetAllWithLatestAnswer(search, ct);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<QaQuestionDto>> GetById(long id, CancellationToken ct)
        {
            var dto = await _service.GetById(id, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpGet("user/{parentProfileId:long}")]
        public async Task<ActionResult<PagedResult<QaQuestionDto>>> GetByUser(
      long parentProfileId,
      [FromQuery] QaQuestionSearchObject search,
      CancellationToken ct)
        {
            var result = await _service.GetByUserAsync(parentProfileId, search, ct);
            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<QaQuestionWithLatestAnswerDto>>> GetMy(
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

        [HttpPost]
        public async Task<ActionResult<QaQuestionDto>> Create([FromBody] CreateQaQuestionDto request, CancellationToken ct)
        {
            var created = await _service.Create(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

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

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            await _service.Delete(id, ct);
            return NoContent();
        }

        [HttpGet("{questionId:long}/answers")]
        public async Task<ActionResult<PagedResult<QaAnswerDto>>> GetAnswers(
        long questionId,
        [FromQuery] QaQuestionSearchObject search,
        CancellationToken ct)
        {
            var result = await _service.GetAnswers(questionId, search, ct);
            return Ok(result);
        }

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
