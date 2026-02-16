using Nestly.Model.DTOObjects;

public interface IDiaperLogService
{
    IEnumerable<DiaperLogResponseDto> Get(DiaperLogSearchObject? search);
    DiaperLogResponseDto? GetById(long id);
    DiaperLogResponseDto Create(CreateDiaperLogDto entity);
    DiaperLogResponseDto? Patch(long id, DiaperLogPatchDto patch);
    bool Delete(long id);
}
