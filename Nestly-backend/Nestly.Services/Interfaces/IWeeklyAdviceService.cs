using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IWeeklyAdviceService
    {
        List<WeeklyAdvice> Get();
        WeeklyAdvice? GetById(int id);
        GetWeeklyAdviceDto GetByWeek(int week);
        WeeklyAdvice Create(CreateWeeklyAdviceDto entity);
        WeeklyAdvice? Patch(int id, WeeklyAdvicePatchDto patch);
        bool Delete(int id);
    }
}
