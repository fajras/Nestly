using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyGrowthService
    {
        PagedResult<BabyGrowthResponseDto> Get(BabyGrowthSearchObject search);
        BabyGrowthResponseDto GetById(long id);
        BabyGrowthResponseDto Create(CreateBabyGrowthDto request);
        BabyGrowthResponseDto Patch(long id, BabyGrowthPatchDto patch);
        void Delete(long id);

    }
}
