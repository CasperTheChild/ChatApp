using Application.Repository.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ContextDb context;

    public MessageRepository(ContextDb context)
    {
        this.context = context;
    }

    public void CreateMessage(MessageEntity message)
    {
        this.context.Messages.Add(message);
    }

    public async Task DeleteMessageAsync(int messageId)
    {
        var message = await this.context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        if (message != null)
        {
            this.context.Messages.Remove(message);
        }
    }

    public async Task<MessageEntity?> GetMessageByIdAsync(int messageId)
    {
        return await this.context.Messages.FindAsync(messageId);
    }

    public Task<IEnumerable<MessageEntity>> GetMessagesByConversationIdAsync(int conversationId)
    {
        return this.context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync()
            .ContinueWith(t => (IEnumerable<MessageEntity>)t.Result);
    }

    public async Task UpdateMessageAsync(int id, MessageEntity message)
    {
        var existingMessage = await this.context.Messages.FindAsync(id);
        if (existingMessage != null)
        {
            existingMessage.Content = message.Content;
            this.context.Messages.Update(existingMessage);
        }
    }
}
