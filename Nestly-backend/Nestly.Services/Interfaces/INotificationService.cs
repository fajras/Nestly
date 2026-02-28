using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<int> GetUnreadCountAsync(long userId);
    }
}
