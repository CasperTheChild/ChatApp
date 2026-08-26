namespace Domain.Entities;

public class ConversationEntity
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public IList<MessageEntity> Messages { get; set; } = new List<MessageEntity>();

    public IList<ConversationParticipantEntity> Participants { get; set; } = new List<ConversationParticipantEntity>();
}
