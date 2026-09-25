using Domain.Conversation.Repository.Interfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ConversationParticipantRepository : IConversationParticipantRepository
{
    private readonly ContextDb context;

    public ConversationParticipantRepository(ContextDb context)
    {
        this.context = context;
    }

    public void CreateParticipant(ConversationParticipantEntity participant)
    {
        context.ConversationParticipants.Add(participant);
    }

    public async Task DeleteParticipantAsync(int conversationId, string userId)
    {
        var entity = await context.ConversationParticipants.FindAsync(conversationId, userId);
        if (entity != null)
        {
            context.ConversationParticipants.Remove(entity);
        }
    }

    public async Task<ConversationParticipantEntity?> GetParticipantByConversationIdAndUserIdAsync(int conversationId, string userId)
    {
        return await context.ConversationParticipants.FindAsync(conversationId, userId);
    }

    public async Task<IEnumerable<ConversationParticipantEntity>> GetParticipantsByConversationIdAsync(int conversationId)
    {
        return await context.ConversationParticipants
            .Where(p => p.ConversationId == conversationId)
            .ToListAsync();
    }

    public async Task UpdateParticipantAsync(ConversationParticipantEntity participant)
    {
        var existingParticipant = await context.ConversationParticipants.FindAsync(participant.ConversationId, participant.UserId);
        if (existingParticipant != null)
        {
            // Update the existing participant with the new data from the provided participant entity
            context.ConversationParticipants.Update(participant);
        }
    }
}
