using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
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

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _db.Notifications
                .FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

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
        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _db.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _db.SaveChangesAsync();
        }
        private static NotificationDto MapToDto(Notification n)
        {
            return new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            };
        }
    }
}
