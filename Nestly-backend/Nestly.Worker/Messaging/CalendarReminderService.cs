using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
using Nestly.Services.Messaging;

namespace Nestly.Worker.Messaging
{
    public class CalendarReminderService : BackgroundService
    {
        private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan ReminderWindow = TimeSpan.FromHours(24);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CalendarReminderService> _logger;

        public CalendarReminderService(
            IServiceScopeFactory scopeFactory,
            ILogger<CalendarReminderService> logger)
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
                    await CheckUpcomingEvents(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CalendarReminderService error.");
                }

                try
                {
                    await Task.Delay(CheckInterval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        // Runs every 15 minutes and reminds about any event starting within
        // the next 24 hours that hasn't been reminded about yet - this gives
        // an accurate "~24h before" notification regardless of the event's
        // time of day, unlike a once-a-day check that only compares dates.
        private async Task CheckUpcomingEvents(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var now = DateTime.UtcNow;
            var horizon = now.Add(ReminderWindow);

            var events = await db.CalendarEvents
                .Where(e =>
                    !e.Reminder24hSent &&
                    e.StartAt > now &&
                    e.StartAt <= horizon)
                .ToListAsync(ct);

            foreach (var ev in events)
            {
                if (ev.UserId.HasValue)
                {
                    publisher.Publish(new NotificationEvent
                    {
                        UserId = ev.UserId.Value,
                        Title = "Podsjetnik za termin",
                        Message = $"Uskoro imate zakazan termin: {ev.Title}"
                    });

                    ev.Reminder24hSent = true;
                }
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
