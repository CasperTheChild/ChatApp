using Domain.Contacts;
using Infrastructure.Contacts.Entities;

namespace Infrastructure.Contacts.Mappers;

public static class BlockedUserMapper
{
    public static BlockedUser ToDomain(BlockedUserEntity entity)
    {
        return new BlockedUser(
            entity.UserId,
            entity.ContactUserId);
    }

    public static BlockedUserEntity ToEntity(BlockedUser domain)
    {
        return new BlockedUserEntity()
        {
            UserId = domain.UserId,
            ContactUserId = domain.ContactUserId
        };
    }
}
