namespace Domain.Contacts;

public class ContactList
{
    public ContactListId ContactListId { get; }

    public string UserId { get; } = string.Empty;

    private readonly List<Contact> contacts = new List<Contact>();

    public IReadOnlyList<Contact> Contacts => contacts;

    private readonly List<BlockedUser> blockedUsers = new List<BlockedUser>();

    public IReadOnlyList<BlockedUser> BlockedUsers => blockedUsers;

    public ContactList(
        ContactListId contactListId, 
        string userId)
    {
        if (contactListId == null)
            throw new ArgumentNullException(nameof(contactListId));

        if (userId == null)
            throw new ArgumentNullException(nameof(userId));

        ContactListId = contactListId;
        UserId = userId;
    }

    public static ContactList Create(
        string userId)
    {
        return new ContactList(
            ContactListId.Generate(),
            userId);
    }

    public void AddContact(
        string contactUserId, 
        string customName)
    {
        CheckUserId(contactUserId);

        if (string.IsNullOrEmpty(customName))
            throw new ArgumentNullException(nameof(customName));

        if (Contacts.Any(c =>
            c.ContactUserId == contactUserId))
            throw new InvalidOperationException(
                "User is already in the contact list!");

        contacts.Add(new Contact(contactUserId, customName));
    }

    public void RemoveContact(
        string userId)
    {
        CheckUserId(userId);

        var contact = contacts
            .FirstOrDefault(c => c.ContactUserId == userId);

        if (contact is null)
            throw new InvalidOperationException(
                "Such contact doesnit exist!");

        contacts.Remove(contact);
    }

    public bool HasContact(
        string userId)
    {
        CheckUserId(userId);

        return contacts
            .Any(c => c.ContactUserId == userId);
    }

    public void BlockUser(
        string userId)
    {
        CheckUserId(userId);

        if (blockedUsers.Any(c => c.UserId == userId))
            throw new InvalidOperationException(
                "User is already blocked");

        blockedUsers.Add(new BlockedUser(userId));
    }

    public void UnblockUser(
        string userId)
    {
        CheckUserId(userId);

        var blockedUser = blockedUsers.FirstOrDefault(c => c.UserId == userId);

        if (blockedUser is null)
            throw new InvalidOperationException(
                "Such blocked user does not exist!");

        blockedUsers.Remove(blockedUser);
    }

    public bool HasBlockedUser(
        string userId)
    {
        CheckUserId(userId);

        return blockedUsers.Any(c => c.UserId == userId);
    }

    private void CheckUserId(
        string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));
    }
}
