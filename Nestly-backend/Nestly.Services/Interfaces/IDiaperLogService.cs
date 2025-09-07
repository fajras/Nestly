using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IDiaperLogService
    {
        List<DiaperLog> Get(DiaperLogSearchObject? search);
        DiaperLog? GetById(long id);
        DiaperLog Create(DiaperLog entity);
        DiaperLog? Patch(long id, DiaperLogPatchDto patch);
        bool Delete(long id);
    }
}
