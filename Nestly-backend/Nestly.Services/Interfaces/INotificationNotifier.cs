using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface INotificationNotifier
    {
        Task NotifyUser(long userId, NotificationDto notification);
    }
}
