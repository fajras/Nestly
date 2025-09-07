using System;

namespace Nestly.Model.SearchObjects
{

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
