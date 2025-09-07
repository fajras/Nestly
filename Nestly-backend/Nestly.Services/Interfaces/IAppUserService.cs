using Nestly.Model.Entity;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IAppUserService
    {
        List<AppUser> Get(AppUserSearchObject? search);
        AppUser? GetById(long id);
        AppUser Create(AppUser entity);
        AppUser? Update(long id, AppUser entity);
        bool Delete(long id);
    }
}
