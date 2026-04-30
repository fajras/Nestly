using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class FetalDevelopmentWeekService : IFetalDevelopmentWeekService
    {
        private readonly NestlyDbContext _db;

        public FetalDevelopmentWeekService(NestlyDbContext db)
        {
            _db = db;
        }

        public PagedResult<FetalDevelopmentWeekResponseDto> Get(FetalDevelopmentWeekSearchObject search)
        {
            IQueryable<FetalDevelopmentWeek> q = _db.FetalDevelopmentWeeks.AsQueryable();

            if (search.WeekNumber is not null)
            {
                q = q.Where(x => x.WeekNumber == search.WeekNumber.Value);
            }

            var totalCount = q.Count();

            var items = q
                .OrderBy(x => x.WeekNumber)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResult<FetalDevelopmentWeekResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public FetalDevelopmentWeekResponseDto GetById(int id)
        {
            var entity = _db.FetalDevelopmentWeeks
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Fetal development week not found.");
            }

            return MapToDto(entity);
        }

        public FetalDevelopmentWeekResponseDto? GetByWeekNumber(int weekNumber)
        {
            var entity = _db.FetalDevelopmentWeeks
                .FirstOrDefault(x => x.WeekNumber == weekNumber);

            return entity is null ? null : MapToDto(entity);
        }

        public FetalDevelopmentWeekResponseDto Create(CreateFetalDevelopmentWeekDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (dto.WeekNumber <= 0)
            {
                throw new BusinessException("Week number must be greater than 0.");
            }

            if (_db.FetalDevelopmentWeeks.Any(x => x.WeekNumber == dto.WeekNumber))
            {
                throw new BusinessException($"Week {dto.WeekNumber} already exists.");
            }

            var entity = new FetalDevelopmentWeek
            {
                WeekNumber = dto.WeekNumber,
                ImageUrl = dto.ImageUrl?.Trim(),
                BabyDevelopment = dto.BabyDevelopment?.Trim(),
                MotherChanges = dto.MotherChanges?.Trim()
            };

            _db.FetalDevelopmentWeeks.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }
        public FetalDevelopmentWeekResponseDto Patch(int id, FetalDevelopmentWeekPatchDto patch)
        {
            var entity = _db.FetalDevelopmentWeeks.FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Fetal development week not found.");
            }

            if (patch.BabyDevelopment is not null)
            {
                entity.BabyDevelopment = patch.BabyDevelopment.Trim();
            }

            if (patch.MotherChanges is not null)
            {
                entity.MotherChanges = patch.MotherChanges.Trim();
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public void Delete(int id)
        {
            var entity = _db.FetalDevelopmentWeeks.FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Fetal development week not found.");
            }

            _db.FetalDevelopmentWeeks.Remove(entity);
            _db.SaveChanges();
        }

        private static FetalDevelopmentWeekResponseDto MapToDto(FetalDevelopmentWeek x)
        {
            return new FetalDevelopmentWeekResponseDto
            {
                Id = x.Id,
                WeekNumber = x.WeekNumber,
                ImageUrl = x.ImageUrl,
                BabyDevelopment = x.BabyDevelopment,
                MotherChanges = x.MotherChanges
            };
        }
    }
}
