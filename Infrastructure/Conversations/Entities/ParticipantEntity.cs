using Infrastructure.Conversations.Configurations;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Conversations.Entities;

[EntityTypeConfiguration(typeof(ParticipantConfigurations))]
public class ParticipantEntity
{
    public string UserId { get; set; } = null!;

    public Guid ConversationId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public ConversationEntity Conversation { get; set; } = null!;
}
