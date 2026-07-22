using Nestly.Model.DTOObjects;

public interface IDiaperLogService
{
    Task<PagedResult<DiaperLogResponseDto>> Get(DiaperLogSearchObject search);
    Task<PagedResult<DiaperLogResponseDto>> GetByParent(long parentProfileId, DiaperLogSearchObject search);
    Task<DiaperLogResponseDto> GetById(long id);
    Task<DiaperLogResponseDto> Create(CreateDiaperLogDto entity);
    Task<DiaperLogResponseDto> Patch(long id, DiaperLogPatchDto patch);
    Task Delete(long id);
}
