using Domain.Conversation;
using Domain.Conversation.Repository.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly ContextDb context;

    public ConversationRepository(ContextDb context)
    {
        this.context = context;
    }

    public void AddAsync(Conversation conversation)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Conversation>> GetAllForUserAsync(string UserId)
    {
        throw new NotImplementedException();
    }

    public Task<Conversation?> GetByIdAsync(int conversationId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Conversation conversation)
    {
        throw new NotImplementedException();
    }
}
