using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.WebAPI.Hubs
{
    public class SignalRNotificationNotifier : INotificationNotifier
    {
        private const int MaxAttempts = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(300);

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<SignalRNotificationNotifier> _logger;
        private readonly IUserConnectionTracker _connectionTracker;

        public SignalRNotificationNotifier(
            IHubContext<NotificationHub> hubContext,
            ILogger<SignalRNotificationNotifier> logger,
            IUserConnectionTracker connectionTracker)
        {
            _hubContext = hubContext;
            _logger = logger;
            _connectionTracker = connectionTracker;
        }

        public async Task NotifyUser(long userId, NotificationDto notification)
        {
            if (!_connectionTracker.IsOnline(userId))
            {
                // Notification is already persisted; the user will see it
                // next time they open the notifications list or poll for
                // unread count, so there's nothing to retry here.
                _logger.LogInformation(
                    "Skipping real-time notification push to user {UserId}: no active connection.",
                    userId);
                return;
            }

            for (var attempt = 1; attempt <= MaxAttempts; attempt++)
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
                    return;
                }
                catch (Exception ex) when (attempt < MaxAttempts)
                {
                    _logger.LogWarning(
                        ex,
                        "Attempt {Attempt}/{MaxAttempts} to push real-time notification to user {UserId} failed, retrying.",
                        attempt, MaxAttempts, userId);

                    await Task.Delay(RetryDelay * attempt);
                }
                catch (Exception ex)
                {
                    // The notification is already saved to the database; a
                    // SignalR outage should not fail the caller. The user will
                    // still see it next time they open the notifications list.
                    _logger.LogError(
                        ex,
                        "Failed to push real-time notification to user {UserId} after {MaxAttempts} attempts.",
                        userId, MaxAttempts);
                }
            }
        }
    }
}
