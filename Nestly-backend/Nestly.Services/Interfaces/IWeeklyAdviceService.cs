using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IWeeklyAdviceService
    {
        PagedResult<WeeklyAdviceResponseDto> Get(WeeklyAdviceSearchObject search);
        WeeklyAdviceResponseDto GetById(int id);
        WeeklyAdviceResponseDto? GetByWeek(short weekNumber);
        WeeklyAdviceResponseDto Create(CreateWeeklyAdviceDto dto);
        WeeklyAdviceResponseDto? Patch(int id, WeeklyAdvicePatchDto patch);
        void Delete(int id);
    }
}
