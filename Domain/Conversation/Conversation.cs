namespace Domain.Conversation;

public class Conversation
{
    public ConversationId ConversationId { get; set; }

    public ConversationType ConversationType { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Message> Messages { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Conversation(
        ConversationId ConversationId,
        ConversationType ConversationType,
        DateTime CreatedAt,
        List<Message> Messages,
        string Title,
        string Description)
    {
        this.ConversationId = ConversationId;
        this.ConversationType = ConversationType;
        this.CreatedAt = CreatedAt;
        this.Messages = Messages;
        this.Title = Title;
        this.Description = Description;
    }

    // Domain methods
    public bool CanSendMessage(string userId)
    {
        throw new NotImplementedException();
    }

    public void SendMessage(string userId, string text)
    {
        throw new NotImplementedException();
    }

    public void AddParticipant(Participant participant)
    {
        throw new NotImplementedException();
    }

    public void DeleteMessage(Message message)
    {
        throw new NotImplementedException();
    }

    public void ChangeTitle(string title)
    {
        throw new NotImplementedException();
    }

    public void ChangeDescription(string description)
    {
        throw new NotImplementedException();
    }
}
