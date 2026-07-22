using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IWeeklyAdviceService
    {
        Task<PagedResult<WeeklyAdviceResponseDto>> Get(WeeklyAdviceSearchObject search);
        Task<WeeklyAdviceResponseDto> GetById(int id);
        Task<WeeklyAdviceResponseDto?> GetByWeek(short weekNumber);
        Task<WeeklyAdviceResponseDto> Create(CreateWeeklyAdviceDto dto);
        Task<WeeklyAdviceResponseDto?> Patch(int id, WeeklyAdvicePatchDto patch);
        Task Delete(int id);
    }
}
