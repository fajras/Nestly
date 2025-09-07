using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IQaAnswerService
    {
        List<QaAnswer> Get(QaAnswerSearchObject? search);
        QaAnswer? GetById(long id);
        QaAnswer Create(QaAnswer entity);
        QaAnswer? Patch(long id, QaAnswerPatchDto patch);
        bool Delete(long id);
    }
}
