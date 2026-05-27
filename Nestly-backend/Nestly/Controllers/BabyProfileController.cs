using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BabyProfileController : ControllerBase
    {
        private readonly IBabyProfileService _service;
        private readonly ICurrentUserService _currentUserService;

        public BabyProfileController(
            IBabyProfileService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<BabyProfileSummaryDto>>> Get(
            [FromQuery] BabyProfileSearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [HttpGet("my-latest")]
        public async Task<ActionResult<BabyProfileSummaryDto>> GetMyLatest()
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            var result = await _service
                .GetLatestByParent(parent.Id);

            return result is null
                ? NotFound()
                : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BabyProfileSummaryDto>> Create(
            [FromBody] CreateBabyProfileDto request)
        {
            var created = await _service.Create(request);

            return Ok(created);
        }

        [HttpPatch("{id:long}")]
        public async Task<ActionResult<BabyProfileSummaryDto>> Patch(
            long id,
            [FromBody] BabyProfilePatchDto patch)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(id);

            var updated = await _service.Patch(id, patch);

            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(id);

            await _service.Delete(id);

            return NoContent();
        }
    }
}