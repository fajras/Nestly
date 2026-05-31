using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public ChatController(
            IChatService chatService,
            ICurrentUserService currentUserService)
        {
            _chatService = chatService;
            _currentUserService = currentUserService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(
            [FromBody] SendMessageRequest request)
        {
            var userId =
                _currentUserService
                    .GetCurrentAppUserId();

            await _currentUserService
                .EnsureCanChatWithUserAsync(
                    request.ReceiverUserId);

            await _chatService.SendMessage(
                userId,
                request);

            return Ok();
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId =
                _currentUserService
                    .GetCurrentAppUserId();

            return Ok(
                await _chatService
                    .GetUserChats(userId));
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(
            long conversationId)
        {
            var userId =
                _currentUserService
                    .GetCurrentAppUserId();

            await _currentUserService
                .EnsureConversationOwnershipAsync(
                    conversationId);

            return Ok(
                await _chatService
                    .GetMessages(
                        conversationId,
                        userId));
        }

        [HttpGet("available-users")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            var currentUserId =
                _currentUserService
                    .GetCurrentAppUserId();

            var result =
                await _chatService
                    .GetAvailableUsers(currentUserId);

            return Ok(result);
        }
    }
}