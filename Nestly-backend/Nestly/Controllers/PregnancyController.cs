using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PregnancyController : ControllerBase
    {
        private readonly IPregnancyService _service;
        private readonly ICurrentUserService _currentUserService;

        public PregnancyController(
            IPregnancyService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<PregnancyResponseDto>>> Get(
            [FromQuery] PregnancySearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PregnancyResponseDto>> GetMy()
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            var entity = await _service
                .GetByParentProfileId(parent.Id);

            return entity is null
                ? NotFound()
                : Ok(entity);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<PregnancyResponseDto>> Create(
            [FromBody] CreatePregnancyDto request)
        {
            var created = await _service.Create(request);

            return Ok(created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<PregnancyResponseDto>> Patch(
            long id,
            [FromBody] PregnancyPatchDto patch)
        {
            await _currentUserService
                .EnsurePregnancyOwnershipAsync(id);

            try
            {
                var updated = await _service
                    .Patch(id, patch);

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
        public async Task<IActionResult> Delete(long id)
        {
            await _currentUserService
                .EnsurePregnancyOwnershipAsync(id);

            await _service.Delete(id);

            return NoContent();
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my-status")]
        public async Task<ActionResult<PregnancyStatusDto>> GetMyStatus()
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            var status = await _service
                .GetStatus(parent.Id);

            if (status is null)
            {
                return NotFound(new
                {
                    message = "Pregnancy data not found for this parent."
                });
            }

            return Ok(status);
        }
    }
}