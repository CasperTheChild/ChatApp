namespace Domain.Contacts;

public sealed record BlockedUser(
    string UserId)
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}
