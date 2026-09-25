using Domain.Entities;

namespace Domain.Conversation.Repository.Interfaces;

public interface IConversationParticipantRepository
{
    public Task<ConversationParticipantEntity?> GetParticipantByConversationIdAndUserIdAsync(int conversationId, string userId);
    public void CreateParticipant(ConversationParticipantEntity participant);
    public Task UpdateParticipantAsync(ConversationParticipantEntity participant);
    public Task DeleteParticipantAsync(int conversationId, string userId);
    public Task<IEnumerable<ConversationParticipantEntity>> GetParticipantsByConversationIdAsync(int conversationId);
}
