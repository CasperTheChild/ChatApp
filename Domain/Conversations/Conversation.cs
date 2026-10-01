namespace Domain.Conversations;

public class Conversation
{
    public ConversationId ConversationId { get; }

    public DateTime CreatedAt { get; }

    private readonly List<Participant> participants = new();

    public IReadOnlyList<Participant> Participants => participants;

    // Non empty
    public string Title { get; private set; }

    // Can be empty
    public string Description { get; private set; }

    public Conversation(
        ConversationId conversationId,
        DateTime createdAt,
        List<Participant> participants,
        string title,
        string description)
    {
        if (participants is null) 
            throw new ArgumentNullException(nameof(participants));

        if (title is null)
            throw new ArgumentNullException(nameof(title));

        if (description is null)
            throw new ArgumentNullException(nameof(description));

        ConversationId = conversationId;
        CreatedAt = createdAt;
        Title = title;
        Description = description;
        this.participants.AddRange(participants);
    }

    private Conversation(
        string userId,
        string title,
        string description = "")
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "UserId can not be empty", 
                nameof(userId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Title can not be empty", 
                nameof(title));

        this.ConversationId = ConversationId.Generate();
        this.CreatedAt = DateTime.UtcNow;
        this.participants.Add(Participant.Create(userId));
        this.Title = title;
        this.Description = description;
    }

    // Domain methods
    public static Conversation Create(
        string userId, 
        string title, 
        string description)
        => new Conversation(
            userId, 
            title, 
            description);

    public bool HasParticipant(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "UserId can not be empty", 
                nameof(userId));

        foreach(var participant in participants)
        {
            if (participant.UserId == userId)
            {
                return true;
            }
        }

        return false;
    }

    public void AddParticipant(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "UserId can not be empty", 
                nameof(userId));

        if (participants
            .Any(p => p.UserId == userId))
            throw new InvalidOperationException(
                "User is already a participant!");

        participants.Add(Participant.Create(userId));
    }

    public void RemoveParticipant(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "UserId can not be empty", 
                nameof(userId));

        var participant = participants
            .FirstOrDefault(p => p.UserId == userId);

        if (participant is null)
            throw new InvalidOperationException(
                "User is not a participant.");

        participants.Remove(participant);
    }

    public void ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Title can not be empty", 
                nameof(title));

        Title = title;
    }

    public void ChangeDescription(string description)
    {
        if (description is null)
            throw new ArgumentNullException(
                nameof(description));

        Description = description;
    }
}
