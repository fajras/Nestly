using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IQaQuestionService
    {
        Task<List<QaQuestionWithLatestAnswerDto>> GetAllWithLatestAnswer(QaQuestionSearchObject? search, CancellationToken ct = default);
        Task<QaQuestionDto?> GetById(long id, CancellationToken ct = default);
        Task<List<QaQuestionDto>> GetByUserAsync(long askedByParentProfileId, CancellationToken ct = default);

        Task<QaQuestionDto> Create(CreateQaQuestionDto dto, CancellationToken ct = default);
        Task<QaQuestionDto?> Patch(long id, QaQuestionPatchDto patch, CancellationToken ct = default);
        Task<bool> Delete(long id, CancellationToken ct = default);

        Task<List<QaAnswerDto>> GetAnswers(long questionId, CancellationToken ct = default);
        Task<QaAnswerDto> CreateAnswer(long questionId, CreateQaAnswerDto dto, CancellationToken ct = default);

        Task<List<QaQuestionWithLatestAnswerDto>> GetWithLatestAnswerForUser(QaQuestionSearchObject search, CancellationToken ct = default);
    }
}
