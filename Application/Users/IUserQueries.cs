using Application.Common.Dto;
using Application.Users.Dto;

namespace Application.Users;

public interface IUserQueries
{
    public Task<PaginatedDto<UserSearchDto>> SearchUsersAsync(
        string username,
        int limit,
        string? cursor);

    public Task<PaginatedDto<UserSearchDto>> GetSuggestedUsersAsync(
        string? cursor,
        int limit);
}
