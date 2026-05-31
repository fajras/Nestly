using Microsoft.AspNetCore.SignalR;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;
namespace Nestly.WebAPI.Hubs

{
    public class SignalRChatNotifier : IChatNotifier
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRChatNotifier(
            IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUser(
            long userId,
            ChatMessageRealtimeDto message)
        {
            await _hubContext.Clients
                .Group($"user-{userId}")
                .SendAsync(
                    "ReceiveMessage",
                    message);
        }
    }
}

