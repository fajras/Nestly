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
            var nowUtc = DateTime.UtcNow;

            IQueryable<AppUser> q = _db.AppUsers.AsNoTracking();

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
            if (search?.RoleId.HasValue == true)
            {
                q = q.Where(x => x.RoleId == search.RoleId.Value);
            }

            var rows = q.Select(x => new AppUserRow
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                RoleId = x.RoleId,
                IdentityUserId = x.IdentityUserId,
                ParentProfileId = x.ParentProfile != null ? x.ParentProfile.Id : null,
                LatestBabyBirthDate = x.ParentProfile != null
                    ? x.ParentProfile.Babies
                        .OrderByDescending(b => b.BirthDate)
                        .Select(b => (DateTime?)b.BirthDate)
                        .FirstOrDefault()
                    : null,

                LatestPregnancyDueDate = x.ParentProfile != null
                ? x.ParentProfile.Pregnancies
                    .OrderByDescending(p => p.DueDate ?? p.LmpDate)
                    .Select(p => p.DueDate)
                    .FirstOrDefault()
                : null

            }).OrderBy(x => x.LastName ?? "")
            .ThenBy(x => x.FirstName ?? "")
            .ToList();


            return rows.Select(r =>
            {
                var dto = new AppUserResultDto
                {
                    Id = r.Id,
                    Email = r.Email,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    PhoneNumber = r.PhoneNumber,
                    DateOfBirth = r.DateOfBirth,
                    Gender = r.Gender,
                    RoleId = r.RoleId,
                    IdentityUserId = r.IdentityUserId,
                    ParentStatus = "UNKNOWN",
                    BabyAgeMonths = null,
                    PregnancyTrimester = null,
                    ParentProfileId = r.ParentProfileId
                };

                if (r.LatestBabyBirthDate.HasValue)
                {
                    dto.ParentStatus = "PARENT";
                    dto.BabyAgeMonths = CalculateBabyAgeInMonths(r.LatestBabyBirthDate.Value);
                    return dto;
                }

                if (r.LatestPregnancyDueDate.HasValue)
                {
                    dto.ParentStatus = "PREGNANT";
                    dto.PregnancyTrimester = CalculatePregnancyTrimester(r.LatestPregnancyDueDate.Value);
                    return dto;
                }

                return dto;
            }).ToList();
        }

        public AppUserResultDto? GetById(long id)
        {
            var nowUtc = DateTime.UtcNow;

            var row = _db.AppUsers.AsNoTracking()
                .Where(u => u.Id == id)
                .Select(x => new AppUserRow
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    RoleId = x.RoleId,
                    PhoneNumber = x.PhoneNumber,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender,
                    ParentProfileId = x.ParentProfile != null ? x.ParentProfile.Id : null,
                    IdentityUserId = x.IdentityUserId,

                    LatestBabyBirthDate = x.ParentProfile != null
                        ? x.ParentProfile.Babies
                            .OrderByDescending(b => b.BirthDate)
                            .Select(b => (DateTime?)b.BirthDate)
                            .FirstOrDefault()
                        : null,

                    LatestPregnancyDueDate = x.ParentProfile != null
                        ? x.ParentProfile.Pregnancies
                            .Where(p => p.DueDate != null && p.DueDate > nowUtc)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p => p.DueDate)
                            .FirstOrDefault()
                        : null
                })
                .FirstOrDefault();

            if (row == null)
            {
                return null;
            }

            var dto = new AppUserResultDto
            {
                Id = row.Id,
                Email = row.Email,
                FirstName = row.FirstName,
                LastName = row.LastName,
                RoleId = row.RoleId,
                PhoneNumber = row.PhoneNumber,
                DateOfBirth = row.DateOfBirth,
                Gender = row.Gender,
                IdentityUserId = row.IdentityUserId,
                ParentStatus = "UNKNOWN",
                BabyAgeMonths = null,
                ParentProfileId = row.ParentProfileId,
                PregnancyTrimester = null
            };

            if (row.LatestBabyBirthDate.HasValue)
            {
                dto.ParentStatus = "PARENT";
                dto.BabyAgeMonths = CalculateBabyAgeInMonths(row.LatestBabyBirthDate.Value);
                return dto;
            }

            if (row.LatestPregnancyDueDate.HasValue)
            {
                dto.ParentStatus = "PREGNANT";
                dto.PregnancyTrimester = CalculatePregnancyTrimester(row.LatestPregnancyDueDate.Value);
                return dto;
            }

            return dto;
        }

        private static (DateTime? Lmp, DateTime? Due) NormalizePregnancyDates(DateTime? lmp, DateTime? due)
        {
            if (lmp.HasValue && !due.HasValue)
            {
                return (lmp, lmp.Value.AddDays(280));
            }

            if (!lmp.HasValue && due.HasValue)
            {
                return (due.Value.AddDays(280 * -1), due);
            }

            return (lmp, due);
        }

        public AppUserResultDto Create(CreateAppUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                throw new ArgumentException("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Password is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                throw new ArgumentException("FirstName is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                throw new ArgumentException("LastName is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                throw new ArgumentException("PhoneNumber is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new ArgumentException("Gender is required.");
            }

            if (_db.AppUsers.Any(u => u.Email == dto.Email))
            {
                throw new ArgumentException("Email already exists.");
            }

            var role = _db.Roles.FirstOrDefault(r => r.Id == dto.RoleId)
                ?? throw new ArgumentException("Role not found.");

            if (!_roleManager.RoleExistsAsync(role.Name!).GetAwaiter().GetResult())
            {
                throw new ArgumentException($"Identity role '{role.Name}' does not exist.");
            }

            var identityUser = new IdentityUser
            {
                UserName = dto.Username.Trim(),
                Email = dto.Email.Trim(),
                EmailConfirmed = true
            };

            var identityResult = _userManager.CreateAsync(identityUser, dto.Password)
                .GetAwaiter().GetResult();

            if (!identityResult.Succeeded)
            {
                var msg = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Identity creation failed: {msg}");
            }

            var roleResult = _userManager.AddToRoleAsync(identityUser, role.Name!)
                .GetAwaiter().GetResult();

            if (!roleResult.Succeeded)
            {
                var msg = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Role assignment failed: {msg}");
            }

            long? parentProfileId = null;

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

                if (role.Id == 1)
                {
                    var parentProfile = new ParentProfile
                    {
                        UserId = user.Id
                    };

                    _db.ParentProfiles.Add(parentProfile);
                    _db.SaveChanges();

                    parentProfileId = parentProfile.Id;

                    var (normLmp, normDue) = NormalizePregnancyDates(dto.LmpDate, dto.DueDate);

                    if (normLmp.HasValue || normDue.HasValue)
                    {
                        var pregnancy = new Pregnancy
                        {
                            ParentProfileId = parentProfile.Id,
                            LmpDate = normLmp,
                            DueDate = normDue,
                            CycleLengthDays = dto.CycleLengthDays
                        };

                        _db.Pregnancies.Add(pregnancy);
                        _db.SaveChanges();
                    }
                }

                if (role.Id == 2)
                {
                    var doctorProfile = new DoctorProfile
                    {
                        UserId = user.Id
                    };

                    _db.DoctorProfiles.Add(doctorProfile);
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
                    IdentityUserId = user.IdentityUserId,
                    ParentStatus = role.Id == 1 ? "PARENT" : "UNKNOWN",
                    BabyAgeMonths = null,
                    PregnancyTrimester = null,
                    ParentProfileId = parentProfileId
                };
            }
            catch
            {
                tx.Rollback();
                _userManager.DeleteAsync(identityUser).GetAwaiter().GetResult();
                throw;
            }
        }

        public AppUserResultDto? Patch(long id, AppUserPatchDto patch)
        {
            var u = _db.AppUsers
                .Include(x => x.ParentProfile)
                .ThenInclude(p => p.Babies)
                .Include(x => x.ParentProfile)
                .ThenInclude(p => p.Pregnancies)
                .FirstOrDefault(x => x.Id == id);

            if (u is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(patch.FirstName))
            {
                u.FirstName = patch.FirstName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(patch.LastName))
            {
                u.LastName = patch.LastName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(patch.PhoneNumber))
            {
                u.PhoneNumber = patch.PhoneNumber.Trim();
            }

            if (patch.DateOfBirth is not null)
            {
                u.DateOfBirth = patch.DateOfBirth.Value;
            }

            if (!string.IsNullOrWhiteSpace(patch.Gender))
            {
                u.Gender = patch.Gender.Trim();
            }

            _db.SaveChanges();

            return MapToResultDto(u);
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

        private static int CalculateBabyAgeInMonths(DateTime birthDate)
        {
            var now = DateTime.UtcNow;
            return (now.Year - birthDate.Year) * 12 + now.Month - birthDate.Month;
        }

        private static int CalculatePregnancyTrimester(DateTime dueDate)
        {
            const int totalWeeks = 40;
            var weeksLeft = (dueDate - DateTime.UtcNow).Days / 7;
            var currentWeek = totalWeeks - weeksLeft;

            if (currentWeek <= 13)
            {
                return 1;
            }

            if (currentWeek <= 27)
            {
                return 2;
            }

            return 3;
        }

        private sealed class AppUserRow
        {
            public long Id { get; set; }
            public string Email { get; set; } = default!;
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? PhoneNumber { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? Gender { get; set; }
            public long? RoleId { get; set; }
            public string IdentityUserId { get; set; } = default!;
            public long? ParentProfileId { get; set; }
            public DateTime? LatestBabyBirthDate { get; set; }
            public DateTime? LatestPregnancyDueDate { get; set; }
        }


        private AppUserResultDto MapToResultDto(AppUser u)
        {
            var dto = new AppUserResultDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                RoleId = u.RoleId,
                IdentityUserId = u.IdentityUserId,
                ParentStatus = "UNKNOWN",
                BabyAgeMonths = null,
                PregnancyTrimester = null
            };

            if (u.ParentProfile != null)
            {
                var latestBaby = u.ParentProfile.Babies?
                    .OrderByDescending(b => b.BirthDate)
                    .FirstOrDefault();

                if (latestBaby != null)
                {
                    dto.ParentStatus = "PARENT";
                    dto.BabyAgeMonths = CalculateBabyAgeInMonths(latestBaby.BirthDate);
                    return dto;
                }

                var latestPregnancy = u.ParentProfile.Pregnancies?
                    .Where(p => p.DueDate != null && p.DueDate > DateTime.UtcNow)
                    .OrderByDescending(p => p.DueDate)
                    .FirstOrDefault();

                if (latestPregnancy?.DueDate != null)
                {
                    dto.ParentStatus = "PREGNANT";
                    dto.PregnancyTrimester = CalculatePregnancyTrimester(latestPregnancy.DueDate.Value);
                }
            }

            return dto;
        }

    }



}
