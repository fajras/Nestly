using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/health-alerts")]
    [Authorize]
    public class HealthDeviationAlertController : ControllerBase
    {
        private readonly IHealthDeviationAlertService _service;
        private readonly IBabyHealthMonitoringService _monitoringService;
        private readonly ICurrentUserService _currentUserService;

        public HealthDeviationAlertController(
            IHealthDeviationAlertService service,
            IBabyHealthMonitoringService monitoringService,
            ICurrentUserService currentUserService)
        {
            _service = service;
            _monitoringService = monitoringService;
            _currentUserService = currentUserService;
        }

        [Authorize(Roles = "Parent")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<HealthDeviationAlertResponseDto>>> Get(
            [FromQuery] HealthDeviationAlertSearchObject search)
        {
            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            if (search.BabyId.HasValue)
            {
                await _currentUserService
                    .EnsureBabyOwnershipAsync(
                        search.BabyId.Value);
            }

            return Ok(
                await _service.GetByParent(
                    parent.Id,
                    search));
        }

        [Authorize(Roles = "Parent")]
        [HttpPost("{id:long}/resolve")]
        public async Task<ActionResult<HealthDeviationAlertResponseDto>> Resolve(long id)
        {
            await _currentUserService
                .EnsureHealthDeviationAlertOwnershipAsync(id);

            return Ok(await _service.Resolve(id));
        }

        // On-demand trigger so the check can be demonstrated/tested without
        // waiting for the daily background job.
        [Authorize(Roles = "Parent")]
        [HttpPost("run/{babyId:long}")]
        public async Task<IActionResult> Run(long babyId)
        {
            await _currentUserService
                .EnsureBabyOwnershipAsync(babyId);

            var created = await _monitoringService.RunCheckForBabyAsync(babyId);

            return Ok(created);
        }

        // Doctor review queue: any doctor can browse generated alerts and
        // confirm/dispute their accuracy, the same way any doctor can
        // answer any parent's Q&A question elsewhere in this app.
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public async Task<ActionResult<PagedResult<HealthDeviationAlertResponseDto>>> GetForDoctorReview(
            [FromQuery] HealthDeviationAlertSearchObject search)
        {
            return Ok(await _service.GetForDoctorReview(search));
        }

        // Human-in-the-loop feedback: records whether a doctor found this
        // ML-generated alert clinically accurate, as a documented basis for
        // future evaluation/retraining of the deviation-detection model.
        [Authorize(Roles = "Doctor")]
        [HttpPost("{id:long}/feedback")]
        public async Task<ActionResult<HealthDeviationAlertResponseDto>> SubmitDoctorFeedback(
            long id, [FromBody] HealthDeviationAlertFeedbackDto feedback)
        {
            var doctor = await _currentUserService.GetCurrentDoctorProfileAsync();

            return Ok(await _service.SubmitDoctorFeedback(id, doctor.Id, feedback));
        }
    }
}
