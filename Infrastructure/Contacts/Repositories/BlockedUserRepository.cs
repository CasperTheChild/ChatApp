using Domain.Contacts;
using Infrastructure.Common;
using Infrastructure.Contacts.Mappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contacts.Repositories;

public class BlockedUserRepository
    : IBlockedUserRepository
{
    private readonly ContextDb context;

    public BlockedUserRepository(ContextDb context)
    {
        this.context = context;
    }

    public void AddBLockedUser(BlockedUser blockedUser)
    {
        context.Add(BlockedUserMapper.ToEntity(blockedUser));
    }

    public async Task<IEnumerable<BlockedUser>> GetAllAsync(string userId)
    {
        var entities = await context.BlockedUsers
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return entities
            .Select(c => new BlockedUser(c.UserId, c.ContactUserId));
    }

    public async Task<bool> IsBlockedAsync(
        string userId, 
        string contactUserId)
    {
        return await context.BlockedUsers
            .AnyAsync(c =>
            c.UserId == userId &&
            c.ContactUserId == contactUserId);
    }

    public async Task RemoveBlockedUserAsync(
        string userId, 
        string contactUserId)
    {
        var entity = await context.BlockedUsers
            .FindAsync(new
            {
                UserId = userId,
                ContactUserId = contactUserId
            });

        if (entity is null)
            return;

        context.Remove(entity);
    }
}
