namespace Application.Messages.Dto;

public sealed record MessageDeleteDto(
    Guid ConversationId,
    Guid MessageId)
{
}
