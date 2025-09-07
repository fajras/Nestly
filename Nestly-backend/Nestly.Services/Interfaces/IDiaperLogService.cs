using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IDiaperLogService
    {
        List<DiaperLog> Get(DiaperLogSearchObject? search);
        DiaperLog? GetById(long id);
        DiaperLog Create(CreateDiaperLogDto entity);
        DiaperLog? Patch(long id, DiaperLogPatchDto patch);
        bool Delete(long id);
    }
}
