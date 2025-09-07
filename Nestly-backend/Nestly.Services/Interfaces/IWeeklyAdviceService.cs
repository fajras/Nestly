using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IWeeklyAdviceService
    {
        List<WeeklyAdvice> Get();
        WeeklyAdvice? GetById(int id);
        WeeklyAdvice Create(WeeklyAdvice entity);
        WeeklyAdvice? Patch(int id, WeeklyAdvicePatchDto patch);
        bool Delete(int id);
    }
}
