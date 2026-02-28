using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;

namespace Nestly.Services.Messaging
{
    public class MedicationReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MedicationReminderService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckMedicationReminders(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CheckMedicationReminders(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var now = DateTime.UtcNow;

            var logs = await db.MedicationIntakeLogs
    .Include(l => l.Plan)
        .ThenInclude(p => p.ParentProfile)
    .Where(l =>
        !l.Taken &&
        !l.ReminderSent &&
        l.ScheduledDate == now.Date)
    .ToListAsync(ct);

            foreach (var log in logs)
            {
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
