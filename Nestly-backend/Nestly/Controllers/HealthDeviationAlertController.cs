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
    }
}
