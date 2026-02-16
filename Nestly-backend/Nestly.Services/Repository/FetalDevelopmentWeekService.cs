using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
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

        public List<FetalDevelopmentWeekResponseDto> Get()
        {
            return _db.FetalDevelopmentWeeks
                .OrderBy(x => x.WeekNumber)
                .Select(MapToDto)
                .ToList();
        }

        public FetalDevelopmentWeekResponseDto? GetById(int id)
        {
            var entity = _db.FetalDevelopmentWeeks
                .FirstOrDefault(x => x.Id == id);

            return entity is null ? null : MapToDto(entity);
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
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be greater than 0.");
            }

            if (_db.FetalDevelopmentWeeks.Any(x => x.WeekNumber == dto.WeekNumber))
            {
                throw new InvalidOperationException($"Week {dto.WeekNumber} already exists.");
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

        public FetalDevelopmentWeekResponseDto? Patch(int id, FetalDevelopmentWeekPatchDto patch)
        {
            var entity = _db.FetalDevelopmentWeeks.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return null;
            }

            if (patch.WeekNumber is not null)
            {
                entity.WeekNumber = patch.WeekNumber.Value;
            }

            if (patch.ImageUrl is not null)
            {
                entity.ImageUrl = patch.ImageUrl.Trim();
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

        public bool Delete(int id)
        {
            var entity = _db.FetalDevelopmentWeeks.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return false;
            }

            _db.FetalDevelopmentWeeks.Remove(entity);
            _db.SaveChanges();
            return true;
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
