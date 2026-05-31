using Nestly.Model.DTOObjects;

public interface IDiaperLogService
{
    PagedResult<DiaperLogResponseDto> Get(DiaperLogSearchObject search);
    PagedResult<DiaperLogResponseDto> GetByParent(long parentProfileId, DiaperLogSearchObject search);
    DiaperLogResponseDto GetById(long id);
    DiaperLogResponseDto Create(CreateDiaperLogDto entity);
    DiaperLogResponseDto Patch(long id, DiaperLogPatchDto patch);
    void Delete(long id);
}
