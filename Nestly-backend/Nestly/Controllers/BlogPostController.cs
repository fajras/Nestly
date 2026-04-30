using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
using Nestly.Services.Repository;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public ActionResult<PagedResult<BlogPostResponseDto>> Get([FromQuery] BlogPostSearchObject search)
    {
        return Ok(_service.Get(search));
    }

    [HttpGet("{id:long}")]
    public ActionResult<BlogPostResponseDto> GetById(long id)
    {
        var result = _service.GetById(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public ActionResult<BlogPostResponseDto> Create([FromBody] CreateBlogPostDto request)
    {
        var created = _service.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:long}")]
    public ActionResult<BlogPostResponseDto> Patch(long id, [FromBody] BlogPostPatchDto patch)
    {
        var updated = _service.Patch(id, patch);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        _service.Delete(id);

        await _blob.DeleteBlogImageAsync(id);
        return NoContent();
    }
    [HttpGet("category/{categoryId:int}")]
    public ActionResult<PagedResult<BlogPostResponseDto>> GetByCategoryId(
        int categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(_service.GetByCategoryId(categoryId, page, pageSize));
    }
}
