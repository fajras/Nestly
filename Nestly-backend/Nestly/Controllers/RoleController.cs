using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
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
        public ActionResult<PagedResult<RoleDto>> Get([FromQuery] RoleSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id}")]
        public ActionResult<RoleDto> GetById(long id)
        {
            var result = _service.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<RoleDto> Create([FromBody] RoleInsertDto request)
        {
            var result = _service.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        public ActionResult<RoleDto> Update(long id, [FromBody] RoleUpdateDto request)
        {
            var result = _service.Update(id, request);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var success = _service.Delete(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
