namespace Domain.Conversation.Repository.Interfaces;

public interface IConversationRepository
{
    public Task<Conversation?> GetByIdAsync(int conversationId);

    public void AddAsync(Conversation conversation);

    public Task UpdateAsync(Conversation conversation);

    public Task<IEnumerable<Conversation>> GetAllForUserAsync(string UserId);
}
