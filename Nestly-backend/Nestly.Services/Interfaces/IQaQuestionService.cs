using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IQaQuestionService
    {
        Task<PagedResult<QaQuestionWithLatestAnswerDto>> GetAllWithLatestAnswer(QaQuestionSearchObject? search, CancellationToken ct = default);
        Task<QaQuestionDto?> GetById(long id, CancellationToken ct = default);
        Task<PagedResult<QaQuestionDto>> GetByUserAsync(long askedByParentProfileId, QaQuestionSearchObject search, CancellationToken ct = default);
        Task<QaQuestionDto> Create(CreateQaQuestionDto dto, CancellationToken ct = default);
        Task<QaQuestionDto?> Patch(long id, QaQuestionPatchDto patch, CancellationToken ct = default);
        Task<bool> Delete(long id, CancellationToken ct = default);
        Task<PagedResult<QaAnswerDto>> GetAnswers(long questionId, QaQuestionSearchObject search, CancellationToken ct);
        Task<QaAnswerDto> CreateAnswer(long questionId, CreateQaAnswerDto dto, CancellationToken ct = default);
        Task<PagedResult<QaQuestionWithLatestAnswerDto>> GetWithLatestAnswerForUser(QaQuestionSearchObject search, CancellationToken ct = default);
    }
}
