using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface ISymptomDiaryService
    {
        SymptomDiaryResponseDto Create(CreateSymptomDiaryDto dto);
        PagedResult<SymptomDiaryResponseDto> Get(SymptomDiarySearchObject search);
        PagedResult<SymptomDiaryResponseDto> GetByParent(long parentProfileId, SymptomDiarySearchObject search);
        PagedResult<DateTime> GetMarkedDays(long parentProfileId, SymptomDiarySearchObject search);
        SymptomDiaryResponseDto GetByDate(long parentProfileId, DateTime date);
        SymptomDiaryResponseDto? Patch(long id, SymptomDiaryPatchDto patch);
        void Delete(long id);
    }
}
