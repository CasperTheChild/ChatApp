using Infrastructure.Persistence.Context;
using Domain.Conversations;
using Infrastructure.Conversations.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Conversations.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly ContextDb context;

    public ConversationRepository(ContextDb context)
    {
        this.context = context;
    }

    public void Add(Conversation conversation)
    {
        if (this.context is null)
            throw new Exception("context is NULL");

        if (conversation is null)
            throw new Exception("conversation is NULL");

        var entity = ConversationMapper.ToEntity(conversation);

        if (entity is null)
            throw new Exception("entity is NULL");

        this.context.Add(entity);
    }

    public async Task<IEnumerable<Conversation>> GetAllConversationByUserId(string userId)
    {
        var result = await this.context.Conversations
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .ToListAsync();

        return result.Select(c => ConversationMapper.ToDomain(c));
    }

    public async Task<Conversation?> GetConversationByIdAsync(Guid id)
    {
        var conversation = await this.context.Conversations.FindAsync(id);

        if (conversation is null)
            return null;

        return ConversationMapper.ToDomain(conversation);
    }

    public void Update(Conversation conversation)
    {
        this.context.Update(ConversationMapper.ToEntity(conversation));
    }
}
