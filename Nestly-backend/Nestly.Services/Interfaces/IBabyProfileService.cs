using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyProfileService
    {
        Task<PagedResult<BabyProfileSummaryDto>> Get(
            BabyProfileSearchObject search);

        Task<BabyProfileSummaryDto> GetById(long id);

        Task<BabyProfileSummaryDto> Create(
            CreateBabyProfileDto dto);

        Task<BabyProfileSummaryDto> Patch(
            long id,
            BabyProfilePatchDto patch);

        Task Delete(long id);

        Task<BabyProfileSummaryDto?> GetLatestByParent(
            long parentProfileId);
    }
}