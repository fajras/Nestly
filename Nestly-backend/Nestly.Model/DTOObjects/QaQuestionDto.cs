using System;

namespace Nestly.Model.DTOObjects
{
    public class QaQuestionSearchObject
    {
        public long? AskedById { get; set; }
        public string? Query { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool? OnlyUnanswered { get; set; }
    }

    public class CreateQaQuestionDto
    {
        public string QuestionText { get; set; } = default!;
        public long? AskedById { get; set; }
    }

    public class QaQuestionPatchDto
    {
        public string? QuestionText { get; set; }
        public long? AskedById { get; set; }
    }

    public class CreateQaAnswerDto
    {
        public string AnswerText { get; set; } = default!;
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
