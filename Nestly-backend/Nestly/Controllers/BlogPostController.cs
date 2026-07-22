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
    private readonly ICurrentUserService _currentUserService;

    public BlogPostController(
        IBlogPostService service,
        AzureBlobService blob,
        ICurrentUserService currentUserService)
    {
        _service = service;
        _blob = blob;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<BlogPostResponseDto>>> Get([FromQuery] BlogPostSearchObject search)
    {
        return Ok(await _service.Get(search));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BlogPostResponseDto>> GetById(long id)
    {
        var result = await _service.GetById(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<BlogPostResponseDto>> Create([FromBody] CreateBlogPostDto request)
    {

        var currentUserId = _currentUserService.GetCurrentAppUserId();

        var created = await _service.Create(request, currentUserId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

    }

    [HttpPatch("{id:long}")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<BlogPostResponseDto>> Patch(long id, [FromBody] BlogPostPatchDto patch)
    {
        var currentUserId =
    _currentUserService.GetCurrentAppUserId();

        var updated = await _service.Patch(id, patch, currentUserId);

        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Delete(long id)
    {
        var currentUserId =
    _currentUserService.GetCurrentAppUserId();

        await _service.Delete(id, currentUserId);

        await _blob.DeleteBlogImageAsync(id);

        return NoContent();
    }
    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<PagedResult<BlogPostResponseDto>>> GetByCategoryId(
        int categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await _service.GetByCategoryId(categoryId, page, pageSize));
    }
}
