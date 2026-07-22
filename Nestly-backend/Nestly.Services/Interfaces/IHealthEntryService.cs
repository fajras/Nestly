using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IHealthEntryService
    {
        Task<PagedResult<HealthEntryResponseDto>> Get(HealthEntrySearchObject search);
        Task<HealthEntryResponseDto> GetById(long id);
        Task<HealthEntryResponseDto> Create(CreateHealthEntryDto entity);
        Task<HealthEntryResponseDto> Patch(long id, HealthEntryPatchDto patch);
        Task Delete(long id);
        Task<PagedResult<HealthEntryResponseDto>> GetByParent(long parentProfileId, HealthEntrySearchObject search);
    }
}
