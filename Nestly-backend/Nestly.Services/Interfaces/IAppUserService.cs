using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IAppUserService
    {
        List<AppUserResultDto> Get(AppUserSearchObject? search);
        AppUserResultDto? GetById(long id);
        AppUser Create(CreateAppUserDto entity);
        AppUser? Patch(long id, AppUserPatchDto patch);
        bool Delete(long id);
    }

}
