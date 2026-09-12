using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.WebAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ICurrentUserService
            _currentUserService;
        private readonly ILogger<ChatHub> _logger;
        private readonly IUserConnectionTracker _connectionTracker;

        public ChatHub(
            ICurrentUserService currentUserService,
            ILogger<ChatHub> logger,
            IUserConnectionTracker connectionTracker)
        {
            _currentUserService =
                currentUserService;
            _logger = logger;
            _connectionTracker = connectionTracker;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId =
                    _currentUserService
                        .GetCurrentAppUserId(
                            Context.User);
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    GetUserGroup(userId));

                _connectionTracker.AddConnection(userId, Context.ConnectionId);

                _logger.LogInformation(
                    "User {UserId} connected to ChatHub (connection {ConnectionId}). Online users: {OnlineCount}.",
                    userId, Context.ConnectionId, _connectionTracker.OnlineUserCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while connecting to ChatHub (connection {ConnectionId}).",
                    Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task
            OnDisconnectedAsync(
                Exception? exception)
        {
            try
            {
                var userId =
                    _currentUserService
                        .GetCurrentAppUserId(
                            Context.User);
                await Groups.RemoveFromGroupAsync(
                    Context.ConnectionId,
                    GetUserGroup(userId));

                _connectionTracker.RemoveConnection(userId, Context.ConnectionId);

                _logger.LogInformation(
                    "User {UserId} disconnected from ChatHub (connection {ConnectionId}). Online users: {OnlineCount}.",
                    userId, Context.ConnectionId, _connectionTracker.OnlineUserCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while disconnecting from ChatHub (connection {ConnectionId}).",
                    Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(
                exception);
        }

        private static string GetUserGroup(
            long userId)
                => $"user-{userId}";
    }
}
