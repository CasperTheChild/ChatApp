using Application.Contacts.Dto;
using Application.Contacts.Exceptions;
using Application.IAM;
using Domain.Common.Repository.Interfaces;
using Domain.Contacts;
using System.Runtime.CompilerServices;

namespace Application.Contacts;

public class BlockedUserService
{
    private readonly IBlockedUserRepository blockedUserRepository;
    private readonly CurrentUserService currentUserService;
    private readonly IUnitOfWork unitOfWork;

    public BlockedUserService(
        IBlockedUserRepository blockedUserRepository,
        IUnitOfWork unitOfWork,
        CurrentUserService currentUserService)
    {
        this.blockedUserRepository = blockedUserRepository;
        this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BlockedUserDto>> GetBlockedUsersAsync()
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var items = await blockedUserRepository
            .GetAllAsync(userId);

        return items.Select(c => new BlockedUserDto(c.UserId, c.ContactUserId));
    }

    public async Task BlockUserAsync(string contactUserId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var isBlocked = await blockedUserRepository
            .IsBlockedAsync(userId, contactUserId);

        if (isBlocked)
            throw new BlockedUserAlreadyExistsException(userId, contactUserId);

        var blockedUser = new BlockedUser(userId, contactUserId);

        blockedUserRepository.AddBLockedUser(blockedUser);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveBlockedUserAsync(string contactUserId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var isBlocked = await blockedUserRepository
            .IsBlockedAsync(userId, contactUserId);

        if (!isBlocked)
            throw new BlockedUserNotFoundException(userId, contactUserId);

        await blockedUserRepository.RemoveBlockedUserAsync(userId, contactUserId);

        await unitOfWork.SaveChangesAsync();
    }
}
