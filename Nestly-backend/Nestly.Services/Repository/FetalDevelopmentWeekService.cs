using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
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

        public FetalDevelopmentWeek? GetById(int id)
            => _db.FetalDevelopmentWeeks.FirstOrDefault(f => f.Id == id);

        public FetalDevelopmentWeek Create(FetalDevelopmentWeek entity)
        {
            if (entity.WeekNumber <= 0)
            {
                throw new ArgumentException("WeekNumber must be > 0.");
            }

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
