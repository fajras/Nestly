using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface ISymptomDiaryService
    {
        SymptomDiaryResponseDto Create(CreateSymptomDiaryDto dto);
        IEnumerable<SymptomDiaryResponseDto> GetByParent(long parentProfileId);
        SymptomDiaryResponseDto? GetByDate(long parentProfileId, DateTime date);
        SymptomDiaryResponseDto? Patch(long id, SymptomDiaryPatchDto patch);
    }
}
