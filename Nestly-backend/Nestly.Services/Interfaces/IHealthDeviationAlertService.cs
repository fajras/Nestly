using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IHealthDeviationAlertService
    {
        Task<PagedResult<HealthDeviationAlertResponseDto>> GetByParent(
            long parentProfileId, HealthDeviationAlertSearchObject search);

        Task<HealthDeviationAlertResponseDto> Resolve(long id);

        // Doctor-facing review queue: any doctor can review any generated
        // alert and record whether it was clinically accurate, the same way
        // any doctor can answer any Q&A question in this app - there's no
        // per-patient doctor assignment model to restrict it further.
        Task<PagedResult<HealthDeviationAlertResponseDto>> GetForDoctorReview(
            HealthDeviationAlertSearchObject search);

        Task<HealthDeviationAlertResponseDto> SubmitDoctorFeedback(
            long id, long doctorProfileId, HealthDeviationAlertFeedbackDto feedback);
    }
}
