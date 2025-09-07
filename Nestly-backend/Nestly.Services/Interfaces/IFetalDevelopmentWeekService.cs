using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IFetalDevelopmentWeekService
    {
        List<FetalDevelopmentWeek> Get();
        FetalDevelopmentWeek? GetById(int id);
        FetalDevelopmentWeek Create(FetalDevelopmentWeek entity);
        FetalDevelopmentWeek? Patch(int id, FetalDevelopmentWeekPatchDto patch);
        bool Delete(int id);
    }
}
