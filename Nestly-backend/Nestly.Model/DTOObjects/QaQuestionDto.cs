using System;

namespace Nestly.Model.DTOObjects
{
    public class QaQuestionPatchDto
    {
        public string? QuestionText { get; set; }
        public long? AskedByUserId { get; set; }
    }

    public class QaAnswerPatchDto
    {
        public string? AnswerText { get; set; }
        public long? AnsweredByUserId { get; set; }
    }
    public class CreateQaQuestionDto
    {
        public string QuestionText { get; set; } = default!;
        public long? AskedById { get; set; }
    }
    public class CreateQaAnswerDto
    {
        public long QuestionId { get; set; }
        public string AnswerText { get; set; } = default!;
        public long? AnsweredById { get; set; }
    }
    public class QaQuestionSearchObject
    {
        public long? AskedByUserId { get; set; }
        public string? Query { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class QaAnswerSearchObject
    {
        public long? QuestionId { get; set; }
        public long? AnsweredByUserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
