using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IBabyProfileService
    {
        List<BabyProfile> Get(BabyProfileSearchObject? search);
        BabyProfile? GetById(long id);
        BabyProfile Create(CreateBabyProfileDto entity);
        BabyProfile? Patch(long id, BabyProfilePatchDto patch);
        bool Delete(long id);
    }
}
