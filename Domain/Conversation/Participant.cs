namespace Domain.Conversation;

public class Participant
{
    public string UserId { get; }

    private Participant(string userId)
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
