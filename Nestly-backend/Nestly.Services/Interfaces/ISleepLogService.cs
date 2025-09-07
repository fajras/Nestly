using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface ISleepLogService
    {
        List<SleepLog> Get(SleepLogSearchObject? search);
        SleepLog? GetById(long id);
        SleepLog Create(SleepLog entity);
        SleepLog? Patch(long id, SleepLogPatchDto patch);
        bool Delete(long id);
    }
}
