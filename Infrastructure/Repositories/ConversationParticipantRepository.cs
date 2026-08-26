using Application.Repository.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ConversationParticipantRepository : IConversationParticipantRepository
{
    private readonly ContextDb context;

    public ConversationParticipantRepository(ContextDb context)
    {
        this.context = context;
    }

    public void CreateParticipant(ConversationParticipantEntity participant)
    {
        this.context.ConversationParticipants.Add(participant);
    }

    public async Task DeleteParticipantAsync(int conversationId, string userId)
    {
        var entity = await this.context.ConversationParticipants.FindAsync(conversationId, userId);
        if (entity != null)
        {
            this.context.ConversationParticipants.Remove(entity);
        }
    }

    public async Task<ConversationParticipantEntity?> GetParticipantByConversationIdAndUserIdAsync(int conversationId, string userId)
    {
        return await this.context.ConversationParticipants.FindAsync(conversationId, userId);
    }

    public async Task<IEnumerable<ConversationParticipantEntity>> GetParticipantsByConversationIdAsync(int conversationId)
    {
        return await this.context.ConversationParticipants
            .Where(p => p.ConversationId == conversationId)
            .ToListAsync();
    }

    public async Task UpdateParticipantAsync(ConversationParticipantEntity participant)
    {
        var existingParticipant = await this.context.ConversationParticipants.FindAsync(participant.ConversationId, participant.UserId);
        if (existingParticipant != null)
        {
            // Update the existing participant with the new data from the provided participant entity
            this.context.ConversationParticipants.Update(participant);
        }
    }
}
