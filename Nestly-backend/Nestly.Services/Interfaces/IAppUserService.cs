using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IAppUserService
    {
        PagedResult<AppUserResultDto> Get(AppUserSearchObject? search);
        AppUserResultDto? GetById(long id);
        AppUserResultDto Create(CreateAppUserDto entity);
        AppUserResultDto? Patch(long id, AppUserPatchDto patch);
        bool Delete(long id);
    }
}
