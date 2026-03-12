using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Services.Data;
using Nestly.Services.Repository;

[ApiController]
[Route("api/blogmedia")]
[Authorize]
public class BlogMediaController : ControllerBase
{
    private readonly AzureBlobService _blob;
    private readonly NestlyDbContext _db;

    public BlogMediaController(
        AzureBlobService blob,
        NestlyDbContext db)
    {
        _blob = blob;
        _db = db;
    }

    [HttpPost("upload/{blogId:long}")]
    public async Task<IActionResult> Upload(long blogId, IFormFile file)
    {
        var post = await _db.BlogPosts.FindAsync(blogId);
        if (post == null)
        {
            return NotFound("Blog not found");
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("File missing");
        }

        var ext = Path.GetExtension(file.FileName).ToLower();
        if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
        {
            return BadRequest("Invalid file type");
        }

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
        if (post != null)
        {
            post.ImageUrl = null;
            post.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        await _blob.DeleteBlogImageAsync(blogId);
        return NoContent();
    }

}
