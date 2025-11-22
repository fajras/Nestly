using Microsoft.AspNetCore.Mvc;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppUserController : ControllerBase
    {
        protected readonly IAppUserService appUserService;
        public AppUserController(IAppUserService service)
        {
            appUserService = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AppUserResultDto>> Get([FromQuery] AppUserSearchObject? search)
     => Ok(appUserService.Get(search));

        [HttpGet("{id:long}", Name = "GetAppUserById")]
        public ActionResult<AppUserResultDto> GetById(long id)
            => appUserService.GetById(id) is { } dto ? Ok(dto) : NotFound();

        [HttpPost]
        public ActionResult<AppUserResultDto> Create([FromBody] CreateAppUserDto request)
        {
            var dto = appUserService.Create(request);
            return CreatedAtRoute("GetAppUserById", new { id = dto.Id }, dto);
        }


        [HttpPatch("{id:long}")]
        public ActionResult<AppUserResultDto> Patch(long id, [FromBody] AppUserPatchDto patch)
        {
            try
            {
                var updated = appUserService.Patch(id, patch);
                if (updated is null)
                {
                    return NotFound();
                }

                var dto = new AppUserResultDto
                {
                    Id = updated.Id,
                    Email = updated.Email,
                    FirstName = updated.FirstName,
                    LastName = updated.LastName,
                    RoleId = updated.RoleId,
                    IdentityUserId = updated.IdentityUserId
                };
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => appUserService.Delete(id) ? NoContent() : NotFound();
    }
}

