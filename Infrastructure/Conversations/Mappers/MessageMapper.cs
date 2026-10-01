using Domain.Conversations;
using Infrastructure.Conversations.Entities;

namespace Infrastructure.Conversations.Mappers;

public class MessageMapper
{
    public static Message ToDomain(MessageEntity entity)
    {
        return new Message(
            new MessageId(entity.MessageId),
            entity.UserId,
            new ConversationId(entity.ConversationId),
            entity.Content,
            entity.CreatedAt,
            entity.LastUpdatedAt,
            entity.IsDeleted);
    }

    public static MessageEntity ToEntity(Message domain)
    {
        return new MessageEntity
        {
            MessageId = domain.MessageId.Value,
            UserId = domain.UserId,
            ConversationId = domain.ConversationId.Value,
            Content = domain.Content,
            CreatedAt = domain.CreatedAt,
            LastUpdatedAt = domain.LastUpdatedAt,
            IsDeleted = domain.IsDeleted
        };
    }
}
