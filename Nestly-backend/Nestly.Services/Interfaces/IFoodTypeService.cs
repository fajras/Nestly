using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IFoodTypeService
    {
        Task<PagedResult<FoodTypeDto>> Get(FoodTypeSearchObject search);
        Task<FoodTypeDto> GetById(int id);
        Task<FoodTypeDto> Create(FoodTypeInsertDto request);
        Task<FoodTypeDto> Update(int id, FoodTypeUpdateDto request);
        Task Delete(int id);
    }
}
