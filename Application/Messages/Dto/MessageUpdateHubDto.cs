namespace Application.Messages.Dto;

public record MessageUpdateHubDto(
    Guid ConversationId,
    Guid MessageId,
    String Content)
{
}
