using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
using Nestly.Services.Messaging;

namespace Nestly.Worker.Messaging
{
    public class CalendarReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CalendarReminderService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;

                    var nextRun = now.Date.AddHours(12);

                    if (now >= nextRun)
                    {
                        nextRun = nextRun.AddDays(1);
                    }

                    var delay = nextRun - now;

                    await Task.Delay(delay, stoppingToken);

                    await CheckTomorrowEvents(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CalendarReminderService error: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task CheckTomorrowEvents(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var tomorrow = DateTime.UtcNow.Date.AddDays(1);

            var events = await db.CalendarEvents
                .Where(e =>
                    !e.Reminder24hSent &&
                    e.StartAt.Date == tomorrow)
                .ToListAsync(ct);

            foreach (var ev in events)
            {
                if (ev.UserId.HasValue)
                {
                    publisher.Publish(new NotificationEvent
                    {
                        UserId = ev.UserId.Value,
                        Title = "Podsjetnik za sutrašnji termin",
                        Message = $"Sutra imate zakazan termin: {ev.Title}"
                    });

                    ev.Reminder24hSent = true;
                }
            }

            await db.SaveChangesAsync(ct);
        }
    }
}

