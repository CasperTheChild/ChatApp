using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Conversation;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Context;

public class ContextDb : IdentityDbContext<ApplicationUser>
{
    public ContextDb(DbContextOptions<ContextDb> options)
        : base(options)
    {
    }

    public DbSet<ConversationEntity> Conversations { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ConversationParticipantEntity> ConversationParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}