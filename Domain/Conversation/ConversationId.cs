namespace Domain.Conversation;

public record ConversationId
{
    public Guid Value { get; }

    public ConversationId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "Conversation ID cannot be empty.",
                nameof(value));

        Value = value;
    }

    public static ConversationId Generate()
        => new(Guid.NewGuid());
}