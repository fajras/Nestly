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

        public async Task<PagedResult<FoodTypeDto>> Get(FoodTypeSearchObject search)
        {
            IQueryable<FoodType> query = _db.FoodTypes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            var totalCount = await query.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<FoodTypeDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<FoodTypeDto> GetById(int id)
        {
            var entity = await _db.FoodTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Food type not found.");
            }

            return MapToDto(entity);
        }

        public async Task<FoodTypeDto> Create(FoodTypeInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BusinessException("Name is required.");
            }

            var name = request.Name.Trim();

            bool duplicate = await _db.FoodTypes
                .AnyAsync(x => x.Name.ToLower() == name.ToLower());

            if (duplicate)
            {
                throw new BusinessException("A food type with this name already exists.");
            }

            var entity = new FoodType
            {
                Name = name
            };

            _db.FoodTypes.Add(entity);
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<FoodTypeDto> Update(int id, FoodTypeUpdateDto request)
        {
            var entity = await _db.FoodTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Food type not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();

                bool duplicate = await _db.FoodTypes
                    .AnyAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());

                if (duplicate)
                {
                    throw new BusinessException("A food type with this name already exists.");
                }

                entity.Name = name;
            }

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FoodTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Food type not found.");
            }

            bool usedInMealPlan = await _db.MealPlans.AnyAsync(x => x.FoodTypeId == id);

            if (usedInMealPlan)
            {
                throw new BusinessException("Food type is used in meal plans and cannot be deleted.");
            }

            bool usedInFeedingLog = await _db.FeedingLogs.AnyAsync(x => x.FoodTypeId == id);

            if (usedInFeedingLog)
            {
                throw new BusinessException("Food type is used in feeding logs and cannot be deleted.");
            }

            _db.FoodTypes.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
