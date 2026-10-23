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
        var conversation = await this.context.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.ConversationId == id);

        if (conversation is null)
            return null;

        return ConversationMapper.ToDomain(conversation);
    }

    public async Task<bool> IsParticipant(
        string userId,
        Guid conversationId)
    {
        return await context.Conversations.AnyAsync(c =>
            c.ConversationId == conversationId &&
            c.Participants.Any(p => p.UserId == userId));
    }

    public async Task Update(Conversation newConversation)
    {
        var conversationEntity = await context.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c =>
                c.ConversationId == newConversation.ConversationId.Value);

        if (conversationEntity is null)
            return;

        conversationEntity.Title = newConversation.Title;
        conversationEntity.Description = newConversation.Description;

        var existingUserIds = conversationEntity.Participants
            .Select(p => p.UserId)
            .ToHashSet();

        foreach (var participant in newConversation.Participants)
        {
            if (existingUserIds.Contains(participant.UserId))
                continue;

            conversationEntity.Participants.Add(new Entities.ParticipantEntity() { 
                UserId = participant.UserId,
                ConversationId = newConversation.ConversationId.Value});
            existingUserIds.Add(participant.UserId);
        }
    }
}
