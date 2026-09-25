namespace Infrastructure.Persistence.Entities;

public class MessageEntity
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
