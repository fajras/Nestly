using Microsoft.AspNetCore.SignalR;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.WebAPI.Hubs
{
    public class SignalRNotificationNotifier : INotificationNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUser(long userId, NotificationDto notification)
        {
            await _hubContext.Clients
                .Group($"user-{userId}")
                .SendAsync("ReceiveNotification", new
                {
                    id = notification.Id,
                    title = notification.Title,
                    message = notification.Message,
                    isRead = notification.IsRead,
                    createdAt = notification.CreatedAt
                });
        }
    }
}
