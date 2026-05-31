using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Messaging
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ICurrentUserService
            _currentUserService;

        public NotificationHub(
            ICurrentUserService currentUserService)
        {
            _currentUserService =
                currentUserService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId =
                _currentUserService
                    .GetCurrentAppUserId(
                        Context.User);

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"user-{userId}");

            await base.OnConnectedAsync();
        }

        public override async Task
            OnDisconnectedAsync(
                Exception? exception)
        {
            var userId =
                _currentUserService
                    .GetCurrentAppUserId(
                        Context.User);
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                $"user-{userId}");

            await base.OnDisconnectedAsync(
                exception);
        }
    }
}
