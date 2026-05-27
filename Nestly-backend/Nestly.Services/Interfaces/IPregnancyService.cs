using Nestly.Model.DTOObjects;

public interface IPregnancyService
{
    Task<PagedResult<PregnancyResponseDto>> Get(
           PregnancySearchObject search);
    Task<PregnancyResponseDto> GetById(long id);
    Task<PregnancyResponseDto?> GetByParentProfileId(
        long parentProfileId);
    Task<PregnancyResponseDto> Create(
        CreatePregnancyDto dto);
    Task<PregnancyResponseDto> Patch(
        long id,
        PregnancyPatchDto patch);
    Task Delete(long id);
    Task<PregnancyStatusDto?> GetStatus(
        long parentProfileId);
}
