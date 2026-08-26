using Application.Repository.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly ContextDb context;

    public ConversationRepository(ContextDb context)
    {
        this.context = context;
    }

    public void CreateConversation(ConversationEntity conversation)
    {
        this.context.Conversations.Add(conversation);
    }

    public async Task DeleteConversationAsync(int conversationId)
    {
        var entity = await this.context.Conversations.FindAsync(conversationId);

        if (entity != null)
        {
            this.context.Conversations.Remove(entity);
        }
    }

    public async Task<IEnumerable<ConversationEntity>> GetAllConversationsAsync(string UserId)
    {
        return await this.context.Conversations
            .Where(c => c.Participants.Any(p => p.UserId == UserId))
            .ToListAsync();
    }

    public async Task<ConversationEntity?> GetConversationByIdAsync(int conversationId)
    {
        return await this.context.Conversations.FindAsync(conversationId);     
    }

    public async Task UpdateConversationAsync(int id, ConversationEntity conversation)
    {
        var existingConversation = await this.context.Conversations.FindAsync(id);
        if (existingConversation != null)
        {
            // add logic to update the existing conversation with the new data from the provided conversation entity

            this.context.Conversations.Update(existingConversation);
        }
    }
}
