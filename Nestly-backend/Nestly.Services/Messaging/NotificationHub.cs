using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Messaging
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ICurrentUserService
            _currentUserService;
        private readonly ILogger<NotificationHub> _logger;
        private readonly IUserConnectionTracker _connectionTracker;

        public NotificationHub(
            ICurrentUserService currentUserService,
            ILogger<NotificationHub> logger,
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
                    $"user-{userId}");

                _connectionTracker.AddConnection(userId, Context.ConnectionId);

                _logger.LogInformation(
                    "User {UserId} connected to NotificationHub (connection {ConnectionId}). Online users: {OnlineCount}.",
                    userId, Context.ConnectionId, _connectionTracker.OnlineUserCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while connecting to NotificationHub (connection {ConnectionId}).",
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
                    $"user-{userId}");

                _connectionTracker.RemoveConnection(userId, Context.ConnectionId);

                _logger.LogInformation(
                    "User {UserId} disconnected from NotificationHub (connection {ConnectionId}). Online users: {OnlineCount}.",
                    userId, Context.ConnectionId, _connectionTracker.OnlineUserCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while disconnecting from NotificationHub (connection {ConnectionId}).",
                    Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(
                exception);
        }
    }
}
