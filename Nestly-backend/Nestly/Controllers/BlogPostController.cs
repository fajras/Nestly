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
    [Authorize(Roles = "Doctor")]
    public ActionResult<BlogPostResponseDto> Create([FromBody] CreateBlogPostDto request)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (!long.TryParse(userIdClaim, out var currentUserId))
        {
            return Unauthorized();
        }

        var created = _service.Create(request, currentUserId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:long}")]
    [Authorize(Roles = "Doctor")]
    public ActionResult<BlogPostResponseDto> Patch(long id, [FromBody] BlogPostPatchDto patch)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (!long.TryParse(userIdClaim, out var currentUserId))
        {
            return Unauthorized();
        }

        var updated = _service.Patch(id, patch, currentUserId);

        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Delete(long id)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (!long.TryParse(userIdClaim, out var currentUserId))
        {
            return Unauthorized();
        }

        _service.Delete(id, currentUserId);

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
