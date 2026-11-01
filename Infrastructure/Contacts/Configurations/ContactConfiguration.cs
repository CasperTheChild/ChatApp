using Infrastructure.Contacts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contacts.Configurations;

internal class ContactConfiguration
    : IEntityTypeConfiguration<ContactEntity>
{
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder
            .HasIndex(c => new { 
                c.ContactListId, 
                c.ContactUserId });

        builder
            .Property(c => c.CustomName)
            .HasMaxLength(100);
    }
}
