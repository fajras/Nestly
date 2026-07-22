using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface ISleepLogService
    {
        Task<PagedResult<SleepLogResponseDto>> Get(SleepLogSearchObject search);
        Task<SleepLogResponseDto> GetById(long id);
        Task<SleepLogResponseDto> Create(CreateSleepLogDto dto);
        Task<SleepLogResponseDto> Patch(long id, SleepLogPatchDto patch);
        Task Delete(long id);
        Task<PagedResult<SleepLogResponseDto>> GetByParent(
            long parentProfileId,
            SleepLogSearchObject search);
    }
}
