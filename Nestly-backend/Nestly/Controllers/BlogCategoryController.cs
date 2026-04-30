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
        public ActionResult<PagedResult<BlogCategoryDto>> Get([FromQuery] BlogCategorySearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id}")]
        public ActionResult<BlogCategoryDto> GetById(int id)
        {
            var result = _service.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<BlogCategoryDto> Create([FromBody] BlogCategoryInsertDto request)
        {
            var result = _service.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        public ActionResult<BlogCategoryDto> Update(int id, [FromBody] BlogCategoryUpdateDto request)
        {
            var result = _service.Update(id, request);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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
