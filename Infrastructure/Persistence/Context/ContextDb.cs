using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Conversations;
using Infrastructure.Conversations.Entities;
using Microsoft.Identity.Client;
using Infrastructure.Contacts.Entities;
using Infrastructure.Images.Entities;

namespace Infrastructure.Persistence.Context;

public class ContextDb
    : IdentityDbContext<ApplicationUser>
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

    public DbSet<ContactEntity> Contacts { get; set; }

    public DbSet<BlockedUserEntity> BlockedUsers { get; set; }

    public DbSet<ImageEntity> Images { get; set; }
}