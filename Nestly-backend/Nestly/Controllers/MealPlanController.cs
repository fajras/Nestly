using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlanController : ControllerBase
    {
        private readonly IMealPlanService _service;

        public MealPlanController(IMealPlanService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MealPlanResponseDto>> Get([FromQuery] MealPlanSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<MealPlanResponseDto> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<MealPlanResponseDto> Create([FromBody] CreateMealPlanDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<MealPlanResponseDto> Patch(long id, [FromBody] MealPlanPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();

        [HttpGet("Recommendation")]
        public ActionResult<IEnumerable<MealRecommendationDto>> GetRecommendation(
            [FromQuery] MealRecommendationSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("Recommendation/{id:long}")]
        public ActionResult<MealRecommendationDto> GetRecommendationById(long id)
        {
            var item = _service.GetRecommendationById(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}
