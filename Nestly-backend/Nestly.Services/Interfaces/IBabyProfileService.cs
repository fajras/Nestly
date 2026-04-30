using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyProfileService
    {
        PagedResult<BabyProfileSummaryDto> Get(BabyProfileSearchObject search);
        BabyProfileSummaryDto? GetById(long id);
        BabyProfileSummaryDto Create(CreateBabyProfileDto entity);
        BabyProfileSummaryDto? Patch(long id, BabyProfilePatchDto patch);
        bool Delete(long id);
        BabyProfileSummaryDto? GetLatestByParent(long parentProfileId);
    }
}
