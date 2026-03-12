using Nestly.Services.Interfaces;
namespace Nestly.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

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
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new UnauthorizedAccessException("User ID not found in token.");
                }

                return int.Parse(userIdClaim);
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
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized();
                }

                var userId = int.Parse(userIdClaim);

                await _notificationService.MarkAllAsReadAsync(userId);

                return Ok();
            }
        }
    }
}
