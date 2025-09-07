using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class WeeklyAdviceService : IWeeklyAdviceService
    {
        private readonly NestlyDbContext _db;
        public WeeklyAdviceService(NestlyDbContext db) => _db = db;

        public List<WeeklyAdvice> Get()
        {
            return _db.WeeklyAdvices.ToList();
        }

        public WeeklyAdvice? GetById(int id)
            => _db.WeeklyAdvices.FirstOrDefault(w => w.Id == id);

        public WeeklyAdvice Create(CreateWeeklyAdviceDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.", nameof(dto.WeekNumber));
            }

            if (string.IsNullOrWhiteSpace(dto.AdviceText))
            {
                throw new ArgumentException("AdviceText is required.", nameof(dto.AdviceText));
            }

            bool exists = _db.WeeklyAdvices.Any(x => x.WeekNumber == dto.WeekNumber);
            if (exists)
            {
                throw new InvalidOperationException($"Advice for week {dto.WeekNumber} already exists.");
            }

            var entity = new WeeklyAdvice
            {
                WeekNumber = dto.WeekNumber,
                AdviceText = dto.AdviceText.Trim()
            };

            _db.WeeklyAdvices.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public WeeklyAdvice? Patch(int id, WeeklyAdvicePatchDto patch)
        {
            var dbEntity = _db.WeeklyAdvices.FirstOrDefault(w => w.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.WeekNumber is not null)
            {
                dbEntity.WeekNumber = patch.WeekNumber.Value;
            }

            if (patch.AdviceText is not null)
            {
                dbEntity.AdviceText = patch.AdviceText;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(int id)
        {
            var dbEntity = _db.WeeklyAdvices.FirstOrDefault(w => w.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.WeeklyAdvices.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }
}
