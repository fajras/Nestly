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

        public NotificationHub(
            ICurrentUserService currentUserService,
            ILogger<NotificationHub> logger)
        {
            _currentUserService =
                currentUserService;
            _logger = logger;
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

                _logger.LogInformation(
                    "User {UserId} connected to NotificationHub (connection {ConnectionId}).",
                    userId, Context.ConnectionId);
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

                _logger.LogInformation(
                    "User {UserId} disconnected from NotificationHub (connection {ConnectionId}).",
                    userId, Context.ConnectionId);
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
