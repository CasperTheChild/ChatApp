using Infrastructure.Conversations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Conversations.Configurations;

internal class ConversationConfigurations
    : IEntityTypeConfiguration<ConversationEntity>
{
    public void Configure(EntityTypeBuilder<ConversationEntity> builder)
    {

        builder.HasKey(c => c.ConversationId);
        builder
            .HasMany(c => c.Participants)
            .WithOne(p => p.Conversation)
            .HasForeignKey(p => p.ConversationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(c => c.Title);

        builder
            .Property(c => c.Title)
            .HasMaxLength(100);

        builder
            .Property(c => c.Description)
            .HasMaxLength(250);
    }
}
