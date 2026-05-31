using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicationPlanController : ControllerBase
    {
        private readonly IMedicationPlanService _service;
        private readonly ICurrentUserService _currentUserService;

        public MedicationPlanController(
            IMedicationPlanService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<MedicationPlanResponseDto>> Get(
            [FromQuery] MedicationPlanSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MedicationPlanResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureMedicationPlanOwnershipAsync(id);

            var dto = _service.GetById(id);

            return Ok(dto);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<MedicationPlanResponseDto>> Create(
            [FromBody] CreateMedicationPlanDto request)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            var created = _service.Create(
                parent.Id,
                request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<MedicationPlanResponseDto>> Patch(
            long id,
            [FromBody] MedicationPlanPatchDto patch)
        {
            await _currentUserService
                .EnsureMedicationPlanOwnershipAsync(id);

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
                .EnsureMedicationPlanOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }

        [Authorize(Roles = "Parent")]
        [HttpPost("mark-taken")]
        public async Task<IActionResult> MarkTaken(
            [FromBody] MarkMedicationTakenDto dto)
        {
            await _currentUserService
                .EnsureMedicationIntakeOwnershipAsync(
                    dto.IntakeLogId);

            _service.MarkAsTaken(dto.IntakeLogId);

            return NoContent();
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("day")]
        public ActionResult<PagedResult<MedicationIntakeLogDto>> GetForDay(
            [FromQuery] MedicationIntakeLogSearchObject search)
        {
            return Ok(
                _service.GetLogsForDay(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my-day")]
        public async Task<ActionResult<PagedResult<MedicationIntakeLogDto>>> GetMyDay(
            [FromQuery] MedicationIntakeLogSearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            return Ok(
                _service.GetLogsForDayByParent(
                    parent.Id,
                    search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<MedicationPlanResponseDto>>> GetMy(
            [FromQuery] MedicationPlanSearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            return Ok(
                _service.GetByParent(
                    parent.Id,
                    search));
        }
    }
}