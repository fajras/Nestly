namespace Nestly.Model.DTOObjects
{
    public class RecommendedBlogPostResponse
    {
        public long PostId { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public double Score { get; set; }
    }
    public class LogBlogInteractionRequest
    {
        public long PostId { get; set; }
        public int EventType { get; set; }
        public int? SpentSeconds { get; set; }
    }

}
