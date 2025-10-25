using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IFetalDevelopmentWeekService
    {
        List<FetalDevelopmentWeek> Get();
        CreateFetalDevelopmentWeekDto? GetById(int id);
        CreateFetalDevelopmentWeekDto GetByWeekNumber(int weekNumber);
        FetalDevelopmentWeek Create(CreateFetalDevelopmentWeekDto entity);
        FetalDevelopmentWeek? Patch(int id, FetalDevelopmentWeekPatchDto patch);
        bool Delete(int id);
    }
}
