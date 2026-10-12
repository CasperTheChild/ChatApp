namespace Application.Messages.Dto;

public record MessageCreateDto(
    Guid ConversationId,
    String Content)
{ }
