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
        private readonly ICurrentUserService _currentUserService;

        public MealPlanController(
            IMealPlanService service,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public ActionResult<PagedResult<MealPlanResponseDto>> Get(
            [FromQuery] MealPlanSearchObject search)
        {
            return Ok(
      _service.GetMealPlans(search));
        }

        [Authorize(Roles = "Parent")]
        [HttpGet("my")]
        public async Task<ActionResult<PagedResult<MealPlanResponseDto>>> GetMy(
     [FromQuery] MealPlanSearchObject search)
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
                _service.GetMealPlansByParent(
                    parent.Id,
                    search));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MealPlanResponseDto>> GetById(
            long id)
        {
            await _currentUserService
                .EnsureMealPlanOwnershipAsync(id);

            var entity = _service.GetById(id);

            return Ok(entity);
        }

        [Authorize(Roles = "Parent")]
        [HttpPost]
        public async Task<ActionResult<MealPlanResponseDto>> Create(
            [FromBody] CreateMealPlanDto request)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(
                    request.BabyId);

            var created =
                _service.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created);
        }

        [Authorize(Roles = "Parent")]
        [HttpPatch("{id:long}")]
        public async Task<ActionResult<MealPlanResponseDto>> Patch(
            long id,
            [FromBody] MealPlanPatchDto patch)
        {
            await _currentUserService
                .EnsureMealPlanOwnershipAsync(id);

            var updated =
                _service.Patch(id, patch);

            return Ok(updated);
        }

        [Authorize(Roles = "Parent")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id)
        {
            await _currentUserService
                .EnsureMealPlanOwnershipAsync(id);

            _service.Delete(id);

            return NoContent();
        }

        [HttpGet("Recommendation")]
        public ActionResult<PagedResult<MealRecommendationDto>> GetRecommendation(
            [FromQuery] MealRecommendationSearchObject search)
        {
            return Ok(
                _service.GetMealRecommendations(search));
        }

        [HttpGet("Recommendation/{id:long}")]
        public ActionResult<MealRecommendationDto> GetRecommendationById(
            long id)
        {
            var item =
                _service.GetRecommendationById(id);

            return Ok(item);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("Recommendation/AvailableFoodTypes")]
        public ActionResult<IEnumerable<FoodTypeDto>> GetFoodTypesWithoutRecommendation()
        {
            return Ok(
                _service.GetFoodTypesWithoutRecommendation());
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("Recommendation")]
        public ActionResult<MealRecommendationDto> CreateRecommendation(
            [FromBody] CreateMealRecommendationDto request)
        {
            var result =
                _service.CreateRecommendation(request);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPatch("Recommendation/{id:long}")]
        public ActionResult<MealRecommendationDto> UpdateRecommendation(
            long id,
            [FromBody] CreateMealRecommendationDto request)
        {
            var result =
                _service.UpdateRecommendation(
                    id,
                    request);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpDelete("Recommendation/{id:long}")]
        public IActionResult DeleteRecommendation(
            long id)
        {
            _service.DeleteRecommendation(id);

            return NoContent();
        }
    }
}