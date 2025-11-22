using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _service;
        public BlogPostController(IBlogPostService service) => _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<BlogPost>> Get([FromQuery] BlogPostSearchObject? search)
            => Ok(_service.Get(search));

        [HttpGet("{id:long}")]
        public ActionResult<BlogPostResponseDto> GetById(long id)
        {
            var entity = _service.GetById(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost]
        public ActionResult<BlogPost> Create([FromBody] CreateBlogPostDto request)
        {
            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<BlogPost> Patch(long id, [FromBody] BlogPostPatchDto patch)
        {
            try
            {
                var updated = _service.Patch(id, patch);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
            => _service.Delete(id) ? NoContent() : NotFound();

        [HttpGet("category")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("category/{categoryId:int}")]
        public ActionResult<IEnumerable<BlogPost>> GetByCategoryId(int categoryId)
        {
            var posts = _service.GetByCategoryId(categoryId);

            if (posts == null || !posts.Any())
            {
                return NotFound(new { message = $"No blog posts found for category id {categoryId}." });
            }

            return Ok(posts);
        }
    }
}
