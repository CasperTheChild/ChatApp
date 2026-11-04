namespace Application.Contacts.Exceptions;

public class ContactAlreadyExistsException
    : Exception
{
    public ContactAlreadyExistsException(
        string userId, 
        string contactUserId)  
        : base($"User {userId} already has user {contactUserId} in contacts")
    { }
}
