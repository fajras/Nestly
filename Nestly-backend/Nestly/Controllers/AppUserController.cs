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
        protected readonly IAppUserService appUserService;

        public AppUserController(IAppUserService service)
        {
            appUserService = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AppUserResultDto>>> Get(
            [FromQuery] AppUserSearchObject search)
        {
            var result = await appUserService.Get(search);

            return Ok(result);
        }

        [HttpGet("{id:long}", Name = "GetAppUserById")]
        public async Task<ActionResult<AppUserResultDto>> GetById(long id)
        {
            var dto = await appUserService.GetById(id);

            return dto is not null ? Ok(dto) : NotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppUserDto request)
        {
            var dto = await appUserService.Create(request);

            return CreatedAtRoute(
                "GetAppUserById",
                new { id = dto.Id },
                dto);
        }

        [HttpPatch("{id:long}")]
        public async Task<ActionResult<AppUserResultDto>> Patch(
            long id,
            [FromBody] AppUserPatchDto patch)
        {
            try
            {
                var updated = await appUserService.Patch(id, patch);

                return updated is null
                    ? NotFound()
                    : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await appUserService.Delete(id);

            return NoContent();
        }
    }
}