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
                    RoleId = x.RoleId
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


        public AppUser Create(CreateAppUserDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // --- validacije koje već imaš (skraćeno) ---
            var email = dto.Email?.Trim();
            var username = dto.Username?.Trim();
            var firstName = dto.FirstName?.Trim();
            var lastName = dto.LastName?.Trim();
            var phone = dto.PhoneNumber?.Trim();
            var gender = dto.Gender?.Trim();
            var password = dto.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.", nameof(dto.Email));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username is required.", nameof(dto.Username));
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("FirstName is required.", nameof(dto.FirstName));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("LastName is required.", nameof(dto.LastName));
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("PhoneNumber is required.", nameof(dto.PhoneNumber));
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                throw new ArgumentException("Gender is required.", nameof(dto.Gender));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required.", nameof(dto.Password));
            }

            if (_db.AppUsers.Any(u => u.Email == email))
            {
                throw new ArgumentException("Email already exists.", nameof(dto.Email));
            }

            if (_db.AppUsers.Any(u => u.Username == username))
            {
                throw new ArgumentException("Username already exists.", nameof(dto.Username));
            }

            var role = _db.Roles.FirstOrDefault(r => r.Id == dto.RoleId)
                ?? throw new ArgumentException("Role not found.", nameof(dto.RoleId));

            using var tx = _db.Database.BeginTransaction();
            try
            {
                var user = new AppUser
                {
                    Email = dto.Email.Trim(),
                    FirstName = dto.FirstName.Trim(),
                    LastName = dto.LastName.Trim(),
                    PhoneNumber = dto.PhoneNumber.Trim(),
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender.Trim(),
                    Username = dto.Username.Trim(),
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    RoleId = role.Id
                };

                _db.AppUsers.Add(user);
                _db.SaveChanges();

                var roleName = role.Name.ToUpperInvariant();
                if (roleName == "PARENT" && !_db.ParentProfiles.Any(p => p.UserId == user.Id))
                {
                    _db.ParentProfiles.Add(new ParentProfile { UserId = user.Id });
                    _db.SaveChanges();
                }
                else if (roleName == "DOCTOR" && !_db.DoctorProfiles.Any(d => d.UserId == user.Id))
                {
                    _db.DoctorProfiles.Add(new DoctorProfile { UserId = user.Id });
                    _db.SaveChanges();
                }

                tx.Commit();
                return user;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
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
                u.Password = BCrypt.Net.BCrypt.HashPassword(patch.Password);
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
