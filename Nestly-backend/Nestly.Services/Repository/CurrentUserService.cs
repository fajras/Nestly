using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NestlyDbContext _db;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            NestlyDbContext db)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public long GetCurrentAppUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("userId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            return long.Parse(userIdClaim);
        }

        public async Task<ParentProfile> GetCurrentParentProfileAsync()
        {
            var appUserId = GetCurrentAppUserId();

            var parent = await _db.ParentProfiles
                .FirstOrDefaultAsync(x => x.AppUserId == appUserId);

            if (parent == null)
            {
                throw new UnauthorizedAccessException("Parent profile not found.");
            }

            return parent;
        }

        public async Task<DoctorProfile> GetCurrentDoctorProfileAsync()
        {
            var appUserId = GetCurrentAppUserId();

            var doctor = await _db.DoctorProfiles
                .FirstOrDefaultAsync(x => x.AppUserId == appUserId);

            if (doctor == null)
            {
                throw new UnauthorizedAccessException("Doctor profile not found.");
            }

            return doctor;
        }

        public async Task<bool> IsDoctorAsync()
        {
            return _httpContextAccessor.HttpContext!
                .User
                .IsInRole("Doctor");
        }

        public async Task EnsureParentOwnershipAsync(long parentProfileId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent = await GetCurrentParentProfileAsync();

            if (currentParent.Id != parentProfileId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this parent profile.");
            }
        }

        public async Task EnsureBabyOwnershipAsync(long babyId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent = await GetCurrentParentProfileAsync();

            var ownsBaby = await _db.BabyProfiles
                .AnyAsync(x =>
                    x.Id == babyId &&
                    x.ParentProfileId == currentParent.Id);

            if (!ownsBaby)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this baby profile.");
            }
        }

        public async Task EnsurePregnancyOwnershipAsync(long pregnancyId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent = await GetCurrentParentProfileAsync();

            var ownsPregnancy = await _db.Pregnancies
                .AnyAsync(x =>
                    x.Id == pregnancyId &&
                    x.ParentProfileId == currentParent.Id);

            if (!ownsPregnancy)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this pregnancy.");
            }
        }

        public async Task EnsureCalendarEventOwnershipAsync(long calendarEventId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent = await GetCurrentParentProfileAsync();

            var ownsEvent = await _db.CalendarEvents
                .AnyAsync(x =>
                    x.Id == calendarEventId &&
                    x.ParentProfileId == currentParent.Id);

            if (!ownsEvent)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this calendar event.");
            }
        }

        public async Task EnsureQaQuestionOwnershipAsync(long questionId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent = await GetCurrentParentProfileAsync();

            var ownsQuestion = await _db.QaQuestions
                .AnyAsync(x =>
                    x.Id == questionId &&
                    x.AskedById == currentParent.Id);

            if (!ownsQuestion)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this question.");
            }
        }
    }
}
