using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUserService;

        public NotificationController(INotificationService notificationService, ICurrentUserService currentUserService)
        {
            _notificationService = notificationService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = _currentUserService.GetCurrentAppUserId();

            var notifications = await _notificationService
                .GetUserNotificationsAsync(userId);

            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = _currentUserService.GetCurrentAppUserId();

            var count = await _notificationService
                .GetUnreadCountAsync(userId);

            return Ok(count);
        }

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = _currentUserService.GetCurrentAppUserId();

            await _notificationService.MarkAsReadAsync(id, userId);

            return Ok();
        }

        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = _currentUserService.GetCurrentAppUserId();

            await _notificationService.MarkAllAsReadAsync(userId);

            return Ok();
        }
    }

}
