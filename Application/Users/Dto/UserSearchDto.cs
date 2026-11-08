namespace Application.Users.Dto;

public record UserSearchDto(
    string UserId,
    string UserName,
    string? DisplayName,
    bool IsContact)
{
}
