using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IHealthDeviationAlertService
    {
        Task<PagedResult<HealthDeviationAlertResponseDto>> GetByParent(
            long parentProfileId, HealthDeviationAlertSearchObject search);

        Task<HealthDeviationAlertResponseDto> Resolve(long id);
    }
}
