using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(long userId);
        Task MarkAsReadAsync(int notificationId, long userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task MarkAllAsReadAsync(long userId);
        Task CreateNotificationAsync(NotificationEvent notificationEvent);
    }
}
