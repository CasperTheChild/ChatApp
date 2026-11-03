using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contacts.Entities;

[EntityTypeConfiguration(typeof(BlockedUserEntity))]
public class BlockedUserEntity
{
    public string UserId { get; set; } = null!;

    public string ContactUserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
