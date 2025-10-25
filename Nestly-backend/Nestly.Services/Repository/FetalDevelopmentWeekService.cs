using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class FetalDevelopmentWeekService : IFetalDevelopmentWeekService
    {
        private readonly NestlyDbContext _db;
        public FetalDevelopmentWeekService(NestlyDbContext db) => _db = db;

        public List<FetalDevelopmentWeek> Get()
        {

            return _db.FetalDevelopmentWeeks.ToList();
        }

        public CreateFetalDevelopmentWeekDto? GetById(int id)
        {
            return _db.FetalDevelopmentWeeks
                .Where(f => f.Id == id)
                .Select(f => new CreateFetalDevelopmentWeekDto
                {
                    WeekNumber = f.WeekNumber,
                    ImageUrl = f.ImageUrl,
                    BabyDevelopment = f.BabyDevelopment,
                    MotherChanges = f.MotherChanges
                })
                .FirstOrDefault();
        }

        public CreateFetalDevelopmentWeekDto? GetByWeekNumber(int weekNumber)
        {
            return _db.FetalDevelopmentWeeks
                .Where(f => f.WeekNumber == weekNumber)
                .Select(f => new CreateFetalDevelopmentWeekDto
                {
                    WeekNumber = f.WeekNumber,
                    ImageUrl = f.ImageUrl,
                    BabyDevelopment = f.BabyDevelopment,
                    MotherChanges = f.MotherChanges
                })
                .FirstOrDefault();
        }

        public FetalDevelopmentWeek Create(CreateFetalDevelopmentWeekDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.", nameof(dto.WeekNumber));
            }

            bool exists = _db.FetalDevelopmentWeeks.Any(x => x.WeekNumber == dto.WeekNumber);
            if (exists)
            {
                throw new InvalidOperationException($"Week {dto.WeekNumber} already exists.");
            }

            var entity = new FetalDevelopmentWeek
            {
                WeekNumber = dto.WeekNumber,
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim(),
                BabyDevelopment = string.IsNullOrWhiteSpace(dto.BabyDevelopment) ? null : dto.BabyDevelopment.Trim(),
                MotherChanges = string.IsNullOrWhiteSpace(dto.MotherChanges) ? null : dto.MotherChanges.Trim()
            };

            _db.FetalDevelopmentWeeks.Add(entity);
            _db.SaveChanges();

            return entity;
        }

        public FetalDevelopmentWeek? Patch(int id, FetalDevelopmentWeekPatchDto patch)
        {
            var dbEntity = _db.FetalDevelopmentWeeks.FirstOrDefault(f => f.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.WeekNumber is not null)
            {
                dbEntity.WeekNumber = patch.WeekNumber.Value;
            }

            if (patch.ImageUrl is not null)
            {
                dbEntity.ImageUrl = patch.ImageUrl;
            }

            if (patch.BabyDevelopment is not null)
            {
                dbEntity.BabyDevelopment = patch.BabyDevelopment;
            }

            if (patch.MotherChanges is not null)
            {
                dbEntity.MotherChanges = patch.MotherChanges;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(int id)
        {
            var dbEntity = _db.FetalDevelopmentWeeks.FirstOrDefault(f => f.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.FetalDevelopmentWeeks.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }
}
