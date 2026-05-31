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
        private readonly ICurrentUserService _currentUserService;

        public QaQuestionController(
            IQaQuestionService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<QaQuestionWithLatestAnswerDto>>> Get(
            [FromQuery] QaQuestionSearchObject search,
            CancellationToken ct)
        {
            var result =
                await _service.GetAllWithLatestAnswer(
                    search,
                    ct);

            return Ok(result);
        }
        [HttpGet("{id:long}")]
        public async Task<ActionResult<QaQuestionDto>> GetById(
            long id,
            CancellationToken ct)
        {
            await _currentUserService
                .EnsureQaQuestionOwnershipAsync(id);

            var dto =
                await _service.GetById(id, ct);

            return Ok(dto);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("user/{parentProfileId:long}")]
        public async Task<ActionResult<PagedResult<QaQuestionDto>>> GetByUser(
            long parentProfileId,
            [FromQuery] QaQuestionSearchObject search,
            CancellationToken ct)
        {
            var result =
                await _service.GetByUserAsync(
                    parentProfileId,
                    search,
                    ct);

            return Ok(result);
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<QaQuestionWithLatestAnswerDto>>> GetMy(
            [FromQuery] QaQuestionSearchObject search,
            CancellationToken ct)
        {
            var currentUserId =
                _currentUserService
                    .GetCurrentAppUserId();

            search.AskedByUserId =
                currentUserId;

            var result =
                await _service
                    .GetWithLatestAnswerForUser(
                        search,
                        ct);

            return Ok(result);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<QaQuestionDto>> Create(
            [FromBody] CreateQaQuestionDto request,
            CancellationToken ct)
        {
            request.CurrentUserId =
                _currentUserService
                    .GetCurrentAppUserId();

            var created =
                await _service.Create(
                    request,
                    ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<QaQuestionDto>> Patch(
            long id,
            [FromBody] QaQuestionPatchDto patch,
            CancellationToken ct)
        {
            await _currentUserService
                .EnsureQaQuestionOwnershipAsync(id);

            try
            {
                var updated =
                    await _service.Patch(
                        id,
                        patch,
                        ct);

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Parent")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken ct)
        {
            await _currentUserService
                .EnsureQaQuestionOwnershipAsync(id);

            await _service.Delete(id, ct);

            return NoContent();
        }

        [Authorize(Roles = "Parent,Doctor")]
        [HttpGet("{questionId:long}/answers")]
        public async Task<ActionResult<PagedResult<QaAnswerDto>>> GetAnswers(
            long questionId,
            [FromQuery] QaQuestionSearchObject search,
            CancellationToken ct)
        {
            await _currentUserService
                .EnsureQaQuestionOwnershipAsync(questionId);

            var result =
                await _service.GetAnswers(
                    questionId,
                    search,
                    ct);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("{questionId:long}/answers")]
        public async Task<ActionResult<QaAnswerDto>> CreateAnswer(
            long questionId,
            [FromBody] CreateQaAnswerDto request,
            CancellationToken ct)
        {
            request.CurrentUserId =
                _currentUserService
                    .GetCurrentAppUserId();

            try
            {
                var created =
                    await _service.CreateAnswer(
                        questionId,
                        request,
                        ct);

                return CreatedAtAction(
                    nameof(GetAnswers),
                    new { questionId },
                    created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}