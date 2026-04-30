using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IFoodTypeService
    {
        PagedResult<FoodTypeDto> Get(FoodTypeSearchObject search);
        FoodTypeDto? GetById(int id);
        FoodTypeDto Create(FoodTypeInsertDto request);
        FoodTypeDto? Update(int id, FoodTypeUpdateDto request);
        bool Delete(int id);
    }
}
