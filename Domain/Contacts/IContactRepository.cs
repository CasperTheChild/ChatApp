using Infrastructure.Common;

namespace Domain.Contacts;

public interface IContactRepository
{
    public Task<bool> HasContact(
        string userId, 
        string contactUserId);

    public Task<Contact?> GetContactAsync(
        string userId,
        string contactUserId);

    public Task<IEnumerable<Contact>> GetAllContactsAsync(
        string userId);

    public Task<PageResult<Contact>> GetAllContactsAsync(
        string userId,
        int limit,
        string? contactUserId);

    public void AddContactAsync(
        Contact contactList);

    public Task UpdateContactsAsync(
        Contact contactList);

    public Task DeleteContactsAsync(
        string userId,
        string contactUserId);
}
