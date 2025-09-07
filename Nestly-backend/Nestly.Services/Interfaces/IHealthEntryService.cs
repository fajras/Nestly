using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IHealthEntryService
    {
        List<HealthEntry> Get(HealthEntrySearchObject? search);
        HealthEntry? GetById(long id);
        HealthEntry Create(CreateHealthEntryDto entity);
        HealthEntry? Patch(long id, HealthEntryPatchDto patch);
        bool Delete(long id);
    }
}
