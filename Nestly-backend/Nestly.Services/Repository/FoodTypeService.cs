using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;
namespace Nestly.Services.Repository
{
    public class FoodTypeService : IFoodTypeService
    {
        private readonly NestlyDbContext _db;

        public FoodTypeService(NestlyDbContext db)
        {
            _db = db;
        }

        private static FoodTypeDto MapToDto(FoodType entity)
        {
            return new FoodTypeDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public PagedResult<FoodTypeDto> Get(FoodTypeSearchObject search)
        {
            IQueryable<FoodType> query = _db.FoodTypes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            var totalCount = query.Count();

            var items = query
                .OrderBy(x => x.Name)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(x => MapToDto(x))
                .ToList();

            return new PagedResult<FoodTypeDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public FoodTypeDto GetById(int id)
        {
            var entity = _db.FoodTypes
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Food type not found.");
            }

            return MapToDto(entity);
        }

        public FoodTypeDto Create(FoodTypeInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }

            var entity = new FoodType
            {
                Name = request.Name.Trim()
            };

            _db.FoodTypes.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public FoodTypeDto Update(int id, FoodTypeUpdateDto request)
        {
            var entity = _db.FoodTypes.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Food type not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entity.Name = request.Name.Trim();
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public void Delete(int id)
        {
            var entity = _db.FoodTypes.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Food type not found.");
            }

            bool usedInMealPlan = _db.MealPlans.Any(x => x.FoodTypeId == id);

            if (usedInMealPlan)
            {
                throw new BusinessException("Food type is used in meal plans and cannot be deleted.");
            }

            _db.FoodTypes.Remove(entity);
            _db.SaveChanges();
        }
    }
}
