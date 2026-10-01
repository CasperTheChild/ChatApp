using Domain.Conversations;
using Infrastructure.Conversations.Entities;
using System.Data;

namespace Infrastructure.Conversations.Mappers;

public class ConversationMapper
{
    public static Conversation ToDomain(ConversationEntity entity)
    {
        return new Conversation(
            new ConversationId(entity.ConversationId),
            entity.CreatedAt,
            entity.Participants.Select(p => ParticipantMapper.ToDomain(p)).ToList(),
            entity.Title,
            entity.Description
        );
    }

    public static ConversationEntity ToEntity(Conversation domain)
    {
        var conversationId = domain.ConversationId.Value;
        var participants = domain.Participants;

        var participantEntities = participants
            .Select(p => ParticipantMapper.ToEntity(p, domain.ConversationId.Value))
            .ToList();

        return new ConversationEntity
        {
            ConversationId = conversationId,
            CreatedAt = domain.CreatedAt,
            Title = domain.Title,
            Description = domain.Description,
            Participants = participantEntities
        };
    }
}
