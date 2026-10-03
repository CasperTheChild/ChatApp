namespace Application.Messages.Dto;

public record MessageCreateDto(
    string UserId,
    Guid ConversationId,
    String Content)
{ }
