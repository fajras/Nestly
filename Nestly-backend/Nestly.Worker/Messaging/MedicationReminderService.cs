using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
using Nestly.Services.Messaging;

namespace Nestly.Worker.Messaging
{
    public class MedicationReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MedicationReminderService> _logger;

        public MedicationReminderService(
            IServiceScopeFactory scopeFactory,
            ILogger<MedicationReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckMedicationReminders(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "MedicationReminderService error.");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        private async Task CheckMedicationReminders(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var now = DateTime.Now;

            var logs = await db.MedicationIntakeLogs
                .Include(l => l.Plan)
                    .ThenInclude(p => p.ParentProfile)
                .Where(l =>
                    !l.Taken &&
                    !l.ReminderSent &&
                    l.ScheduledDate >= now.Date &&
                    l.ScheduledDate < now.Date.AddDays(1))
                .ToListAsync(ct);

            foreach (var log in logs)
            {
                if (log.Plan?.ParentProfile == null)
                {
                    continue;
                }

                var intakeDateTime = log.ScheduledDate
                    .Add(log.IntakeTime);

                var reminderTime = intakeDateTime.AddHours(-1);

                if (now >= reminderTime && now < intakeDateTime)
                {
                    publisher.Publish(new NotificationEvent
                    {
                        UserId = log.Plan.ParentProfile.UserId,
                        Title = "Podsjetnik za terapiju",
                        Message = $"Za 1 sat trebate uzeti: {log.Plan.MedicineName} ({log.Plan.Dose})"
                    });

                    log.ReminderSent = true;
                }
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
