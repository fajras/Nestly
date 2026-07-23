using System.Collections.Generic;
using System.Threading.Tasks;
using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IBabyHealthMonitoringService
    {
        /// <summary>
        /// Runs all WHO-standards evaluators for one baby, persists any new
        /// deviation alerts and pushes a notification for each one. Returns
        /// only the newly created alerts (already-active ones are skipped).
        /// </summary>
        Task<List<HealthDeviationAlertResponseDto>> RunCheckForBabyAsync(long babyId);

        /// <summary>
        /// Runs <see cref="RunCheckForBabyAsync"/> for every tracked baby
        /// (0-24 months old). Intended for the daily background job.
        /// </summary>
        Task RunDailyCheckAsync();
    }
}
