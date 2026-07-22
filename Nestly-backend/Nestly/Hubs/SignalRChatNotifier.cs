using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
namespace Nestly.WebAPI.Hubs

{
    public class SignalRChatNotifier : IChatNotifier
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<SignalRChatNotifier> _logger;

        public SignalRChatNotifier(
            IHubContext<ChatHub> hubContext,
            ILogger<SignalRChatNotifier> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyUser(
            long userId,
            ChatMessageRealtimeDto message)
        {
            try
            {
                await _hubContext.Clients
                    .Group($"user-{userId}")
                    .SendAsync(
                        "ReceiveMessage",
                        message);
            }
            catch (Exception ex)
            {
                // Real-time delivery is best-effort: the message is already
                // persisted, so a SignalR outage must not fail the send.
                _logger.LogError(
                    ex,
                    "Failed to push real-time chat message to user {UserId}.",
                    userId);
            }
        }
    }
}
