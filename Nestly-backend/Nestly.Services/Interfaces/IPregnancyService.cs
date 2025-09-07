using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IPregnancyService
    {
        List<Pregnancy> Get(PregnancySearchObject? search);
        Pregnancy? GetById(long id);
        Pregnancy Create(Pregnancy entity);
        Pregnancy? Patch(long id, PregnancyPatchDto patch);
        bool Delete(long id);
    }
}
