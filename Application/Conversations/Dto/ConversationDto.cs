using Application.Conversations.Dto;
using Domain.Conversations;

namespace Application.Conversations.Models;

public record ConversationDto(
    Guid ConversationId,
    DateTime CreatedAt,
    ConversationType ConversationType,
    string? Title,
    string? Description,
    List<ParticipantDto> Participants)
{ }
