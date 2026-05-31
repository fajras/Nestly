using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ICurrentUserService
            _currentUserService;

        public ChatHub(
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
                GetUserGroup(userId));

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
                GetUserGroup(userId));

            await base.OnDisconnectedAsync(
                exception);
        }

        private static string GetUserGroup(
            long userId)
                => $"user-{userId}";
    }
}
