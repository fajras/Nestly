using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IHealthEntryService
    {
        PagedResult<HealthEntryResponseDto> Get(HealthEntrySearchObject search);
        HealthEntryResponseDto GetById(long id);
        HealthEntryResponseDto Create(CreateHealthEntryDto entity);
        HealthEntryResponseDto Patch(long id, HealthEntryPatchDto patch);
        void Delete(long id);
    }
}
