using System.Collections.Generic;

namespace Nestly.Model.DTOObjects
{

    public class RecommendationScoreBreakdown
    {
        public string FactorName { get; set; } = string.Empty;
        public double Value { get; set; }
        public double Weight { get; set; }
        public string Explanation { get; set; }
    }

    public class BlogPostRecommendationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }

        public int Phase { get; set; }
        public int? WeekFrom { get; set; }
        public int? WeekTo { get; set; }

        public double FinalScore { get; set; }

        public List<RecommendationScoreBreakdown> ScoreBreakdown { get; set; }

        public string MainReason { get; set; }
    }
    public class LogBlogInteractionRequest
    {
        public long PostId { get; set; }
        public int EventType { get; set; }
        public int? SpentSeconds { get; set; }
    }

}
