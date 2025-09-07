using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface ISleepLogService
    {
        List<SleepLog> Get(SleepLogSearchObject? search);
        SleepLog? GetById(long id);
        SleepLog Create(CreateSleepLogDto entity);
        SleepLog? Patch(long id, SleepLogPatchDto patch);
        bool Delete(long id);
    }
}
