using Nestly.Model.DTOObjects;

public interface IPregnancyService
{
    PagedResult<PregnancyResponseDto> Get(PregnancySearchObject search);
    PregnancyResponseDto? GetById(long id);
    PregnancyResponseDto? GetByParentProfileId(long parentProfileId);
    PregnancyResponseDto Create(CreatePregnancyDto entity);
    PregnancyResponseDto? Patch(long id, PregnancyPatchDto patch);
    bool Delete(long id);
    PregnancyStatusDto? GetStatus(long parentProfileId);
}
