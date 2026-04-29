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

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(claim))
            {
                throw new UnauthorizedAccessException("userId not found in token.");
            }

            return int.Parse(claim);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = GetUserId();

            var notifications = await _notificationService
                .GetUserNotificationsAsync(userId);

            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetUserId();

            var count = await _notificationService
                .GetUnreadCountAsync(userId);

            return Ok(count);
        }

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = GetUserId();

            var success = await _notificationService
                .MarkAsReadAsync(id, userId);

            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetUserId();

            await _notificationService.MarkAllAsReadAsync(userId);

            return Ok();
        }
    }

}
