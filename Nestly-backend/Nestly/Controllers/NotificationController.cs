using Microsoft.AspNetCore.Mvc;
using Nestly.Services.Interfaces;
namespace Nestly_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications = await _notificationService
                .GetUserNotificationsAsync(userId);

            return Ok(notifications);
        }

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _notificationService
                .MarkAsReadAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("unread-count/{userId}")]
        public async Task<IActionResult> GetUnreadCount(long userId)
        {
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(count);
        }
    }
}
