using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationPlanController : ControllerBase
    {
        private readonly IMedicationPlanService _service;
        public MedicationPlanController(IMedicationPlanService service) => _service = service;

        // GET /api/medicationplan?UserId=123
        [HttpGet]
        public ActionResult<IEnumerable<MedicationPlanResponseDto>> Get(
            [FromQuery] MedicationPlanSearchObject? search)
            => Ok(_service.Get(search));

        // GET /api/medicationplan/{id}
        [HttpGet("{id:long}")]
        public ActionResult<MedicationPlanResponseDto> GetById(long id)
        {
            var dto = _service.GetById(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        // POST /api/medicationplan
        [HttpPost]
        public ActionResult<MedicationPlanResponseDto> Create(
            [FromBody] CreateMedicationPlanDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PATCH /api/medicationplan/{id}
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

        // DELETE /api/medicationplan/{id}
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();
    }
}
