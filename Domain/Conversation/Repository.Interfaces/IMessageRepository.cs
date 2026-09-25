using Domain.Entities;

namespace Domain.Conversation.Repository.Interfaces;

public interface IMessageRepository
{
    public Task<MessageEntity?> GetMessageByIdAsync(int messageId);
    public void CreateMessage(MessageEntity message);
    public Task UpdateMessageAsync(int id, MessageEntity message);
    public Task DeleteMessageAsync(int messageId);
    public Task<IEnumerable<MessageEntity>> GetMessagesByConversationIdAsync(int conversationId);
}
