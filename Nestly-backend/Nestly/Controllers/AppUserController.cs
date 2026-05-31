using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly ICurrentUserService _currentUserService;

        public AppUserController(
            IAppUserService service,
            ICurrentUserService currentUserService)
        {
            _appUserService = service;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<PagedResult<AppUserResultDto>>> Get(
            [FromQuery] AppUserSearchObject search)
        {
            var result = await _appUserService.Get(search);

            return Ok(result);
        }

        [HttpGet("{id:long}", Name = "GetAppUserById")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<AppUserResultDto>> GetById(long id)
        {
            var dto = await _appUserService.GetById(id);

            return dto is not null
                ? Ok(dto)
                : NotFound();
        }

        [HttpGet("me")]
        public async Task<ActionResult<AppUserResultDto>> GetMe()
        {
            var appUserId =
                _currentUserService.GetCurrentAppUserId();

            var dto =
                await _appUserService.GetById(appUserId);

            return dto is not null
                ? Ok(dto)
                : NotFound();
        }

        [HttpPatch("me")]
        public async Task<ActionResult<AppUserResultDto>> PatchMe(
            [FromBody] AppUserPatchDto patch)
        {
            try
            {
                var appUserId =
                    _currentUserService.GetCurrentAppUserId();

                var updated =
                    await _appUserService.Patch(
                        appUserId,
                        patch);

                return updated is not null
                    ? Ok(updated)
                    : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppUserDto request)
        {
            var dto = await _appUserService.Create(request);

            return CreatedAtRoute(
                "GetAppUserById",
                new { id = dto.Id },
                dto);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(long id)
        {
            await _appUserService.Delete(id);

            return NoContent();
        }

        [HttpPost("doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateDoctor(
            [FromBody] CreateAppUserDto request)
        {
            var dto = await _appUserService.CreateDoctor(request);

            return CreatedAtRoute(
                "GetAppUserById",
                new { id = dto.Id },
                dto);
        }
    }
}