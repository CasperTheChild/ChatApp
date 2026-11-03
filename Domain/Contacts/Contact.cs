using System.Reflection.Metadata.Ecma335;

namespace Domain.Contacts;

public class Contact
{
    public string UserId { get; private set; } = null!;

    public string ContactUserId { get; private set; } = null!;

    public string CustomName { get; private set; } = null!;


    public Contact(
        string userId,
        string contactUserId,
        string customName)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(UserId));

        if (string.IsNullOrWhiteSpace(contactUserId))
            throw new ArgumentNullException(nameof(ContactUserId));

        if (string.IsNullOrWhiteSpace(customName))
            throw new ArgumentNullException(nameof(CustomName));

        this.UserId = userId;
        this.ContactUserId = contactUserId;
        this.CustomName = customName;
    }

    public void ChangeCustomName(string customName)
    {
        if (string.IsNullOrWhiteSpace(customName))
            throw new ArgumentNullException(nameof(CustomName));

        this.CustomName = customName;
    }
}
