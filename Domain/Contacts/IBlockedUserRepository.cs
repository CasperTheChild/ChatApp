using Infrastructure.Common;

namespace Domain.Contacts;

public interface IBlockedUserRepository
{
    public Task<IEnumerable<BlockedUser>> GetAllAsync(
        string userId);

    public Task<bool> IsBlockedAsync(
        string userId,
        string contactUserId);

    public void AddBLockedUser(BlockedUser blockedUser);

    public Task RemoveBlockedUserAsync(
        string userId,
        string contactUserId);
}
