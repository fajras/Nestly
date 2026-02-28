using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
namespace Nestly.Services.Repository
{
    public class NotificationService : INotificationService
    {
        private readonly NestlyDbContext _db;

        public NotificationService(NestlyDbContext db)
        {
            _db = db;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _db.Notifications.FindAsync(notificationId);

            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<int> GetUnreadCountAsync(long userId)
        {
            return await _db.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}
