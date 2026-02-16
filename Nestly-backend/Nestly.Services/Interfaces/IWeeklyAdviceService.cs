using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IWeeklyAdviceService
    {
        IEnumerable<WeeklyAdviceResponseDto> Get();
        WeeklyAdviceResponseDto? GetById(int id);
        WeeklyAdviceResponseDto? GetByWeek(short weekNumber);
        WeeklyAdviceResponseDto Create(CreateWeeklyAdviceDto dto);
        WeeklyAdviceResponseDto? Patch(int id, WeeklyAdvicePatchDto patch);
        bool Delete(int id);
    }
}
