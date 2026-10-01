using Infrastructure.Conversations.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Conversations.Entities;

[EntityTypeConfiguration(typeof(MessageConfigurations))]
public class MessageEntity
{
    public Guid MessageId { get; set; }

    public Guid ConversationId { get; set; }

    public string UserId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
