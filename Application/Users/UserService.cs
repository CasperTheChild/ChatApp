using Application.Common.Dto;
using Application.IAM;
using Application.Users.Dto;

namespace Application.Users;

public class UserService
{
    private readonly IUserQueries userQueries;
    private readonly CurrentUserService currentUserService;

    public UserService(
        IUserQueries userQueries,
        CurrentUserService currentUserService)
    {
        this.userQueries = userQueries;
        this.currentUserService = currentUserService;
    }

    public async Task<PaginatedDto<UserSearchDto>> SearchUsersAsync(
        string userName,
        int? limit,
        string? cursor)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var lim = limit ?? 30;

        return await userQueries.SearchUsersAsync(userName, lim, cursor);
    }
}
