using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HealthEntryController : ControllerBase
    {
        private readonly IHealthEntryService _service;
        private readonly ICurrentUserService _currentUserService;

        public HealthEntryController(
            IHealthEntryService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<HealthEntryResponseDto>> Get(
            [FromQuery] HealthEntrySearchObject search)
        {
            return Ok(
                _service.Get(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<HealthEntryResponseDto>>> GetMy(
            [FromQuery] HealthEntrySearchObject search)
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
        public async Task<ActionResult<HealthEntryResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureHealthEntryOwnershipAsync(id);

            var entity =
                _service.GetById(id);

            return Ok(entity);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<HealthEntryResponseDto>> Create(
            [FromBody] CreateHealthEntryDto request)
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
        public async Task<ActionResult<HealthEntryResponseDto>> Patch(
            long id,
            [FromBody] HealthEntryPatchDto patch)
        {
            await _currentUserService
                .EnsureHealthEntryOwnershipAsync(id);

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
                .EnsureHealthEntryOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }
    }
}