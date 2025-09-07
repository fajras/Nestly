using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IQaQuestionService
    {
        Task<List<QaQuestion>> Get(QaQuestionSearchObject? search);
        Task<QaQuestion?> GetById(long id);
        Task<QaQuestion> Create(CreateQaQuestionDto entity);
        Task<QaQuestion?> Patch(long id, QaQuestionPatchDto patch);
        Task<bool> Delete(long id);
        Task<QaAnswer> CreateAnswer(long questionId, QaAnswer answer);
        Task<List<QaAnswer>> GetAnswers(long questionId);
    }
}
