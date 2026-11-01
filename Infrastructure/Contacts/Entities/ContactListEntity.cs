using Infrastructure.Contacts.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contacts.Entities;

[EntityTypeConfiguration(typeof(ContactListConfiguration))]
public class ContactListEntity
{
    public Guid ContactListId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public List<ContactEntity> Contacts { get; set; } = new();

    public List<BlockedUserEntity> BlockedUsers { get; set; } = new();
}
