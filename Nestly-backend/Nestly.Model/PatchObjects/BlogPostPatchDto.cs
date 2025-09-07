namespace Nestly.Model.PatchObjects
{
    public class BlogPostPatchDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public long? AuthorId { get; set; }
    }
}
