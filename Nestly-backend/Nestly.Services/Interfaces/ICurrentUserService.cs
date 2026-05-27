using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface ICurrentUserService
    {
        long GetCurrentAppUserId();

        Task<ParentProfile> GetCurrentParentProfileAsync();

        Task<DoctorProfile> GetCurrentDoctorProfileAsync();

        Task<bool> IsDoctorAsync();

        Task EnsureParentOwnershipAsync(long parentProfileId);

        Task EnsureBabyOwnershipAsync(long babyId);

        Task EnsurePregnancyOwnershipAsync(long pregnancyId);

        Task EnsureCalendarEventOwnershipAsync(long calendarEventId);

        Task EnsureQaQuestionOwnershipAsync(long questionId);

        Task EnsureSymptomDiaryOwnershipAsync(long diaryId);

        Task EnsureMedicationPlanOwnershipAsync(long medicationPlanId);

        Task EnsureHealthEntryOwnershipAsync(long healthEntryId);
    }
}
