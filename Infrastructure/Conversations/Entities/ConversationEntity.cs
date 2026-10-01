using Infrastructure.Conversations.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Conversations.Entities;

[EntityTypeConfiguration(typeof(ConversationConfigurations))]
public class ConversationEntity
{
    public Guid ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<ParticipantEntity> Participants { get; set; } = new();
}
