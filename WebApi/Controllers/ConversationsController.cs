using Application.Conversations;
using Application.Conversations.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
        return Ok(await conversationService.GetConversationsAsync());
    }
    
    [HttpGet("{conversationId:guid}")]
    public async Task<IActionResult> GetConversationById(
        Guid conversationId)
    {
        var result = await conversationService
            .GetConversationByIdAsync(conversationId);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("group")]
    public async Task<IActionResult> CreateGroupConversation(
        [FromBody] ConversationCreateDto conversation)
    {
        var result = await conversationService
            .CreateGroupConversationAsync(conversation);
        return Ok(result);
    }

    [HttpPost("direct/{contactUserId}")]
    public async Task<IActionResult> CreateDirectConversation(
        string contactUserId) 
    {
        var result = await conversationService
            .CreateDirectConversationAsync(contactUserId);
        return Ok(result);
    }

    [HttpPut("{conversationId:guid}")]
    public async Task<IActionResult> UpdateConversation(
        Guid conversationId, 
        [FromBody] ConversationUpdateDto conversation)
    {
        await conversationService
            .UpdateConversationAsync(conversationId, conversation);
        return NoContent();
    }

    [HttpPost("{conversationId:guid}/add")]
    public async Task<IActionResult> AddParticipant(
        Guid conversationId, 
        [FromBody] ParticipantDto participantDto)
    {
        await conversationService
            .AddParticipantToConversationAsync(conversationId, participantDto);
        return NoContent();
    }

    [HttpPost("{conversationId:guid}/remove")]
    public async Task<IActionResult> RemoveParticipant(
        Guid conversationId, 
        [FromBody] ParticipantDto participant)
    {
        await conversationService
            .RemoveParticipantFromConversationAsync(conversationId, participant);
        return NoContent();
    }

    [HttpPost("{conversationId:guid}/leave")]
    public async Task<IActionResult> LeaveConversation(
        Guid conversationId)
    {
        await conversationService
            .LeaveConversationAsync(conversationId);
        return NoContent();
    }
}
