using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.WebAPI.Hubs
{
    public class SignalRNotificationNotifier : INotificationNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<SignalRNotificationNotifier> _logger;

        public SignalRNotificationNotifier(
            IHubContext<NotificationHub> hubContext,
            ILogger<SignalRNotificationNotifier> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyUser(long userId, NotificationDto notification)
        {
            try
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
            catch (Exception ex)
            {
                // The notification is already saved to the database; a
                // SignalR outage should not fail the caller. The user will
                // still see it next time they open the notifications list.
                _logger.LogError(
                    ex,
                    "Failed to push real-time notification to user {UserId}.",
                    userId);
            }
        }
    }
}
