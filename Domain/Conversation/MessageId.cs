namespace Domain.Conversation;

public class MessageId
{
    public Guid Id { get; set; }

    public MessageId Generate()
    {
        return new MessageId()
        {
            Id = Guid.NewGuid(),
        };
    }
}
