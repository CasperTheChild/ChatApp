namespace Domain.Entities;

public class ConversationParticipantEntity
{
    public int ConversationId { get; set; }

    public string UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public int? LastReadMessageId { get; set; }
}
