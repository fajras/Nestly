namespace Nestly.Model.PatchObjects
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
}
