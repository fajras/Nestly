using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IHealthEntryService
    {
        List<HealthEntry> Get(HealthEntrySearchObject? search);
        HealthEntry? GetById(long id);
        HealthEntry Create(HealthEntry entity);
        HealthEntry? Patch(long id, HealthEntryPatchDto patch);
        bool Delete(long id);
    }
}
