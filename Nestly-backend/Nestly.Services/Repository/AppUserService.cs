using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class AppUserService : IAppUserService
    {
        private readonly NestlyDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppUserService(
            NestlyDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
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

            if (!string.IsNullOrWhiteSpace(search?.FirstName))
            {
                q = q.Where(x => x.FirstName.Contains(search.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(search?.LastName))
            {
                q = q.Where(x => x.LastName.Contains(search.LastName));
            }

            return q.Select(x => new AppUserResultDto
            {
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                RoleId = x.RoleId,
                IdentityUserId = x.IdentityUserId
            }).ToList();
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
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    RoleId = x.RoleId,
                    IdentityUserId = x.IdentityUserId
                })
                .FirstOrDefault();
        }
        private static (DateTime? Lmp, DateTime? Due) NormalizePregnancyDates(DateTime? lmp, DateTime? due)
        {
            if (lmp.HasValue && !due.HasValue)
            {
                return (lmp, lmp.Value.AddDays(280));
            }

            if (!lmp.HasValue && due.HasValue)
            {
                return (due.Value.AddDays(-280), due);
            }

            return (lmp, due);
        }

        public AppUserResultDto Create(CreateAppUserDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email is required.", nameof(dto.Email));
            }

            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                throw new ArgumentException("Username is required.", nameof(dto.Username));
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Password is required.", nameof(dto.Password));
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                throw new ArgumentException("FirstName is required.", nameof(dto.FirstName));
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                throw new ArgumentException("LastName is required.", nameof(dto.LastName));
            }

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                throw new ArgumentException("PhoneNumber is required.", nameof(dto.PhoneNumber));
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new ArgumentException("Gender is required.", nameof(dto.Gender));
            }

            if (_db.AppUsers.Any(u => u.Email == dto.Email))
            {
                throw new ArgumentException("Email already exists in domain.", nameof(dto.Email));
            }

            var role = _db.Roles.FirstOrDefault(r => r.Id == dto.RoleId)
                ?? throw new ArgumentException("Role not found.", nameof(dto.RoleId));

            var identityRoleName = role.Name;
            if (!_roleManager.RoleExistsAsync(identityRoleName).GetAwaiter().GetResult())
            {
                throw new ArgumentException($"Identity role '{identityRoleName}' does not exist. Seed it in AuthDbContext.");
            }

            var identityUser = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var createResult = _userManager.CreateAsync(identityUser, dto.Password).GetAwaiter().GetResult();
            if (!createResult.Succeeded)
            {
                var msg = string.Join("; ", createResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Failed to create Identity user: {msg}");
            }

            var roleResult = _userManager.AddToRoleAsync(identityUser, identityRoleName).GetAwaiter().GetResult();
            if (!roleResult.Succeeded)
            {
                var msg = string.Join("; ", roleResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"Failed to add role '{identityRoleName}' to user: {msg}");
            }

            using var tx = _db.Database.BeginTransaction();
            try
            {
                var user = new AppUser
                {
                    IdentityUserId = identityUser.Id,
                    Email = dto.Email.Trim(),
                    FirstName = dto.FirstName.Trim(),
                    LastName = dto.LastName.Trim(),
                    PhoneNumber = dto.PhoneNumber.Trim(),
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender.Trim(),
                    RoleId = role.Id
                };

                _db.AppUsers.Add(user);
                _db.SaveChanges();

                var roleNameUpper = role.Name.ToUpperInvariant();

                if (roleNameUpper == "PARENT" && !_db.ParentProfiles.Any(p => p.UserId == user.Id))
                {
                    var (normLmp, normDue) = NormalizePregnancyDates(dto.LmpDate, dto.DueDate);
                    var conception = normLmp.HasValue ? normLmp.Value.AddDays(14) : DateTime.UtcNow.Date;

                    var parentProfile = new ParentProfile
                    {
                        UserId = user.Id,
                        ConceptionDate = conception
                    };
                    _db.ParentProfiles.Add(parentProfile);
                    _db.SaveChanges();

                    var preg = new Pregnancy
                    {
                        UserId = parentProfile.Id,
                        LmpDate = normLmp,
                        DueDate = normDue,
                        CycleLengthDays = dto.CycleLengthDays
                    };
                    _db.Pregnancies.Add(preg);
                    _db.SaveChanges();
                }
                else if (roleNameUpper == "DOCTOR" && !_db.DoctorProfiles.Any(d => d.UserId == user.Id))
                {
                    _db.DoctorProfiles.Add(new DoctorProfile { UserId = user.Id });
                    _db.SaveChanges();
                }

                tx.Commit();

                return new AppUserResultDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = user.RoleId,
                    IdentityUserId = user.IdentityUserId
                };
            }
            catch
            {
                tx.Rollback();
                _ = _userManager.DeleteAsync(identityUser).GetAwaiter().GetResult();
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
                u.FirstName = patch.FirstName.Trim();
            }

            if (patch.LastName is not null)
            {
                u.LastName = patch.LastName.Trim();
            }

            if (patch.PhoneNumber is not null)
            {
                u.PhoneNumber = patch.PhoneNumber.Trim();
            }

            if (patch.DateOfBirth is not null)
            {
                u.DateOfBirth = patch.DateOfBirth.Value;
            }

            if (patch.Gender is not null)
            {
                u.Gender = patch.Gender.Trim();
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

            var identityUser = _userManager.FindByIdAsync(u.IdentityUserId).GetAwaiter().GetResult();
            if (identityUser != null)
            {
                var del = _userManager.DeleteAsync(identityUser).GetAwaiter().GetResult();
                if (!del.Succeeded)
                {
                    var msg = string.Join("; ", del.Errors.Select(e => $"{e.Code}: {e.Description}"));
                    throw new InvalidOperationException($"Failed to delete Identity user: {msg}");
                }
            }

            _db.AppUsers.Remove(u);
            _db.SaveChanges();
            return true;
        }
    }
}
