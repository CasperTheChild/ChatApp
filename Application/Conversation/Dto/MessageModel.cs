namespace Application.Conversation.Models;

public class MessageModel
{
    public int Id { get; set; }

    public int ConversationId { get; set; }

    public string Content { get; set; }

    public string SenderId { get; set; }

    public DateTime CreatedAt { get; set; }
}
