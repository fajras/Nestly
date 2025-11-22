using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;


namespace Nestly.Services.Interfaces
{
    public interface IPregnancyService
    {
        List<Pregnancy> Get(PregnancySearchObject? search);
        Pregnancy? GetById(long id);
        Pregnancy Create(CreatePregnancyDto entity);
        Pregnancy? Patch(long id, PregnancyPatchDto patch);
        bool Delete(long id);
        PregnancyStatusDto? GetStatus(long parentProfileId);
    }
}
