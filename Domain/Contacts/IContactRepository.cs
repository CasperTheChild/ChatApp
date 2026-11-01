namespace Domain.Contacts;

public interface IContactRepository
{
    public Task<ContactList?> GetContactsAsync(string userId);

    public void AddContactsAsync(ContactList contactList);

    public Task UpdateContactsAsync(ContactList contactList);

    public Task<bool> HasContact(string userId, string ContactUserId);

    public Task<bool> IsBlocked(string userId, string ContactUserId);
}
