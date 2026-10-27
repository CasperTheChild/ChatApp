using Domain.Conversations;
using Infrastructure.Common;
using Infrastructure.Conversations.Entities;
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
            .ThenBy(m => m.MessageId)
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

    public async Task<PageResult<Message>> GetMessagesAsync(
        Guid conversationId, 
        int limit, 
        Guid? beforeMessageId)
    {
        var count = limit + 1;

        var query = context.Messages
            .Where(m => m.ConversationId == conversationId);

        if (beforeMessageId.HasValue)
        {
            var cursor = await this.context.Messages
                .Where(m => m.ConversationId == conversationId
                    && m.MessageId == beforeMessageId)
                .Select(m => new { m.MessageId, m.CreatedAt })
                .SingleAsync();

            query = query
                .Where(m => m.CreatedAt < cursor.CreatedAt
                || 
                (m.CreatedAt == cursor.CreatedAt
                && m.MessageId.CompareTo(cursor.MessageId) < 0));
        }

        var entities = await query
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.MessageId)
            .Take(count)
            .ToListAsync();

        var hasMore = entities.Count > count;
        var items = entities
            .Take(limit)
            .OrderBy(m => m.CreatedAt)
            .ThenBy(m => m.MessageId)
            .Select(MessageMapper.ToDomain)
            .ToArray();

        return new PageResult<Message>
        {
            Items = items,
            HasMore = hasMore
        };
    }
}
