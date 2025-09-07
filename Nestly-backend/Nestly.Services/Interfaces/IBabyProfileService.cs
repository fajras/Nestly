using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyProfileService
    {
        List<BabyProfile> Get(BabyProfileSearchObject? search);
        BabyProfile? GetById(long id);
        BabyProfile Create(BabyProfile entity);
        BabyProfile? Patch(long id, BabyProfilePatchDto patch);
        bool Delete(long id);
    }
}
