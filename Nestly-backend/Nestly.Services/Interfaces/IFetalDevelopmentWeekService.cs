using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IFetalDevelopmentWeekService
    {
        List<FetalDevelopmentWeek> Get();
        FetalDevelopmentWeek? GetById(int id);
        FetalDevelopmentWeek Create(CreateFetalDevelopmentWeekDto entity);
        FetalDevelopmentWeek? Patch(int id, FetalDevelopmentWeekPatchDto patch);
        bool Delete(int id);
    }
}
