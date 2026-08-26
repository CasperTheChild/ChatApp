using Domain.Entities;

namespace Application.Repository.Interfaces;

public interface IConversationRepository
{
    public Task<ConversationEntity> GetConversationByIdAsync(int conversationId);

    public void CreateConversation(ConversationEntity conversation);

    public Task UpdateConversationAsync(int id, ConversationEntity conversation);

    public Task DeleteConversationAsync(int conversationId);

    public Task<IEnumerable<ConversationEntity>> GetAllConversationsAsync(string UserId);
}
