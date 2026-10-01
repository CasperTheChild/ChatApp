namespace Domain.Conversations;

public class Participant
{
    public string UserId { get; }

    public Participant(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "UserId can not be empty", 
                nameof(userId));

        this.UserId = userId;
    }

    public static Participant Create(string userId)
        => new Participant(userId);
}
