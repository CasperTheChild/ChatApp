using Application.Conversations;
using Application.Conversations.Dto;
using Application.Conversations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly ConversationService conversationService;

        public ConversationsController(ConversationService conversationService)
        {
            this.conversationService = conversationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            return Ok(await conversationService.GetConversations());
        }
        
        [HttpGet("{conversationId:guid}")]
        public async Task<IActionResult> GetConversationById(Guid conversationId)
        {
            var result = await conversationService.GetConversationById(conversationId);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] ConversationCreateDto conversation)
        {
            var result = await conversationService.CreateConversation(conversation);
            return Ok(result);
        }

        [HttpPut("{conversationId:guid}")]
        public async Task<IActionResult> UpdateConversation(Guid conversationId, [FromBody] ConversationDto conversation)
        {
            await conversationService.UpdateConversation(conversation);
            return NoContent();
        }

        [HttpPost("{conversationId:guid}")]
        public async Task<IActionResult> AddParticipant(Guid conversationId, [FromBody] ParticipantDto participantDto)
        {
            await conversationService.AddParticipantToConversation(conversationId, participantDto);
            return NoContent();
        }

        [HttpPost("RemoveParticipant/{conversationId:guid}")]
        public async Task<IActionResult> RemoveParticipant(Guid conversationId, [FromBody] ParticipantDto participant)
        {
            await conversationService.RemoveParticipantFromConversation(conversationId, participant);
            return NoContent();
        }

        [HttpPost("LeaveConversation/{conversationId:guid}")]
        public async Task<IActionResult> LeaveConversation(Guid conversationId, [FromBody] ParticipantDto participant)
        {
            await conversationService.LeaveConversation(conversationId, participant);
            return NoContent();
        }
    }
}
