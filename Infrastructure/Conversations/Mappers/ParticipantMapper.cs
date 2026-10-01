using Domain.Conversations;
using Infrastructure.Conversations.Entities;

namespace Infrastructure.Conversations.Mappers;

public class ParticipantMapper
{
    public static ParticipantEntity ToEntity(Participant domain, Guid ConversationId)
    {
        return new ParticipantEntity
        {
            ConversationId = ConversationId,
            UserId = domain.UserId
        };
    }

    public static Participant ToDomain(ParticipantEntity entity)
    {
        return Participant.Create(entity.UserId);
    }
}
