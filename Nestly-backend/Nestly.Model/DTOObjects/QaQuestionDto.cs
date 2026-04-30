using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class QaQuestionSearchObject
    {
        public long? AskedById { get; set; }
        public string? Query { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool? OnlyUnanswered { get; set; }
        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }

    public class CreateQaQuestionDto
    {
        [Required]
        public string QuestionText { get; set; } = default!;
        [Range(1, long.MaxValue)]
        public long? AskedById { get; set; }
    }

    public class QaQuestionPatchDto
    {
        public string? QuestionText { get; set; }
        public long? AskedById { get; set; }
    }

    public class CreateQaAnswerDto
    {
        [Required]
        public string AnswerText { get; set; } = default!;
        [Range(1, long.MaxValue)]
        public long? AnsweredById { get; set; }
    }

    public class QaAnswerDto
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string AnswerText { get; set; } = default!;
        public long? AnsweredById { get; set; }
        public string? AnsweredByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class QaQuestionDto
    {
        public long Id { get; set; }
        public string QuestionText { get; set; } = default!;
        public long? AskedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class QaQuestionWithLatestAnswerDto
    {
        public long Id { get; set; }
        public string QuestionText { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public bool IsAnswered { get; set; }
        public string? LatestAnswerText { get; set; }
        public DateTime? LatestAnswerCreatedAt { get; set; }
        public string? AnsweredByName { get; set; }
    }
}
