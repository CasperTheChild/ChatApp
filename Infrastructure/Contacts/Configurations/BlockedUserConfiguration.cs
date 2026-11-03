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
            .HasKey(c => new
            {
                c.UserId,
                c.ContactUserId
            });
    }
}
