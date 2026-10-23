using Application.Conversations;
using Application.Conversations.Dto;
using Application.Conversations.Models;
using Application.IAM;
using Application.Messages;
using Application.Messages.Dto;
using Domain.Conversations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly MessageService messageService;
    private readonly CurrentUserService currentUserService;
    private readonly ConversationService conversationService;

    public ChatHub(
        MessageService messageService,
        CurrentUserService currentUserService,
        ConversationService conversationService)
    {
        this.messageService = messageService;
        this.currentUserService = currentUserService;
        this.conversationService = conversationService;
    }

    public async Task SendMessage(MessageCreateDto message)
    {
        var newMessage = await messageService.SendMessage(message);

        await Clients
            .Group(newMessage.ConversationId.ToString())
            //.All
            .SendAsync("ReceiveMessage", newMessage);
    }

    public async Task UpdateMessage(Guid conversationId, Guid messageId, MessageUpdateDto message)
    {
        var newMessage = await messageService.UpdateMessageAsync(conversationId, messageId, message);

        if (newMessage is null)
            return;

        await Clients
            .Group(newMessage.ConversationId.ToString())
            .SendAsync("ReceiveUpdatedMessage", newMessage);
    }

    public async Task CreateConversation(ConversationCreateDto conversationDto)
    {
        var newConversation = await conversationService.CreateConversation(conversationDto);

        await Clients
            .Group(newConversation.ConversationId.ToString())
            .SendAsync("ReceiveConversation", newConversation);
    }

    public async Task UpdateConversation(Guid conversationId, ConversationUpdateDto conversationDto)
    {
        var newConversation = await conversationService.UpdateConversation(conversationId, conversationDto);
    }

    public async Task AddToGroup(Guid conversationId)
    {
        var userId = currentUserService.GetCurrentUserId();

        if (userId is null)
            return;

        if (!await conversationService.IsParticipantAsync(conversationId, userId))
        {
            throw new HubException("User is not a participant.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
    }

    public async Task RemoveFromGroup(Guid conversationId)
    {
        var userId = currentUserService.GetCurrentUserId();

        if (userId is null)
            return;

        Console.WriteLine($"Conversation ID: {conversationId}");
        Console.WriteLine($"User ID: {userId}");

        var isParticipant = await conversationService
            .IsParticipantAsync(conversationId, userId);

        Console.WriteLine($"Is participant: {isParticipant}");

        if (!isParticipant)
            throw new HubException("User is not a participant.");

        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            conversationId.ToString());
    }
}
