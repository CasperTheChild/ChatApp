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
            .HasKey(c => new
            {
                c.UserId,
                c.ContactUserId
            });

        builder
            .HasIndex(c => new
            {
                c.UserId,
                c.CustomName
            });

        builder
            .Property(c => c.CustomName)
            .HasMaxLength(100);
    }
}
