namespace Application.Contacts.Dto;

public record ContactCompleteDto(
    string contactUserId,
    string customName,
    bool isBlocked)
{
}
