using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MealPlanController : ControllerBase
    {
        private readonly IMealPlanService _service;

        public MealPlanController(IMealPlanService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PagedResult<MealPlanResponseDto>> Get([FromQuery] MealPlanSearchObject search)
       => Ok(_service.GetMealPlans(search));
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
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("Recommendation")]
        public ActionResult<PagedResult<MealRecommendationDto>> GetRecommendation(
          [FromQuery] MealRecommendationSearchObject search)
          => Ok(_service.GetMealRecommendations(search));

        [HttpGet("Recommendation/{id:long}")]
        public ActionResult<MealRecommendationDto> GetRecommendationById(long id)
        {
            var item = _service.GetRecommendationById(id);
            return item is null ? NotFound() : Ok(item);
        }
        [HttpGet("Recommendation/AvailableFoodTypes")]
        public ActionResult<IEnumerable<FoodTypeDto>> GetFoodTypesWithoutRecommendation()
        {
            return Ok(_service.GetFoodTypesWithoutRecommendation());
        }
        [HttpPost("Recommendation")]
        public ActionResult<MealRecommendationDto> CreateRecommendation(
            [FromBody] CreateMealRecommendationDto request)
        {
            var result = _service.CreateRecommendation(request);
            return Ok(result);
        }
        [HttpPatch("Recommendation/{id:long}")]
        public ActionResult<MealRecommendationDto> UpdateRecommendation(
    long id,
    [FromBody] CreateMealRecommendationDto request)
        {
            var result = _service.UpdateRecommendation(id, request);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("Recommendation/{id:long}")]
        public IActionResult DeleteRecommendation(long id)
        {
            _service.Delete(id);
            return NoContent();
        }

    }
}
