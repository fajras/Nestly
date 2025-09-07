using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IQaAnswerService
    {
        List<QaAnswer> Get(QaAnswerSearchObject? search);
        QaAnswer? GetById(long id);
        QaAnswer Create(CreateQaAnswerDto entity);
        QaAnswer? Patch(long id, QaAnswerPatchDto patch);
        bool Delete(long id);
    }
}
