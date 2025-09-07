using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IBabyGrowthService
    {
        List<BabyGrowth> Get(BabyGrowthSearchObject? search);
        BabyGrowth? GetById(long id);
        BabyGrowth Create(CreateBabyGrowthDto entity);
        BabyGrowth? Patch(long id, BabyGrowthPatchDto patch);
        bool Delete(long id);
    }
}
