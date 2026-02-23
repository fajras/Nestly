using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
using System.Security.Claims;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    [Authorize]
    public class RecommendationController : ControllerBase
    {
        private readonly IBlogRecommendationService _service;

        public RecommendationController(IBlogRecommendationService service)
        {
            _service = service;
        }

        [HttpGet("blog")]
        public async Task<IActionResult> Get(int take = 10)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User id claim not found.");
            }

            var userId = long.Parse(claim.Value);

            return Ok(await _service.GetRecommendations(userId, take));
        }

        [HttpPost("log")]
        public async Task<IActionResult> Log(LogBlogInteractionRequest req)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized("User id claim not found.");
            }

            var userId = long.Parse(claim.Value);

            await _service.LogInteraction(userId, req);
            return Ok();
        }
    }
}
