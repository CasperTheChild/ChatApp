using Infrastructure.Contacts.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contacts.Entities;

[EntityTypeConfiguration(typeof(ContactConfiguration))]
public class ContactEntity
{
    public string ContactUserId { get; set; } = null!;

    public string CustomName { get; set; } = null!;

    public Guid ContactListId { get; set; }

    public ContactListEntity ContactList { get; set; } = null!;
}
