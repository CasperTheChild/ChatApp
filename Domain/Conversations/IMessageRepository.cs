namespace Domain.Conversations;

public interface IMessageRepository
{
    Task<Message?> GetMessageByIdAsync(Guid id);

    Task<List<Message>> GetAllMessagesAsync(Guid conversationId);

    void Add(Message message);

    void Update(Message message);
}
