using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using System.Security.Claims;

namespace Nestly.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly Nestly.Services.Interfaces.IChatService _chatService;

        public ChatController(Nestly.Services.Interfaces.IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _chatService.SendMessage(userId, request);

            return Ok();
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _chatService.GetUserChats(userId));
        }

        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(long conversationId)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _chatService.GetMessages(conversationId, userId));
        }


    }

}
