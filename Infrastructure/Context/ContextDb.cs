using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Context;

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
        modelBuilder.Entity<ConversationEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasMany(c => c.Messages)
                .WithOne()
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(c => c.Participants)
                .WithOne()
                .HasForeignKey(cp => cp.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MessageEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<ConversationParticipantEntity>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.UserId });
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("GETDATE()");
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}