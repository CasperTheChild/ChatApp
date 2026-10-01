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
        return await this.context.Messages
            .Where(m => m.ConversationId == conversationId)
            .Select(m => MessageMapper.ToDomain(m))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(Guid id)
    {
        var entity = await context.Messages
            .FindAsync(id);

        if (entity is null)
            return null;

        return MessageMapper.ToDomain(entity);
    }

    public void Update(Message message)
    {
        this.context.Messages.Update(MessageMapper.ToEntity(message));
    }
}
