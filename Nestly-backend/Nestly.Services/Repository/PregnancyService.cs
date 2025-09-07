using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class PregnancyService : IPregnancyService
    {
        private readonly NestlyDbContext _db;
        public PregnancyService(NestlyDbContext db) => _db = db;

        public List<Pregnancy> Get(PregnancySearchObject? search)
        {
            IQueryable<Pregnancy> q = _db.Pregnancies
                                          .Include(p => p.User)
                                          .AsQueryable();

            if (search?.UserId is not null)
            {
                q = q.Where(p => p.UserId == search.UserId);
            }

            if (search?.LmpFrom is not null)
            {
                q = q.Where(p => p.LmpDate >= search.LmpFrom.Value);
            }

            if (search?.LmpTo is not null)
            {
                q = q.Where(p => p.LmpDate <= search.LmpTo.Value);
            }

            if (search?.DueFrom is not null)
            {
                q = q.Where(p => p.DueDate >= search.DueFrom.Value);
            }

            if (search?.DueTo is not null)
            {
                q = q.Where(p => p.DueDate <= search.DueTo.Value);
            }

            return q.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public Pregnancy? GetById(long id)
        {
            return _db.Pregnancies
                      .Include(p => p.User)
                      .FirstOrDefault(p => p.Id == id);
        }

        public Pregnancy Create(Pregnancy entity)
        {
            if (entity.UserId <= 0)
            {
                throw new ArgumentException("UserId is required.");
            }

            if (!_db.AppUsers.Any(u => u.Id == entity.UserId))
            {
                throw new ArgumentException("User does not exist.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            _db.Pregnancies.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public Pregnancy? Patch(long id, PregnancyPatchDto patch)
        {
            var dbEntity = _db.Pregnancies.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            if (patch.UserId is not null && patch.UserId.Value != dbEntity.UserId)
            {
                if (!_db.AppUsers.Any(u => u.Id == patch.UserId.Value))
                {
                    throw new ArgumentException("User does not exist.");
                }

                dbEntity.UserId = patch.UserId.Value;
            }

            if (patch.LmpDate is not null)
            {
                dbEntity.LmpDate = patch.LmpDate.Value;
            }

            if (patch.DueDate is not null)
            {
                dbEntity.DueDate = patch.DueDate.Value;
            }

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.Pregnancies.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.Pregnancies.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }
    }
}
