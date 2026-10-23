
namespace Application.Conversations.Exceptions;

public sealed class ParticipantNotFoundException : Exception
{
    public ParticipantNotFoundException(string userId, Guid conversationId)
        : base($"User '{userId}' is not a participant in conversation '{conversationId}'.")
    {
    }
}