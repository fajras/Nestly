using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class MilestoneService : IMilestoneService
    {
        private readonly NestlyDbContext _db;
        public MilestoneService(NestlyDbContext db) => _db = db;

        public List<Milestone> Get(MilestoneSearchObject? search)
        {
            IQueryable<Milestone> q = _db.Milestones
                                         .Include(x => x.Baby)
                                         .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.AchievedDate >= search.DateFrom.Value);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.AchievedDate <= search.DateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(search?.Title))
            {
                q = q.Where(x => x.Title.Contains(search.Title));
            }

            return q.OrderBy(x => x.AchievedDate).ToList();
        }

        public Milestone? GetById(long id)
        {
            return _db.Milestones
                      .Include(x => x.Baby)
                      .FirstOrDefault(x => x.Id == id);
        }

        public Milestone Create(Milestone entity)
        {
            if (entity.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == entity.BabyId))
            {
                throw new ArgumentException("Baby does not exist.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            _db.Milestones.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public Milestone? Patch(long id, MilestonePatchDto patch)
        {
            var dbEntity = _db.Milestones.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.Title is not null)
            {
                dbEntity.Title = patch.Title;
            }

            if (patch.AchievedDate is not null)
            {
                dbEntity.AchievedDate = patch.AchievedDate.Value;
            }

            if (patch.Notes is not null)
            {
                dbEntity.Notes = patch.Notes;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.Milestones.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.Milestones.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }

}
