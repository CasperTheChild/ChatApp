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
            return Ok(await messageService.SendMessage(message));
        }

        [HttpGet("conversations/{conversationId:guid}")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            return Ok(await messageService.GetAllMessagesAsync(conversationId));
        }

        [HttpGet("{messageId:guid}")]
        public async Task<IActionResult> GetMessageById(Guid messageId)
        {
            return Ok(await messageService.GetMessageByIdAsync(messageId));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessage(MessageDto messageDto)
        {
            await messageService.UpdateMessageAsync(messageDto);
            return NoContent();
        }

    }
}
