using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationScheduleTimeController : ControllerBase
    {
        private readonly IMedicationScheduleTimeService _medicationScheduleTimeService;

        public MedicationScheduleTimeController(IMedicationScheduleTimeService medicationScheduleTimeService)
        {
            _medicationScheduleTimeService = medicationScheduleTimeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicationScheduleTime>>> GetAll()
        {
            try
            {
                var medicationScheduleTimes = await _medicationScheduleTimeService.GetAllAsync();
                return Ok(medicationScheduleTimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicationScheduleTime>> GetById(int id)
        {
            try
            {
                var medicationScheduleTime = await _medicationScheduleTimeService.GetByIdAsync(id);
                if (medicationScheduleTime == null)
                {
                    return NotFound($"Medication schedule time with ID {id} not found");
                }
                return Ok(medicationScheduleTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("plan/{medicationPlanId}")]
        public async Task<ActionResult<IEnumerable<MedicationScheduleTime>>> GetByMedicationPlanId(int medicationPlanId)
        {
            try
            {
                var medicationScheduleTimes = await _medicationScheduleTimeService.GetByMedicationPlanIdAsync(medicationPlanId);
                return Ok(medicationScheduleTimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicationScheduleTime>> Create([FromBody] MedicationScheduleTime medicationScheduleTime)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdMedicationScheduleTime = await _medicationScheduleTimeService.CreateAsync(medicationScheduleTime);
                return CreatedAtAction(nameof(GetById), new { id = createdMedicationScheduleTime.Id }, createdMedicationScheduleTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MedicationScheduleTime>> Update(int id, [FromBody] MedicationScheduleTime medicationScheduleTime)
        {
            try
            {
                if (id != medicationScheduleTime.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedMedicationScheduleTime = await _medicationScheduleTimeService.UpdateAsync(medicationScheduleTime);
                return Ok(updatedMedicationScheduleTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _medicationScheduleTimeService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            try
            {
                var exists = await _medicationScheduleTimeService.ExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
