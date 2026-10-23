namespace Domain.Conversations;

public class Message
{
    public MessageId MessageId { get; private set; }

    public string UserId { get; private set; }

    public ConversationId ConversationId { get; private set; }

    public string Content { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime LastUpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    private static readonly int MaxMessageLength = 500;

    // Constructurs
    public Message(
        MessageId messageId,
        string userId,
        ConversationId conversationId,
        string content,
        DateTime createdAt,
        DateTime lastUpdatedAt,
        bool isDeleted)
    {
        ValidateUser(userId);
        ValidateConversationId(conversationId);
        ValidateContent(content);
        
        MessageId = messageId;
        UserId = userId;
        ConversationId = conversationId;
        Content = content;
        CreatedAt = createdAt;
        LastUpdatedAt = lastUpdatedAt;
        IsDeleted = isDeleted;
    }

    private Message(
        string userId,
        ConversationId conversationId,
        string content)
    {
        ValidateUser(userId);
        ValidateConversationId(conversationId);
        ValidateContent(content);

        this.MessageId = MessageId.Generate();
        this.UserId = userId;
        this.ConversationId = conversationId;
        this.Content = content;
        this.CreatedAt = DateTime.UtcNow;
        this.LastUpdatedAt = CreatedAt;
        this.IsDeleted = false;
    }

    // Domain methods
    public static Message Create(
        string userId,
        ConversationId conversationId,
        string content)
        => new Message(userId, conversationId, content);

    public void ChangeContent(string content)
    {
        ValidateContent(content);

        if (this.IsDeleted)
            throw new InvalidOperationException(
                "Can not edit a deleted message!");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Content can not be empty!",
                nameof(content));

        this.Content = content;
        this.LastUpdatedAt = DateTime.UtcNow;
    }

    public void DeleteMessage()
    {
        if (this.IsDeleted)
            return;

        this.LastUpdatedAt = DateTime.UtcNow;
        this.IsDeleted = true;
    }

    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Content can not be empty",
                nameof(content));

        if (content.Length > MaxMessageLength)
            throw new ArgumentException(
                $"Content length can not be more than {MaxMessageLength} characters",
                nameof(content));
    }

    private static void ValidateUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "User Id can not be empty!",
                nameof(userId));
    }

    private static void ValidateConversationId(ConversationId conversationId)
    {
        if (conversationId is null)
            throw new ArgumentNullException(nameof(conversationId));
    }
}
