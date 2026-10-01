using Infrastructure.Conversations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Conversations.Configurations;

public class MessageConfigurations
    : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder
            .HasKey(m => m.MessageId);

        builder
            .HasIndex(m => new
            {
                m.ConversationId,
                m.CreatedAt
            });

        builder
            .Property(m => m.Content)
            .HasMaxLength(500);
    }
}
