using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;
using System.Security.Claims;
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
            var userIdClaim = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("userId")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                throw new UnauthorizedAccessException(
                    "User not authenticated.");
            }

            return long.Parse(userIdClaim);
        }

        public ParentProfile GetCurrentParentProfile()
        {
            var appUserId = GetCurrentAppUserId();

            var parent = _db.ParentProfiles
                .FirstOrDefault(x =>
                    x.UserId == appUserId);

            if (parent == null)
            {
                throw new UnauthorizedAccessException(
                    "Parent profile not found.");
            }

            return parent;
        }

        public async Task<ParentProfile>
            GetCurrentParentProfileAsync()
        {
            var appUserId = GetCurrentAppUserId();

            var parent = await _db.ParentProfiles
                .FirstOrDefaultAsync(x =>
                    x.UserId == appUserId);

            if (parent == null)
            {
                throw new UnauthorizedAccessException(
                    "Parent profile not found.");
            }

            return parent;
        }

        public async Task<DoctorProfile>
            GetCurrentDoctorProfileAsync()
        {
            var appUserId = GetCurrentAppUserId();

            var doctor = await _db.DoctorProfiles
                .FirstOrDefaultAsync(x =>
                    x.UserId == appUserId);

            if (doctor == null)
            {
                throw new UnauthorizedAccessException(
                    "Doctor profile not found.");
            }

            return doctor;
        }

        public async Task<bool> IsDoctorAsync()
        {
            return _httpContextAccessor
                .HttpContext!
                .User
                .IsInRole("Doctor");
        }

        public async Task EnsureParentOwnershipAsync(
            long parentProfileId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            if (currentParent.Id != parentProfileId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this parent profile.");
            }
        }

        public async Task EnsureBabyOwnershipAsync(
            long babyId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsBaby = await _db.BabyProfiles
                .AnyAsync(x =>
                    x.Id == babyId &&
                    x.ParentProfileId ==
                    currentParent.Id);

            if (!ownsBaby)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this baby profile.");
            }
        }

        public async Task EnsurePregnancyOwnershipAsync(
            long pregnancyId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsPregnancy =
                await _db.Pregnancies
                    .AnyAsync(x =>
                        x.Id == pregnancyId &&
                        x.ParentProfileId ==
                        currentParent.Id);

            if (!ownsPregnancy)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this pregnancy.");
            }
        }

        public async Task EnsureCalendarEventOwnershipAsync(
            long calendarEventId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsEvent =
                await _db.CalendarEvents
                    .AnyAsync(x =>
                        x.Id == calendarEventId &&
                        x.UserId ==
                        currentParent.Id);

            if (!ownsEvent)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this calendar event.");
            }
        }

        public async Task EnsureQaQuestionOwnershipAsync(
            long questionId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsQuestion =
                await _db.QaQuestions
                    .AnyAsync(x =>
                        x.Id == questionId &&
                        x.AskedById ==
                        currentParent.Id);

            if (!ownsQuestion)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this question.");
            }
        }

        public async Task EnsureSymptomDiaryOwnershipAsync(
            long symptomDiaryId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsDiary =
                await _db.SymptomDiaries
                    .AnyAsync(x =>
                        x.Id == symptomDiaryId &&
                        x.ParentProfileId ==
                        currentParent.Id);

            if (!ownsDiary)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this symptom diary.");
            }
        }

        public async Task EnsureMedicationPlanOwnershipAsync(
            long medicationPlanId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsPlan =
                await _db.MedicationPlans
                    .AnyAsync(x =>
                        x.Id == medicationPlanId &&
                        x.ParentProfileId ==
                        currentParent.Id);

            if (!ownsPlan)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this medication plan.");
            }
        }

        public async Task EnsureMedicationIntakeOwnershipAsync(
            long intakeLogId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var parent =
                await GetCurrentParentProfileAsync();

            var exists =
                await _db.MedicationIntakeLogs
                    .AnyAsync(x =>
                        x.Id == intakeLogId &&
                        x.Plan.ParentProfileId ==
                        parent.Id);

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this medication intake log.");
            }
        }

        public async Task EnsureFeedingLogOwnershipAsync(
            long feedingLogId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var parent =
                await GetCurrentParentProfileAsync();

            var exists =
                await _db.FeedingLogs
                    .AnyAsync(x =>
                        x.Id == feedingLogId &&
                        x.Baby.ParentProfileId ==
                        parent.Id);

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this feeding log.");
            }
        }

        public async Task EnsureSleepLogOwnershipAsync(
            long sleepLogId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var parent =
                await GetCurrentParentProfileAsync();

            var exists =
                await _db.SleepLogs
                    .AnyAsync(x =>
                        x.Id == sleepLogId &&
                        x.Baby.ParentProfileId ==
                        parent.Id);

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this sleep log.");
            }
        }

        public async Task EnsureDiaperLogOwnershipAsync(
            long diaperLogId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var parent =
                await GetCurrentParentProfileAsync();

            var exists =
                await _db.DiaperLogs
                    .AnyAsync(x =>
                        x.Id == diaperLogId &&
                        x.Baby.ParentProfileId ==
                        parent.Id);

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this diaper log.");
            }
        }

        public async Task EnsureGrowthRecordOwnershipAsync(
            long growthRecordId)
        {
            await EnsureBabyGrowthOwnershipAsync(
                growthRecordId);
        }

        public async Task EnsureBabyGrowthOwnershipAsync(
            long growthId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsGrowth =
                await _db.BabyGrowths
                    .AnyAsync(x =>
                        x.Id == growthId &&
                        x.Baby.ParentProfileId ==
                        currentParent.Id);

            if (!ownsGrowth)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this baby growth.");
            }
        }

        public async Task EnsureHealthRecordOwnershipAsync(
            long healthRecordId)
        {
            await EnsureHealthEntryOwnershipAsync(
                healthRecordId);
        }

        public async Task EnsureHealthEntryOwnershipAsync(
            long healthEntryId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var parent =
                await GetCurrentParentProfileAsync();

            var exists =
                await _db.HealthEntries
                    .AnyAsync(x =>
                        x.Id == healthEntryId &&
                        x.Baby.ParentProfileId ==
                        parent.Id);

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this health entry.");
            }
        }

        public async Task EnsureMilestoneOwnershipAsync(
            long milestoneId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentParent =
                await GetCurrentParentProfileAsync();

            var ownsMilestone =
                await _db.Milestones
                    .AnyAsync(x =>
                        x.Id == milestoneId &&
                        x.Baby.ParentProfileId ==
                        currentParent.Id);

            if (!ownsMilestone)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this milestone.");
            }
        }

        public async Task EnsureMealPlanOwnershipAsync(
            long mealPlanId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var parent =
                await GetCurrentParentProfileAsync();

            var exists =
                await _db.MealPlans
                    .AnyAsync(x =>
                        x.Id == mealPlanId &&
                        x.Baby.ParentProfileId ==
                        parent.Id);

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this meal plan.");
            }
        }

        public async Task EnsureCanChatWithUserAsync(
            long otherUserId)
        {
            var currentUserId =
                GetCurrentAppUserId();

            if (currentUserId == otherUserId)
            {
                throw new UnauthorizedAccessException(
                    "You cannot chat with yourself.");
            }

            var otherUserExists =
                await _db.AppUsers
                    .AnyAsync(x =>
                        x.Id == otherUserId);

            if (!otherUserExists)
            {
                throw new NotFoundException(
                    "User not found.");
            }
        }

        public async Task EnsureAppUserOwnershipAsync(
            long appUserId)
        {
            if (await IsDoctorAsync())
            {
                return;
            }

            var currentUserId =
                GetCurrentAppUserId();

            if (currentUserId != appUserId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this user.");
            }
        }
        public async Task EnsureConversationOwnershipAsync(
    long conversationId)
        {
            var currentUserId =
                GetCurrentAppUserId();

            var exists =
                await _db.ChatConversations.AnyAsync(
                    x =>
                        x.Id == conversationId &&
                        (
                            x.User1Id == currentUserId ||
                            x.User2Id == currentUserId
                        ));

            if (!exists)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this conversation.");
            }

        }
        public long GetCurrentAppUserId(
            ClaimsPrincipal user)
        {
            var userIdClaim =
                user?
                .FindFirst("userId")
                ?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                throw new UnauthorizedAccessException(
                    "User not authenticated.");
            }

            return long.Parse(userIdClaim);
        }

    }
}