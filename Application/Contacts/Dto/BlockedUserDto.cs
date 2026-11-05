namespace Application.Contacts.Dto;

public record BlockedUserDto(
    string userId,
    string contactUserId)
{
}
