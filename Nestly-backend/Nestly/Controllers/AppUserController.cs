﻿using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
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
            var created = appUserService.Create(request);

            var dto = new AppUserResultDto
            {
                Email = created.Email,
                Username = created.Username,
                FirstName = created.FirstName,
                LastName = created.LastName,
                RoleId = created.RoleId
            };

            return CreatedAtRoute("GetAppUserById", new { id = created.Id }, dto);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<AppUser> Patch(long id, [FromBody] AppUserPatchDto patch)
        {
            try
            {
                var updated = appUserService.Patch(id, patch);
                return updated is null ? NotFound() : Ok(updated);
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

