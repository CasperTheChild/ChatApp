using Domain.Conversation.Repository.Interfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ContextDb context;

    public MessageRepository(ContextDb context)
    {
        this.context = context;
    }

    public void CreateMessage(MessageEntity message)
    {
        context.Messages.Add(message);
    }

    public async Task DeleteMessageAsync(int messageId)
    {
        var message = await context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message != null)
        {
            context.Messages.Remove(message);
        }
    }

    public async Task<MessageEntity?> GetMessageByIdAsync(int messageId)
    {
        return await context.Messages.FindAsync(messageId);
    }

    public Task<IEnumerable<MessageEntity>> GetMessagesByConversationIdAsync(int conversationId)
    {
        return context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync()
            .ContinueWith(t => (IEnumerable<MessageEntity>)t.Result);
    }

    public async Task UpdateMessageAsync(int id, MessageEntity message)
    {
        var existingMessage = await context.Messages.FindAsync(id);
        if (existingMessage != null)
        {
            existingMessage.Content = message.Content;
            context.Messages.Update(existingMessage);
        }
    }
}
