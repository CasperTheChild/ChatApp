using Application.Messages;
using Application.Messages.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly MessageService messageService;

    public ChatHub(MessageService messageService)
    {
        this.messageService = messageService;
    }

    public async Task SendMessage(MessageCreateDto message)
    {
        var newMessage = await messageService.SendMessage(message);

        await Clients
            //.Group(newMessage.ConversationId.ToString())
            .All
            .SendAsync("ReceiveMessage", newMessage);
    }
}
