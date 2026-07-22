using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FetalDevelopmentWeekController : ControllerBase
    {
        private readonly IFetalDevelopmentWeekService _service;

        public FetalDevelopmentWeekController(IFetalDevelopmentWeekService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<FetalDevelopmentWeekResponseDto>>> Get(
    [FromQuery] FetalDevelopmentWeekSearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FetalDevelopmentWeekResponseDto>> GetById(int id)
        {
            var entity = await _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpGet("week/{weekNumber:int}")]
        public async Task<ActionResult<FetalDevelopmentWeekResponseDto>> GetByWeekNumber(int weekNumber)
        {
            var entity = await _service.GetByWeekNumber(weekNumber);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<FetalDevelopmentWeekResponseDto>> Create([FromBody] CreateFetalDevelopmentWeekDto request)
        {
            var created = await _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<FetalDevelopmentWeekResponseDto>> Patch(int id, [FromBody] FetalDevelopmentWeekPatchDto patch)
        {
            var updated = await _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
