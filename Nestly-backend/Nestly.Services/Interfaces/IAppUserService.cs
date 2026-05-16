using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<AppUserResultDto> Create(CreateAppUserDto dto);
        Task<AppUserResultDto?> Patch(long id, AppUserPatchDto patch);
        Task Delete(long id);
        Task<AppUserResultDto?> GetById(long id);
        Task<PagedResult<AppUserResultDto>> Get(AppUserSearchObject search);
    }
}
