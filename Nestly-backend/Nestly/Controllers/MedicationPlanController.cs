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
        public MedicationPlanController(IMedicationPlanService service) => _service = service;

        [HttpGet]
        public ActionResult<PagedResult<MedicationPlanResponseDto>> Get(
      [FromQuery] MedicationPlanSearchObject search)
      => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<MedicationPlanResponseDto> GetById(long id)
        {
            var dto = _service.GetById(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public ActionResult<MedicationPlanResponseDto> Create(
            [FromBody] CreateMedicationPlanDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<MedicationPlanResponseDto> Patch(
            long id,
            [FromBody] MedicationPlanPatchDto patch)
        {
            try
            {
                var updated = _service.Patch(id, patch);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpPost("mark-taken")]
        public IActionResult MarkTaken([FromBody] MarkMedicationTakenDto dto)
        {
            _service.MarkAsTaken(dto.IntakeLogId);
            return NoContent();
        }
        [HttpGet("day")]
        public ActionResult<PagedResult<MedicationIntakeLogDto>> GetForDay(
         [FromQuery] MedicationIntakeLogSearchObject search)
        {
            return Ok(_service.GetLogsForDay(search));
        }

    }
}
