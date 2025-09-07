using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
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

        public WeeklyAdvice Create(WeeklyAdvice entity)
        {
            if (entity.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.");
            }

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
