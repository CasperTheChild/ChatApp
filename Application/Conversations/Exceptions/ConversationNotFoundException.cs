namespace Application.Conversations.Exceptions;

public sealed class ConversationNotFoundException : Exception
{
    public ConversationNotFoundException(Guid conversationId)
        : base($"Conversation with ID '{conversationId}' was not found.")
    {
    }
}
