using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BabyGrowthController : ControllerBase
    {
        private readonly IBabyGrowthService _service;
        private readonly ICurrentUserService _currentUserService;

        public BabyGrowthController(
            IBabyGrowthService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<BabyGrowthResponseDto>> Get(
            [FromQuery] BabyGrowthSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<BabyGrowthResponseDto>>> GetMy(
            [FromQuery] BabyGrowthSearchObject search)
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
        public async Task<ActionResult<BabyGrowthResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureBabyGrowthOwnershipAsync(id);

            var entity = _service.GetById(id);

            return Ok(entity);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<BabyGrowthResponseDto>> Create(
            [FromBody] CreateBabyGrowthDto request)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(
                    request.BabyId);

            var created = _service.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<BabyGrowthResponseDto>> Patch(
            long id,
            [FromBody] BabyGrowthPatchDto patch)
        {
            await _currentUserService
                .EnsureBabyGrowthOwnershipAsync(id);

            try
            {
                var updated = _service.Patch(id, patch);

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
                .EnsureBabyGrowthOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }
    }
}
