using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RoleDto>>> Get([FromQuery] RoleSearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(long id)
        {
            var result = await _service.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleInsertDto request)
        {
            var result = await _service.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<RoleDto>> Update(long id, [FromBody] RoleUpdateDto request)
        {
            var result = await _service.Update(id, request);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
