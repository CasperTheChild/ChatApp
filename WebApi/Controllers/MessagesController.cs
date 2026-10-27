using Application.Messages;
using Application.Messages.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageService messageService;

        public MessagesController(MessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageCreateDto message)
        {
            var result = await messageService.SendMessage(message);

            return Ok(result);
        }

        [HttpGet("conversations/{conversationId:guid}")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var result = await messageService.GetAllMessagesAsync(conversationId);

            return Ok(result);
        }

        [HttpGet("{messageId:guid}")]
        public async Task<IActionResult> GetMessageById(Guid messageId)
        {
            var result = await messageService.GetMessageByIdAsync(messageId);

            return Ok(result);
        }

        [HttpPut("conversations/{conversationId:guid}/{messageId:guid}")]
        public async Task<IActionResult> UpdateMessage(
            Guid conversationId, 
            Guid messageId, 
            MessageUpdateDto messageDto)
        {
            var result = await messageService.UpdateMessageAsync(
                conversationId, 
                messageId, 
                messageDto);

            return NoContent();
        }

        [HttpGet("conversations/{conversationId:guid}/messages")]
        public async Task<IActionResult> GetMessagesAsync(
            Guid conversationId,
            [FromQuery] int? limit,
            [FromQuery] Guid? lastMessageId)
        {
            var result = await messageService.GetMessagesAsync(
                conversationId, 
                limit, 
                lastMessageId);

            return Ok(result);
        }
    }
}
