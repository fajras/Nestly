using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task MarkAllAsReadAsync(int userId);
    }
}
