using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlanController : ControllerBase
    {
        private readonly IMealPlanService _service;
        public MealPlanController(IMealPlanService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<MealPlan>> Get([FromQuery] MealPlanSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<MealPlan> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<MealPlan> Create([FromBody] CreateMealPlanDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<MealPlan> Patch(long id, [FromBody] MealPlanPatchDto patch)
        {
            var updated = _service.Patch(id, patch);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();

        [HttpGet("Recommendation")]
        public ActionResult<IEnumerable<MealRecommendationDto>> Get(
           [FromQuery] MealRecommendationSearchObject? search)
        {
            var result = _service.Get(search);
            return Ok(result);
        }

        [HttpGet("Recommendation/{id:long}")]
        public ActionResult<MealRecommendationDto> GetRecommendationById(long id)
        {
            var item = _service.GetRecommendationById(id);
            return item is null ? NotFound() : Ok(item);
        }
    }
}
