using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DiaperLogController : ControllerBase
    {
        private readonly IDiaperLogService _service;
        private readonly ICurrentUserService _currentUserService;

        public DiaperLogController(
            IDiaperLogService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<DiaperLogResponseDto>> Get(
            [FromQuery] DiaperLogSearchObject search)
        {
            return Ok(
                _service.Get(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<DiaperLogResponseDto>>> GetMy(
            [FromQuery] DiaperLogSearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            if (search.BabyId.HasValue)
            {
                await _currentUserService
                    .EnsureBabyOwnershipAsync(
                        search.BabyId.Value);
            }

            return Ok(
                _service.GetByParent(
                    parent.Id,
                    search));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<DiaperLogResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureDiaperLogOwnershipAsync(id);

            var result =
                _service.GetById(id);

            return Ok(result);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<DiaperLogResponseDto>> Create(
            [FromBody] CreateDiaperLogDto request)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(
                    request.BabyId);

            var created =
                _service.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<DiaperLogResponseDto>> Patch(
            long id,
            [FromBody] DiaperLogPatchDto patch)
        {
            await _currentUserService
                .EnsureDiaperLogOwnershipAsync(id);

            var updated =
                _service.Patch(id, patch);

            return Ok(updated);
        }

        [Authorize(Roles = "Parent")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id)
        {
            await _currentUserService
                .EnsureDiaperLogOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }
    }
}