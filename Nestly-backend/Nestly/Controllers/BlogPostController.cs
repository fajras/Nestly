using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;
using Nestly.Services.Repository;

[ApiController]
[Route("api/[controller]")]
public class BlogPostController : ControllerBase
{
    private readonly IBlogPostService _service;
    private readonly AzureBlobService _blob;

    public BlogPostController(
        IBlogPostService service,
        AzureBlobService blob)
    {
        _service = service;
        _blob = blob;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BlogPost>> Get([FromQuery] BlogPostSearchObject? search)
        => Ok(_service.Get(search));

    [HttpGet("{id:long}")]
    public ActionResult<BlogPostResponseDto> GetById(long id)
    {
        var post = _service.GetById(id);
        if (post == null)
        {
            return NotFound();
        }

        return Ok(new BlogPostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            ImageUrl = post.ImageUrl,
            AuthorId = post.AuthorId
        });
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
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = _service.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }

        await _blob.DeleteBlogImageAsync(id);
        return NoContent();
    }

    [HttpGet("category")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("category/{categoryId:int}")]
    public ActionResult<IEnumerable<BlogPost>> GetByCategoryId(int categoryId)
    {
        var posts = _service.GetByCategoryId(categoryId);
        if (!posts.Any())
        {
            return NotFound();
        }
        return Ok(posts);
    }
}
