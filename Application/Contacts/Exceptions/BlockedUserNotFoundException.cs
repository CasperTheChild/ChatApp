namespace Application.Contacts.Exceptions;

public class BlockedUserNotFoundException
    : Exception
{
    public BlockedUserNotFoundException(
        string userId,
        string contactUserId)
        : base($"No blocked user of {{ {userId}, {contactUserId} }} was found")
    { }
}
