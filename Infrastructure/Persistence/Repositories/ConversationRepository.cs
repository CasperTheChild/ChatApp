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

    public void CreateConversation(Conversation conversation)
    {
        context.Conversations.Add(conversation);
    }

    public async Task DeleteConversationAsync(int conversationId)
    {
        var entity = await context.Conversations.FindAsync(conversationId);

        if (entity != null)
        {
            context.Conversations.Remove(entity);
        }
    }

    public async Task<IEnumerable<Conversation>> GetAllConversationsAsync(string UserId)
    {
        return await context.Conversations
            .Where(c => c.Participants.Any(p => p.UserId == UserId))
            .ToListAsync();
    }

    public async Task<Conversation?> GetConversationByIdAsync(int conversationId)
    {
        return await context.Conversations.FindAsync(conversationId);     
    }

    public async Task UpdateConversationAsync(int id, Conversation conversation)
    {
        var existingConversation = await context.Conversations.FindAsync(id);
        if (existingConversation != null)
        {
            // add logic to update the existing conversation with the new data from the provided conversation entity

            context.Conversations.Update(existingConversation);
        }
    }
}
