using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;
namespace Nestly.WebAPI.Hubs

{
    public class SignalRChatNotifier : IChatNotifier
    {
        private const int MaxAttempts = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(300);

        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<SignalRChatNotifier> _logger;
        private readonly IUserConnectionTracker _connectionTracker;

        public SignalRChatNotifier(
            IHubContext<ChatHub> hubContext,
            ILogger<SignalRChatNotifier> logger,
            IUserConnectionTracker connectionTracker)
        {
            _hubContext = hubContext;
            _logger = logger;
            _connectionTracker = connectionTracker;
        }

        public async Task NotifyUser(
            long userId,
            ChatMessageRealtimeDto message)
        {
            if (!_connectionTracker.IsOnline(userId))
            {
                // Nobody is connected for this user right now - the message
                // is already persisted, so it'll simply show up next time
                // they open the chat. No point sending to an empty group.
                _logger.LogInformation(
                    "Skipping real-time chat push to user {UserId}: no active connection.",
                    userId);
                return;
            }

            for (var attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                try
                {
                    await _hubContext.Clients
                        .Group($"user-{userId}")
                        .SendAsync(
                            "ReceiveMessage",
                            message);
                    return;
                }
                catch (Exception ex) when (attempt < MaxAttempts)
                {
                    _logger.LogWarning(
                        ex,
                        "Attempt {Attempt}/{MaxAttempts} to push real-time chat message to user {UserId} failed, retrying.",
                        attempt, MaxAttempts, userId);

                    await Task.Delay(RetryDelay * attempt);
                }
                catch (Exception ex)
                {
                    // Real-time delivery is best-effort: the message is already
                    // persisted, so a SignalR outage must not fail the send.
                    _logger.LogError(
                        ex,
                        "Failed to push real-time chat message to user {UserId} after {MaxAttempts} attempts.",
                        userId, MaxAttempts);
                }
            }
        }
    }
}
