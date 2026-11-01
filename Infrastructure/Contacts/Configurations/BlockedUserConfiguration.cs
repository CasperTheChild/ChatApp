using Infrastructure.Contacts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contacts.Configurations;

internal class BlockedUserConfiguration
    : IEntityTypeConfiguration<BlockedUserEntity>
{
    public void Configure(EntityTypeBuilder<BlockedUserEntity> builder)
    {
        builder
            .HasIndex(c => new
            {
                c.ContactListId,
                c.ContactUserId
            });
    }
}
