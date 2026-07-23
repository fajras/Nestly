using Microsoft.Extensions.Logging;
using Nestly.Services.Interfaces;

namespace Nestly.Worker.Messaging
{
    // Daily check: compares each tracked baby's recent logs (growth,
    // feeding, sleep, diapers, fever) against WHO/pediatric reference
    // standards and notifies the parent when a real multi-day deviation
    // pattern is found. See Nestly.Services.Repository.BabyHealthMonitoringService.
    public class HealthDeviationMonitoringService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<HealthDeviationMonitoringService> _logger;

        public HealthDeviationMonitoringService(
            IServiceScopeFactory scopeFactory,
            ILogger<HealthDeviationMonitoringService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var nextRun = now.Date.AddHours(13);

                    if (now >= nextRun)
                    {
                        nextRun = nextRun.AddDays(1);
                    }

                    var delay = nextRun - now;

                    await Task.Delay(delay, stoppingToken);

                    await RunCheck(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "HealthDeviationMonitoringService error.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task RunCheck(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();

            var monitoringService = scope.ServiceProvider
                .GetRequiredService<IBabyHealthMonitoringService>();

            await monitoringService.RunDailyCheckAsync();
        }
    }
}
