namespace Domain.Contacts;

public sealed record BlockedUser(
    string ContactUserId)
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}
