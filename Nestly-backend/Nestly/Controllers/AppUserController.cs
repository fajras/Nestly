using Microsoft.AspNetCore.Mvc;
using Nestly.Model.Entity;
using Nestly.Model.SearchObjects;
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
        public ActionResult<IEnumerable<AppUser>> Get([FromQuery] AppUserSearchObject? search)
        {
            var result = appUserService.Get(search);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public ActionResult<AppUser> GetById([FromRoute] long id)
        {
            var entity = appUserService.GetById(id);
            if (entity is null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        // POST api/AppUser
        [HttpPost]
        public ActionResult<AppUser> Create([FromBody] AppUser request)
        {
            var created = appUserService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/AppUser/5
        [HttpPut("{id:long}")]
        public ActionResult<AppUser> Update([FromRoute] long id, [FromBody] AppUser request)
        {
            var updated = appUserService.Update(id, request);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        // DELETE api/AppUser/5
        [HttpDelete("{id:long}")]
        public IActionResult Delete([FromRoute] long id)
        {
            var ok = appUserService.Delete(id);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

