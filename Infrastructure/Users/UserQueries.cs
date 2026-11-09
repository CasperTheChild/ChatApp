using Application.Common.Dto;
using Application.Users;
using Application.Users.Dto;
using Infrastructure.Identity;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

public class UserQueries
    : IUserQueries
{
    private readonly ContextDb context;
    private readonly UserManager<ApplicationUser> userManager;

    public UserQueries(
        ContextDb context,
        UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    // Incorrect
    // Needs to access contacts
    public Task<PaginatedDto<UserSearchDto>> GetSuggestedUsersAsync(
        string? cursor, 
        int limit)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginatedDto<UserSearchDto>> SearchUsersAsync(
        string username, 
        int limit, 
        string? cursor)
    {
        var count = limit + 1;

        var normalizedUserName = userManager.NormalizeName(username);

        var query = context.Users
            .Where(c => c.NormalizedUserName != null &&
            c.NormalizedUserName.StartsWith(
                normalizedUserName!));

        if (cursor != null)
        {
            var cursorUser = context.Users
                .FirstOrDefault(c => c.Id == cursor);

            if (cursorUser is not null)
            {
                query = query
                    .Where(c => c.NormalizedUserName
                        !.CompareTo(cursorUser.NormalizedUserName) < 0);
            }
        }

        query = query
            .OrderByDescending(c => c.NormalizedUserName);

        var hasMore = query.Count() > limit;

        var items = await query
            .Select(c => new UserSearchDto
            (
                c.Id,
                c.UserName!,
                null,
                false
            ))
            .ToListAsync();

        return new PaginatedDto<UserSearchDto>
        {
            Items = items,
            HasMore = hasMore,
            NextCursor = items[items.Count - 1]
        };
    }
}
