using Domain.Conversations;
using Infrastructure.Conversations.Mappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Conversations.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ContextDb context;

    public MessageRepository(ContextDb context)
    {
        this.context = context;
    }

    public void Add(Message message)
    {
        context.Add(MessageMapper.ToEntity(message));
    }

    public async Task<List<Message>> GetAllMessagesAsync(Guid conversationId)
    {
        var messageEntities = await this.context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        return messageEntities.Select(m => MessageMapper.ToDomain(m)).ToList();
    }

    public async Task<Message?> GetMessageByIdAsync(Guid id)
    {
        var entity = await context.Messages
            .FindAsync(id);

        if (entity is null)
            return null;

        return MessageMapper.ToDomain(entity);
    }

    public async Task UpdateAsync(Message message)
    {
        var entity = await this.context.Messages.FindAsync(message.MessageId.Value);

        if (entity is null) 
            return;

        entity.Content = message.Content;
        entity.IsDeleted = message.IsDeleted;
    }
}
