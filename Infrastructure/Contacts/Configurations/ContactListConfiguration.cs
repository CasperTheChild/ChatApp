using Infrastructure.Contacts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contacts.Configurations;

public class ContactListConfiguration
    : IEntityTypeConfiguration<ContactListEntity>
{
    public void Configure(EntityTypeBuilder<ContactListEntity> builder)
    {
        builder.HasKey(c => c.ContactListId);
        builder
            .HasMany(c => c.Contacts)
            .WithOne(c => c.ContactList)
            .HasForeignKey(c => c.ContactListId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.BlockedUsers)
            .WithOne(c => c.ContactList)
            .HasForeignKey(c => c.ContactListId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(c => c.UserId);
    }
}
