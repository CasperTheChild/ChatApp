namespace Domain.Conversation;

public record MessageId
{
    public Guid Value { get; }

    private MessageId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "Message Id can not be empty!",
                nameof(value));

        Value = value;
    }

    public static MessageId Generate()
        => new MessageId(Guid.NewGuid());
}
