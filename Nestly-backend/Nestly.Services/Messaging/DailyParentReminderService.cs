using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
namespace Nestly.Services.Messaging
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
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await SendDailyReminder(stoppingToken);
            }
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        var now = DateTime.UtcNow;

        //        var nextRun = now.Date.AddHours(12);

        //        if (now >= nextRun)
        //        {
        //            nextRun = nextRun.AddDays(1);
        //        }

        //        var delay = nextRun - now;

        //        await Task.Delay(delay, stoppingToken);

        //        await SendDailyReminder(stoppingToken);
        //    }
        //}

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
