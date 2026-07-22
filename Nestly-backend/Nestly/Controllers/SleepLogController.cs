using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SleepLogController : ControllerBase
    {
        private readonly ISleepLogService _service;
        private readonly ICurrentUserService _currentUserService;

        public SleepLogController(ISleepLogService service, ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<SleepLogResponseDto>>> Get(
    [FromQuery] SleepLogSearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<SleepLogResponseDto>> GetById(
     long id)
        {
            await _currentUserService
                .EnsureSleepLogOwnershipAsync(id);

            var entity = await _service.GetById(id);

            return Ok(entity);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<SleepLogResponseDto>> Create(
    [FromBody] CreateSleepLogDto request)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(request.BabyId);

            var created = await _service.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<SleepLogResponseDto>> Patch(
       long id,
       [FromBody] SleepLogPatchDto patch)
        {
            await _currentUserService
                .EnsureSleepLogOwnershipAsync(id);

            var updated = await _service.Patch(id, patch);

            return Ok(updated);
        }

        [Authorize(Roles = "Parent")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _currentUserService
                .EnsureSleepLogOwnershipAsync(id);

            await _service.Delete(id);

            return NoContent();
        }
        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<SleepLogResponseDto>>> GetMy(
    [FromQuery] SleepLogSearchObject search)
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
                await _service.GetByParent(
                    parent.Id,
                    search));
        }
    }
}
