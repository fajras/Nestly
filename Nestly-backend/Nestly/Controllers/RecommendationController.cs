using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    [Authorize]
    public class RecommendationController : ControllerBase
    {
        private readonly IBlogRecommendationService _service;
        private readonly ICurrentUserService _currentUserService;

        public RecommendationController(IBlogRecommendationService service, ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        [HttpGet("blog")]
        public async Task<IActionResult> Get(int take = 10)
        {
            var userId = _currentUserService.GetCurrentAppUserId();

            return Ok(await _service.GetRecommendations(userId, take));
        }

        [HttpPost("log")]
        public async Task<IActionResult> Log(LogBlogInteractionRequest req)
        {
            var userId = _currentUserService.GetCurrentAppUserId();

            await _service.LogInteraction(userId, req);
            return Ok();
        }
    }
}
