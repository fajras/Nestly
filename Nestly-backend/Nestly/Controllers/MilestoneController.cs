using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MilestoneController : ControllerBase
    {
        private readonly IMilestoneService _service;
        private readonly ICurrentUserService _currentUserService;

        public MilestoneController(
            IMilestoneService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<MilestoneResponseDto>> Get(
            [FromQuery] MilestoneSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MilestoneResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureMilestoneOwnershipAsync(id);

            var entity = _service.GetById(id);

            return Ok(entity);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<MilestoneResponseDto>> Create(
            [FromBody] CreateMilestoneDto request)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(request.BabyId);

            var created = _service.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<MilestoneResponseDto>> Patch(
            long id,
            [FromBody] MilestonePatchDto patch)
        {
            await _currentUserService
                .EnsureMilestoneOwnershipAsync(id);

            var updated = _service.Patch(id, patch);

            return Ok(updated);
        }

        [Authorize(Roles = "Parent")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _currentUserService
                .EnsureMilestoneOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<MilestoneResponseDto>>> GetMy(
            [FromQuery] MilestoneSearchObject search)
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
    }
}