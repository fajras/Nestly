using Microsoft.AspNetCore.SignalR;

namespace Nestly.Services.Messaging
{
    public class NotificationHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        }
    }
}
