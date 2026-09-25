namespace Application.Conversation.Models;

public class ConversationParticipantModel
{
    public int ConversationId { get; set; }

    public string UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public int? LastReadMessageId { get; set; }
}
