using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;
namespace Nestly.Services.Repository
{
    public class NotificationService : INotificationService
    {
        private readonly NestlyDbContext _db;
        private readonly INotificationNotifier? _notifier;
        public NotificationService(
    NestlyDbContext db,
    INotificationNotifier? notifier = null)
        {
            _db = db;
            _notifier = notifier;
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(long userId)
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

        public async Task MarkAsReadAsync(int notificationId, long userId)
        {
            var notification = await _db.Notifications
                .FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);

            if (notification == null)
            {
                throw new NotFoundException("Notification not found.");
            }

            notification.IsRead = true;

            await _db.SaveChangesAsync();
        }
        public async Task<int> GetUnreadCountAsync(long userId)
        {
            return await _db.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
        public async Task MarkAllAsReadAsync(long userId)
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
        public async Task CreateNotificationAsync(NotificationEvent notificationEvent)
        {
            var notification = new Notification
            {
                UserId = notificationEvent.UserId,
                Title = notificationEvent.Title,
                Message = notificationEvent.Message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();
            var dto = new NotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };

            if (_notifier != null)
            {
                await _notifier.NotifyUser(notification.UserId, dto);
            }
        }
    }
}
