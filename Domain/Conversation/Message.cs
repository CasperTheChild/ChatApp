namespace Domain.Conversation;

public class Message
{
    public MessageId MessageId { get; }

    public string UserId {  get; }

    public ConversationId ConversationId { get; }

    public string Content { get; private set; }

    public DateTime CreatedAt { get; }

    public DateTime LastUpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    // Constructurs

    private Message(
        string userId,
        ConversationId conversationId,
        string content)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException(
                "User Id can not be empty!",
                nameof(userId));

        if (conversationId is null)
            throw new ArgumentNullException(nameof(conversationId));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Content can not be null",
                nameof(content));

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
}
