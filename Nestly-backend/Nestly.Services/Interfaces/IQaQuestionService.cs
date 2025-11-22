using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IQaQuestionService
    {
        Task<List<QaQuestion>> Get(QaQuestionSearchObject? search, CancellationToken ct = default);
        Task<QaQuestion?> GetById(long id, CancellationToken ct = default);
        Task<List<QaQuestion>> GetByUserAsync(long askedByUserId, CancellationToken ct = default);
        Task<QaQuestion> Create(CreateQaQuestionDto dto, CancellationToken ct = default);
        Task<QaQuestion?> Patch(long id, QaQuestionPatchDto patch, CancellationToken ct = default);
        Task<bool> Delete(long id, CancellationToken ct = default);
        Task<QaAnswer> CreateAnswer(long questionId, QaAnswer answer, CancellationToken ct = default);
        Task<List<QaAnswer>> GetAnswers(long questionId, CancellationToken ct = default);
    }
}
