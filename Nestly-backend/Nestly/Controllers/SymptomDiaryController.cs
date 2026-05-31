using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SymptomDiaryController : ControllerBase
    {
        private readonly ISymptomDiaryService _service;
        private readonly ICurrentUserService _currentUserService;

        public SymptomDiaryController(
     ISymptomDiaryService service,
     ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<SymptomDiaryResponseDto>> Get(
     [FromQuery] SymptomDiarySearchObject search)
        {
            return Ok(_service.Get(search));
        }
        [HttpPost]
        [Authorize(Roles = "Parent")]
        public ActionResult<SymptomDiaryResponseDto> Create(
     [FromBody] CreateSymptomDiaryDto request)
        {
            try
            {
                var dto = _service.Create(request);

                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("my")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult<PagedResult<SymptomDiaryResponseDto>>> GetMy(
    [FromQuery] SymptomDiarySearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            return Ok(
                _service.GetByParent(parent.Id, search));
        }

        [HttpGet("by-date")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult<SymptomDiaryResponseDto>> GetByDate(
      [FromQuery] DateTime date)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            var dto = _service.GetByDate(
                parent.Id,
                date);

            return dto is null
                ? NotFound()
                : Ok(dto);
        }

        [HttpPatch("{id:long}")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult<SymptomDiaryResponseDto>> Patch(
         long id,
         [FromBody] SymptomDiaryPatchDto patch)
        {
            await _currentUserService
                .EnsureSymptomDiaryOwnershipAsync(id);

            try
            {
                var dto = _service.Patch(id, patch);

                return dto is null
                    ? NotFound()
                    : Ok(dto);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> Delete(long id)
        {
            await _currentUserService
                .EnsureSymptomDiaryOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }

        [HttpGet("marked-days")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult<PagedResult<DateTime>>> GetMarkedDays(
     [FromQuery] SymptomDiarySearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            return Ok(
                _service.GetMarkedDays(
                    parent.Id,
                    search));
        }
    }
}
