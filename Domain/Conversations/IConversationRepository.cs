namespace Domain.Conversations;

public interface IConversationRepository
{
    Task<IEnumerable<Conversation>> GetAllConversationByUserId(string userId);

    Task<Conversation?> GetConversationByIdAsync(Guid id);

    void Add(Conversation conversation);

    Task UpdateAsync(Conversation conversation);

    Task<bool> IsParticipant(string userId, Guid conversationId);
}