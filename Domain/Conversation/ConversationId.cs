namespace Domain.Conversation;

public record ConversationId
{
    public Guid Value { get; init; }

    public static ConversationId Generate()
    {
        return new ConversationId()
        {
            Value = Guid.NewGuid(),
        };
    }
}
