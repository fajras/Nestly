using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IFetalDevelopmentWeekService
    {
        PagedResult<FetalDevelopmentWeekResponseDto> Get(FetalDevelopmentWeekSearchObject search);
        FetalDevelopmentWeekResponseDto? GetById(int id);
        FetalDevelopmentWeekResponseDto? GetByWeekNumber(int weekNumber);
        FetalDevelopmentWeekResponseDto Create(CreateFetalDevelopmentWeekDto entity);
        FetalDevelopmentWeekResponseDto? Patch(int id, FetalDevelopmentWeekPatchDto patch);
        bool Delete(int id);
    }
}
