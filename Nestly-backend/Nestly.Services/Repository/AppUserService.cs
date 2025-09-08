using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
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
        public List<AppUserResultDto> Get(AppUserSearchObject? search)
        {
            IQueryable<AppUser> q = _db.AppUsers
                .Include(u => u.ParentProfile)
                .Include(u => u.DoctorProfile);

            if (!string.IsNullOrWhiteSpace(search?.Email))
            {
                q = q.Where(x => x.Email == search.Email);
            }

            if (!string.IsNullOrWhiteSpace(search?.Username))
            {
                q = q.Where(x => x.Username.Contains(search.Username));
            }

            if (!string.IsNullOrWhiteSpace(search?.FirstName))
            {
                q = q.Where(x => x.FirstName.Contains(search.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(search?.LastName))
            {
                q = q.Where(x => x.LastName.Contains(search.LastName));
            }

            return q
                .Select(x => new AppUserResultDto
                {
                    Email = x.Email,
                    Username = x.Username,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                })
                .ToList();
        }

        public AppUserResultDto? GetById(long id)
        {
            return _db.AppUsers
                .Include(u => u.ParentProfile)
                .Include(u => u.DoctorProfile)
                .Where(u => u.Id == id)
                .Select(x => new AppUserResultDto
                {
                    Email = x.Email,
                    Username = x.Username,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                })
                .FirstOrDefault();
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

            if (_db.AppUsers.Any(u => u.Email == entity.Email))
            {
                throw new ArgumentException("Email already exists.");
            }

            if (_db.AppUsers.Any(u => u.Username == entity.Username))
            {
                throw new ArgumentException("Username already exists.");
            }

            _db.AppUsers.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public AppUser? Patch(long id, AppUserPatchDto patch)
        {
            var u = _db.AppUsers.FirstOrDefault(x => x.Id == id);
            if (u is null)
            {
                return null;
            }


            if (patch.FirstName is not null)
            {
                u.FirstName = patch.FirstName;
            }

            if (patch.LastName is not null)
            {
                u.LastName = patch.LastName;
            }

            if (patch.PhoneNumber is not null)
            {
                u.PhoneNumber = patch.PhoneNumber;
            }

            if (patch.DateOfBirth is not null)
            {
                u.DateOfBirth = patch.DateOfBirth;
            }

            if (patch.Gender is not null)
            {
                u.Gender = patch.Gender;
            }

            if (patch.Password is not null)
            {
                u.Password = patch.Password;
            }

            _db.SaveChanges();
            return u;
        }

        public bool Delete(long id)
        {
            var u = _db.AppUsers.FirstOrDefault(x => x.Id == id);
            if (u is null)
            {
                return false;
            }

            _db.AppUsers.Remove(u);
            _db.SaveChanges();
            return true;
        }
    }


}
