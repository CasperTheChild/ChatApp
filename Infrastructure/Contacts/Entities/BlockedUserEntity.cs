using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contacts.Entities;

[EntityTypeConfiguration(typeof(BlockedUserEntity))]
public class BlockedUserEntity
{
    public string ContactUserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid ContactListId { get; set; }

    public ContactListEntity ContactList { get; set; } = null!;
}
