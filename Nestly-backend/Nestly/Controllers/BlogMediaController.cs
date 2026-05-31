using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using Nestly.Services.Repository;

[ApiController]
[Route("api/blogmedia")]
[Authorize(Roles = "Doctor")]
public class BlogMediaController : ControllerBase
{
    private readonly AzureBlobService _blob;
    private readonly NestlyDbContext _db;
    private readonly ICurrentUserService _currentUserService;

    public BlogMediaController(
        AzureBlobService blob,
        NestlyDbContext db, ICurrentUserService currentUserService)
    {
        _blob = blob;
        _db = db;
        _currentUserService = currentUserService;
    }

    [HttpPost("upload/{blogId:long}")]
    public async Task<IActionResult> Upload(long blogId, IFormFile file)
    {
        var post = await _db.BlogPosts.FindAsync(blogId);

        if (post == null)
        {
            return NotFound("Blog not found.");
        }

        if (!CanModifyPost(post.AuthorId))
        {
            return Forbid();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("File missing.");
        }

        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest("Maximum allowed file size is 5 MB.");
        }

        if (!await HasValidImageSignature(file))
        {
            return BadRequest("Only valid JPG and PNG images are allowed.");
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        await using var stream = file.OpenReadStream();

        var url = await _blob.UploadBlogImageAsync(blogId, stream, ext);

        post.ImageUrl = url;
        post.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new { url });
    }

    [HttpDelete("delete/{blogId:long}")]
    public async Task<IActionResult> Delete(long blogId)
    {
        var post = await _db.BlogPosts.FindAsync(blogId);

        if (post == null)
        {
            return NotFound("Blog not found.");
        }

        if (blogId <= 12)
        {
            return BadRequest("System blog posts cannot be modified.");
        }

        if (!CanModifyPost(post.AuthorId))
        {
            return Forbid();
        }

        post.ImageUrl = null;
        post.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        await _blob.DeleteBlogImageAsync(blogId);

        return NoContent();
    }

    private bool CanModifyPost(long? authorId)
    {
        if (User.IsInRole("Doctor"))
        {
            return true;
        }

        var currentUserId =
            _currentUserService
                .GetCurrentAppUserId();

        return authorId == currentUserId;
    }

    private static async Task<bool> HasValidImageSignature(IFormFile file)
    {
        byte[] buffer = new byte[8];

        await using var stream = file.OpenReadStream();

        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead < 4)
        {
            return false;
        }

        var isJpeg =
            buffer[0] == 0xFF &&
            buffer[1] == 0xD8 &&
            buffer[2] == 0xFF;

        var isPng =
            bytesRead >= 8 &&
            buffer[0] == 0x89 &&
            buffer[1] == 0x50 &&
            buffer[2] == 0x4E &&
            buffer[3] == 0x47 &&
            buffer[4] == 0x0D &&
            buffer[5] == 0x0A &&
            buffer[6] == 0x1A &&
            buffer[7] == 0x0A;

        return isJpeg || isPng;
    }
}