namespace Application.Contacts.Exceptions;

public class BlockedUserAlreadyExistsException
    : Exception
{
    public BlockedUserAlreadyExistsException(
        string userId,
        string contactUserId)
        : base($"User {userId} already has user {contactUserId} in contacts")
    { }
}
