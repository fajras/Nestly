using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyGrowthService
    {
        List<BabyGrowth> Get(BabyGrowthSearchObject? search);
        BabyGrowth? GetById(long id);
        BabyGrowth Create(BabyGrowth entity);
        BabyGrowth? Patch(long id, BabyGrowthPatchDto patch);
        bool Delete(long id);
    }
}
