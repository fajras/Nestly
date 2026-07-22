using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IFetalDevelopmentWeekService
    {
        Task<PagedResult<FetalDevelopmentWeekResponseDto>> Get(FetalDevelopmentWeekSearchObject search);
        Task<FetalDevelopmentWeekResponseDto> GetById(int id);
        Task<FetalDevelopmentWeekResponseDto?> GetByWeekNumber(int weekNumber);
        Task<FetalDevelopmentWeekResponseDto> Create(CreateFetalDevelopmentWeekDto entity);
        Task<FetalDevelopmentWeekResponseDto> Patch(int id, FetalDevelopmentWeekPatchDto patch);
        Task Delete(int id);
    }
}
