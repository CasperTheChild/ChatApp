namespace Application.Conversations.Exceptions;

public class ParticipantAlreadyExistsException : Exception
{
    public ParticipantAlreadyExistsException(Guid conversationId, string userId)
        : base($"Participant {userId} already exists in the conversation {conversationId}") { }
}
