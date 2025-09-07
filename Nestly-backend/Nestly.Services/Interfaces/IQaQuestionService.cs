using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IQaQuestionService
    {
        Task<List<QaQuestion>> Get(QaQuestionSearchObject? search);
        Task<QaQuestion?> GetById(long id);
        Task<QaQuestion> Create(QaQuestion entity);
        Task<QaQuestion?> Patch(long id, QaQuestionPatchDto patch);
        Task<bool> Delete(long id);
        Task<QaAnswer> CreateAnswer(long questionId, QaAnswer answer);
        Task<List<QaAnswer>> GetAnswers(long questionId);
    }
}
