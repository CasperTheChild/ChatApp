namespace Application.Contacts.Exceptions;

public class ContactNotFoundException
    : Exception
{
    public ContactNotFoundException(
        string userId,
        string contactUserId)
        : base($"No contact of {{ {userId}, {contactUserId} }} was found")
    { }
}
