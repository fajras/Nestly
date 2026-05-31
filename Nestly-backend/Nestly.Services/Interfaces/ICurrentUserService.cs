using Nestly.Model.Entity;
using System.Security.Claims;

namespace Nestly.Services.Interfaces
{
    public interface ICurrentUserService
    {
        long GetCurrentAppUserId();

        ParentProfile GetCurrentParentProfile();

        Task<ParentProfile> GetCurrentParentProfileAsync();

        Task<DoctorProfile> GetCurrentDoctorProfileAsync();

        Task<bool> IsDoctorAsync();

        Task EnsureParentOwnershipAsync(long parentProfileId);

        Task EnsureBabyOwnershipAsync(long babyId);

        Task EnsurePregnancyOwnershipAsync(long pregnancyId);

        Task EnsureCalendarEventOwnershipAsync(long calendarEventId);

        Task EnsureQaQuestionOwnershipAsync(long questionId);

        Task EnsureSymptomDiaryOwnershipAsync(long symptomDiaryId);

        Task EnsureMedicationPlanOwnershipAsync(long medicationPlanId);

        Task EnsureMedicationIntakeOwnershipAsync(long intakeLogId);

        Task EnsureFeedingLogOwnershipAsync(long feedingLogId);

        Task EnsureSleepLogOwnershipAsync(long sleepLogId);

        Task EnsureDiaperLogOwnershipAsync(long diaperLogId);

        Task EnsureGrowthRecordOwnershipAsync(long growthRecordId);

        Task EnsureHealthRecordOwnershipAsync(long healthRecordId);

        Task EnsureMilestoneOwnershipAsync(long milestoneId);

        Task EnsureMealPlanOwnershipAsync(long mealPlanId);

        Task EnsureHealthEntryOwnershipAsync(long healthEntryId);

        Task EnsureCanChatWithUserAsync(long otherUserId);

        Task EnsureBabyGrowthOwnershipAsync(long growthId);

        Task EnsureAppUserOwnershipAsync(long appUserId);

        Task EnsureConversationOwnershipAsync(long conversationId);
        long GetCurrentAppUserId(ClaimsPrincipal user);

    }
}