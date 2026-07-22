using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyGrowthService
    {
        Task<PagedResult<BabyGrowthResponseDto>> Get(BabyGrowthSearchObject search);
        Task<BabyGrowthResponseDto> GetById(long id);
        Task<BabyGrowthResponseDto> Create(CreateBabyGrowthDto request);
        Task<BabyGrowthResponseDto> Patch(long id, BabyGrowthPatchDto patch);
        Task Delete(long id);
        Task<PagedResult<BabyGrowthResponseDto>> GetByParent(long parentProfileId, BabyGrowthSearchObject search);

    }
}
