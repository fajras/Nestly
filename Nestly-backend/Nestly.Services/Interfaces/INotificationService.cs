using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationDto>> GetUserNotificationsAsync(long userId, int page = 1, int pageSize = 50);
        Task MarkAsReadAsync(int notificationId, long userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task MarkAllAsReadAsync(long userId);
        Task CreateNotificationAsync(NotificationEvent notificationEvent);
    }
}
