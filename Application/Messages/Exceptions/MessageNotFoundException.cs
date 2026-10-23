namespace Application.Messages.Exceptions;

public class MessageNotFoundException : Exception
{
    public MessageNotFoundException(Guid messageId)
        : base($"Message with ID '{messageId}' was not found.")
    {
    }
    public MessageNotFoundException(Guid messageId, Guid conversationId)
        : base(
            $"Message with ID '{messageId}' was not found " +
            $"in conversation '{conversationId}'.")
    {
    }
}
