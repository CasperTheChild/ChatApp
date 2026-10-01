using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Conversations;
using Infrastructure.Conversations.Entities;

namespace Infrastructure.Persistence.Context;

public class ContextDb : IdentityDbContext<ApplicationUser>
{
    public ContextDb(DbContextOptions<ContextDb> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<ConversationEntity> Conversations { get; set; }

    public DbSet<ParticipantEntity> Participants { get; set; }

    public DbSet<MessageEntity> Messages { get; set; }
}