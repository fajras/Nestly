using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
using Nestly.Services.Messaging;
namespace Nestly.Worker.Messaging
{
    public class DailyParentReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DailyParentReminderService(IServiceScopeFactory scopeFactory)
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

                    await SendDailyReminder(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DailyParentReminderService error: {ex.Message}");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task SendDailyReminder(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NestlyDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var parents = await db.ParentProfiles
               .Include(p => p.User)
               .Where(p => p.User.RoleId == 1)
               .AsNoTracking()
               .ToListAsync(ct);

            foreach (var parent in parents)
            {
                publisher.Publish(new NotificationEvent
                {
                    UserId = parent.UserId,
                    Title = "Dnevni podsjetnik",
                    Message = "Ne zaboravite unijeti današnje parametre."
                });
            }
        }
    }
}
