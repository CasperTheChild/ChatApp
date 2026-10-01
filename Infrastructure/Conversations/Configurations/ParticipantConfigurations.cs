using Infrastructure.Conversations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Conversations.Configurations;

internal class ParticipantConfigurations
    : IEntityTypeConfiguration<ParticipantEntity>
{
    public void Configure(EntityTypeBuilder<ParticipantEntity> builder)
    {
        builder
            .HasKey(p => new {
                p.UserId,
                p.ConversationId
            });

        builder
            .HasIndex(p => new {
                p.ConversationId,
                p.UserId
            });
    }
}
