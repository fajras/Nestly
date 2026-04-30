using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface ISleepLogService
    {
        PagedResult<SleepLogResponseDto> Get(SleepLogSearchObject search);
        SleepLogResponseDto? GetById(long id);
        SleepLogResponseDto Create(CreateSleepLogDto dto);
        SleepLogResponseDto? Patch(long id, SleepLogPatchDto patch);
        bool Delete(long id);
    }
}
