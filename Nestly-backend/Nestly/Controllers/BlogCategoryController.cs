using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _service;

        public BlogCategoryController(IBlogCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BlogCategoryDto>>> Get([FromQuery] BlogCategorySearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogCategoryDto>> GetById(int id)
        {
            var result = await _service.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<BlogCategoryDto>> Create([FromBody] BlogCategoryInsertDto request)
        {
            var result = await _service.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<BlogCategoryDto>> Update(int id, [FromBody] BlogCategoryUpdateDto request)
        {
            var result = await _service.Update(id, request);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
