using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;

namespace Nestly.Services.Messaging
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
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                // Sljedeći 12:00 UTC
                var nextRun = now.Date.AddHours(12);

                if (now >= nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;

                await Task.Delay(delay, stoppingToken);

                await CheckTomorrowEvents(stoppingToken);
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

