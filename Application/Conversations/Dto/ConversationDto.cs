using Application.Conversations.Dto;

namespace Application.Conversations.Models;

public record ConversationDto(
    Guid ConversationId,
    DateTime CreatedAt,
    string Title,
    string Description,
    List<ParticipantDto> Participants)
{ }
