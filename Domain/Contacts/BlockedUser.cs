namespace Domain.Contacts;

public class BlockedUser
{
    public string UserId { get; private set; } = null!;

    public string ContactUserId { get; private set; } = null!;

    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public BlockedUser(
        string userId,
        string contactUserId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        if (string.IsNullOrWhiteSpace(contactUserId))
            throw new ArgumentNullException(nameof(contactUserId));

        UserId = userId;
        ContactUserId = contactUserId;
    }
}
