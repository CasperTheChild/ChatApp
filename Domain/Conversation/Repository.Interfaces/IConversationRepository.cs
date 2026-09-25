using Domain.Conversation;

namespace Domain.Conversation.Repository.Interfaces;

public interface IConversationRepository
{
    public Task<Conversation> GetConversationByIdAsync(int conversationId);

    public void CreateConversation(Conversation conversation);

    public Task UpdateConversationAsync(int id, Conversation conversation);

    public Task DeleteConversationAsync(int conversationId);

    public Task<IEnumerable<Conversation>> GetAllConversationsAsync(string UserId);
}
