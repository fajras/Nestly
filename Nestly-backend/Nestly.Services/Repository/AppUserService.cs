using Nestly.Model.Entity;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class AppUserService : IAppUserService
    {
        private readonly NestlyDbContext _db;
        public AppUserService(NestlyDbContext db)
        {
            _db = db;
        }
        public List<AppUser> Get(AppUserSearchObject? search)
        {
            IQueryable<AppUser> q = _db.AppUsers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search?.Email))
            {
                var email = search.Email.Trim();
                q = q.Where(x => x.Email.Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(search?.Username))
            {
                var username = search.Username.Trim();
                q = q.Where(x => x.Username.Contains(username));
            }

            if (!string.IsNullOrWhiteSpace(search?.FirstName))
            {
                var fn = search.FirstName.Trim();
                q = q.Where(x => x.FirstName != null && x.FirstName.Contains(fn));
            }

            if (!string.IsNullOrWhiteSpace(search?.LastName))
            {
                var ln = search.LastName.Trim();
                q = q.Where(x => x.LastName != null && x.LastName.Contains(ln));
            }

            return q.ToList();
        }

        public AppUser? GetById(long id)
        {
            return _db.AppUsers.FirstOrDefault(x => x.Id == id);
        }

        public AppUser Create(AppUser entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(entity.Username))
            {
                throw new ArgumentException("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                throw new ArgumentException("Password is required.");
            }

            _db.AppUsers.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public AppUser? Update(long id, AppUser entity)
        {
            var dbEntity = _db.AppUsers.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return null;
            }

            dbEntity.FirstName = entity.FirstName;
            dbEntity.LastName = entity.LastName;
            dbEntity.PhoneNumber = entity.PhoneNumber;
            dbEntity.DateOfBirth = entity.DateOfBirth;
            dbEntity.Gender = entity.Gender;
            dbEntity.Role = entity.Role;

            _db.SaveChanges();
            return dbEntity;
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.AppUsers.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.AppUsers.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }



    }
}
