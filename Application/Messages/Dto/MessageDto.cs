namespace Application.Messages.Dto;

public record MessageDto(
    Guid MessageId,
    string UserId,
    Guid ConversationId,
    string Content,
    DateTime CreatedAt,
    DateTime LastUpdatedAt,
    bool IsDeleted)
{ }