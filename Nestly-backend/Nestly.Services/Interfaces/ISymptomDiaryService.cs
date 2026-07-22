using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface ISymptomDiaryService
    {
        Task<SymptomDiaryResponseDto> Create(CreateSymptomDiaryDto dto);
        Task<PagedResult<SymptomDiaryResponseDto>> Get(SymptomDiarySearchObject search);
        Task<PagedResult<SymptomDiaryResponseDto>> GetByParent(long parentProfileId, SymptomDiarySearchObject search);
        Task<PagedResult<DateTime>> GetMarkedDays(long parentProfileId, SymptomDiarySearchObject search);
        Task<SymptomDiaryResponseDto> GetByDate(long parentProfileId, DateTime date);
        Task<SymptomDiaryResponseDto?> Patch(long id, SymptomDiaryPatchDto patch);
        Task Delete(long id);
    }
}
