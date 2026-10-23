using Domain.Conversations;
using Infrastructure.Conversations.Repositories;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Tests.Integration.Infrastructure;
using Xunit;

namespace Tests.Integration.Infrastructure.Conversations;

public sealed class ConversationRepositoryTests
    : IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture fixture;

    public ConversationRepositoryTests(PostgresFixture fixture)
    {
        this.fixture = fixture;
    }

    private static string NewUserId()
        => $"user-{Guid.NewGuid():N}";

    private static Conversation CreateConversation(
        string userId,
        string title = "Test conversation",
        string description = "Test description")
    {
        return Conversation.Create(userId, title, description);
    }

    private async Task CreateUserAsync(
        ContextDb context,
        string userId)
    {
        context.Users.Add(new ApplicationUser
        {
            Id = userId,
            UserName = userId,
            NormalizedUserName = userId.ToUpperInvariant(),
            Email = $"{userId}@example.com",
            NormalizedEmail = $"{userId}@EXAMPLE.COM",
            SecurityStamp = Guid.NewGuid().ToString()
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task Add_ShouldPersistConversation()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);

        using (var context = fixture.CreateContext())
        {
            await CreateUserAsync(context, userId);

            var repository = new ConversationRepository(context);
            repository.Add(conversation);

            await context.SaveChangesAsync();
        }

        using var verifyContext = fixture.CreateContext();
        var verifyRepository = new ConversationRepository(verifyContext);

        var result = await verifyRepository.GetConversationByIdAsync(
            conversation.ConversationId.Value);

        Assert.NotNull(result);
        Assert.Equal("Test conversation", result.Title);
        Assert.Equal("Test description", result.Description);
        Assert.True(result.HasParticipant(userId));
    }

    [Fact]
    public async Task GetAllConversationByUserId_ShouldReturnOnlyUsersConversations()
    {
        var userId = NewUserId();
        var secondUserId = NewUserId();
        var thirdUserId = NewUserId();
        var fourthUserId = NewUserId();

        var first = CreateConversation(userId, "First");
        first.AddParticipant(secondUserId);

        var second = CreateConversation(thirdUserId, "Second");
        var third = CreateConversation(fourthUserId, "Third");

        using (var setupContext = fixture.CreateContext())
        {
            await CreateUserAsync(setupContext, userId);
            await CreateUserAsync(setupContext, secondUserId);
            await CreateUserAsync(setupContext, thirdUserId);
            await CreateUserAsync(setupContext, fourthUserId);

            var repository = new ConversationRepository(setupContext);
            repository.Add(first);
            repository.Add(second);
            repository.Add(third);

            await setupContext.SaveChangesAsync();
        }

        using var queryContext = fixture.CreateContext();
        var queryRepository = new ConversationRepository(queryContext);

        var result = (await queryRepository
            .GetAllConversationByUserId(userId))
            .ToList();

        Assert.Single(result);
        Assert.Equal(first.ConversationId, result[0].ConversationId);
    }

    [Fact]
    public async Task GetConversationByIdAsync_WhenConversationDoesNotExist_ShouldReturnNull()
    {
        using var context = fixture.CreateContext();
        var repository = new ConversationRepository(context);

        var result = await repository.GetConversationByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task IsParticipant_WhenUserIsParticipant_ShouldReturnTrue()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);

        using (var setupContext = fixture.CreateContext())
        {
            await CreateUserAsync(setupContext, userId);

            var repository = new ConversationRepository(setupContext);
            repository.Add(conversation);

            await setupContext.SaveChangesAsync();
        }

        using var queryContext = fixture.CreateContext();
        var queryRepository = new ConversationRepository(queryContext);

        var result = await queryRepository.IsParticipant(
            userId,
            conversation.ConversationId.Value);

        Assert.True(result);
    }

    [Fact]
    public async Task IsParticipant_WhenUserIsNotParticipant_ShouldReturnFalse()
    {
        var userId = NewUserId();
        var otherUserId = NewUserId();
        var conversation = CreateConversation(userId);

        using (var setupContext = fixture.CreateContext())
        {
            await CreateUserAsync(setupContext, userId);
            await CreateUserAsync(setupContext, otherUserId);

            var repository = new ConversationRepository(setupContext);
            repository.Add(conversation);

            await setupContext.SaveChangesAsync();
        }

        using var queryContext = fixture.CreateContext();
        var queryRepository = new ConversationRepository(queryContext);

        var result = await queryRepository.IsParticipant(
            otherUserId,
            conversation.ConversationId.Value);

        Assert.False(result);
    }

    [Fact]
    public async Task IsParticipant_WhenConversationDoesNotExist_ShouldReturnFalse()
    {
        var userId = NewUserId();

        using var setupContext = fixture.CreateContext();
        await CreateUserAsync(setupContext, userId);

        var repository = new ConversationRepository(setupContext);

        var result = await repository.IsParticipant(
            userId,
            Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task Update_ShouldPersistTitleDescriptionAndNewParticipant()
    {
        var originalUserId = NewUserId();
        var newUserId = NewUserId();
        var conversation = CreateConversation(originalUserId);

        using (var setupContext = fixture.CreateContext())
        {
            await CreateUserAsync(setupContext, originalUserId);
            await CreateUserAsync(setupContext, newUserId);

            var repository = new ConversationRepository(setupContext);
            repository.Add(conversation);

            await setupContext.SaveChangesAsync();
        }

        conversation.ChangeTitle("Updated title");
        conversation.ChangeDescription("Updated description");
        conversation.AddParticipant(newUserId);

        using (var updateContext = fixture.CreateContext())
        {
            var repository = new ConversationRepository(updateContext);

            await repository.Update(conversation);
            await updateContext.SaveChangesAsync();
        }

        using var verifyContext = fixture.CreateContext();
        var verifyRepository = new ConversationRepository(verifyContext);

        var result = await verifyRepository.GetConversationByIdAsync(
            conversation.ConversationId.Value);

        Assert.NotNull(result);
        Assert.Equal("Updated title", result.Title);
        Assert.Equal("Updated description", result.Description);
        Assert.True(result.HasParticipant(originalUserId));
        Assert.True(result.HasParticipant(newUserId));
    }
}
