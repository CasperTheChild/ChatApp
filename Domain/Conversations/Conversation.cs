using Domain.Common;

namespace Domain.Conversations;

public class Conversation
{
    public ConversationId ConversationId { get; }

    public DateTime CreatedAt { get; }

    private readonly List<Participant> participants = new();

    public IReadOnlyList<Participant> Participants => participants;

    public ConversationType Type { get; private set; }

    public string? Title { get; private set; }

    public string? Description { get; private set; }

    private const int MaxTitleLength = 50;
    private const int MaxDescriptionLength = 200;

    public Conversation(
        ConversationId conversationId,
        DateTime createdAt,
        List<Participant> participants,
        ConversationType type,
        string? title,
        string? description)
    {
        ArgumentNullException.ThrowIfNull(participants);

        if (!Enum.IsDefined(typeof(ConversationType), type))
            throw new ArgumentOutOfRangeException(nameof(type));

        if (participants.Any(p => p is null))
            throw new ArgumentException(
                "Participants cannot contain null values.",
                nameof(participants));

        if (participants.Select(p => p.UserId).Distinct().Count()
            != participants.Count)
            throw new DomainException("Duplicate participants.");

        if (type == ConversationType.Direct && participants.Count != 2)
            throw new DomainException(
                "Direct conversations must have exactly two participants.");

        if (type == ConversationType.Group)
        {
            ValidateTitle(title);
            ValidateDescription(description);

            Title = title!.Trim();
            Description = description?.Trim();
        }
        else if (title != null || description != null)
        {
            throw new DomainException(
                "Direct conversations cannot have group metadata.");
        }

        ConversationId = conversationId;
        CreatedAt = createdAt;
        Type = type;
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

        ValidateTitle(title);
        ValidateDescription(description);

        this.ConversationId = ConversationId.Generate();
        this.CreatedAt = DateTime.UtcNow;
        this.participants.Add(Participant.Create(userId));
        this.Type = ConversationType.Group;
        this.Title = title;
        this.Description = description;
    }

    private Conversation(
        string userId,
        string contactUserId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "UserId can not be empty",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(contactUserId))
            throw new ArgumentException(
                "UserId can not be empty",
                nameof(contactUserId));

        this.ConversationId = ConversationId.Generate();
        this.CreatedAt = DateTime.UtcNow;
        this.participants.Add(Participant.Create(userId));
        this.participants.Add(Participant.Create(contactUserId));
        this.Type = ConversationType.Direct;
    }

    // Domain methods
    public static Conversation CreateGroupConversation(
        string userId, 
        string title, 
        string description)
        => new Conversation(
            userId, 
            title, 
            description);

    public static Conversation CreateDirectConversation(
        string userId,
        string contactUserId)
        => new Conversation(
            userId,
            contactUserId);

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
        ValidateConversationType(nameof(AddParticipant));

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

    public void AddMultipleParticipants(IEnumerable<string> userIds)
    {
        ArgumentNullException.ThrowIfNull(userIds);

        ValidateConversationType(nameof(AddMultipleParticipants));

        if (userIds.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException(
                "UserIds cannot contain empty values.",
                nameof(userIds));

        if (userIds.Distinct().Count() != userIds.Count())
            throw new DomainException(
                "Duplicate participants are not allowed.");

        if (userIds.Any(id => participants.Any(p => p.UserId == id)))
            throw new DomainException(
                "One or more users are already participants.");

        foreach (var userId in userIds)
            participants.Add(Participant.Create(userId));
    }

    public void RemoveParticipant(string userId)
    {
        ValidateConversationType(nameof(RemoveParticipant));

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

    public void LeaveConversation(string userId)
    {
        ValidateConversationType(nameof(LeaveConversation));

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
        ValidateConversationType(nameof(ChangeTitle));

        ValidateTitle(title);

        Title = title.Trim();
    }

    public void ChangeDescription(string description)
    {
        ValidateConversationType(nameof(ChangeDescription));

        ValidateDescription(description);

        Description = description.Trim();
    }

    private static void ValidateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Title can not be empty",
                nameof(title));

        if (title.Length > MaxTitleLength)
            throw new ArgumentException(
                "Title can not have more than 50 letters",
                nameof(title));

        return;
    }

    private static void ValidateDescription(string? description)
    {
        if (description?.Length > MaxDescriptionLength)
            throw new ArgumentException(
                "Description can not have more than 200 letters",
                nameof(description));
    }

    private void ValidateConversationType(string operation)
    {
        if (Type == ConversationType.Direct)
            throw new DomainException(
                $"This operationf of {operation} can not be handled in Direct chats");
    }
}
