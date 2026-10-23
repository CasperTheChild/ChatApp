using Domain.Conversations;
using global::Infrastructure.Conversations.Repositories;
using global::Infrastructure.Identity;
using global::Infrastructure.Persistence.Context;
namespace Tests.Integration.Infrastructure.Conversations;

public sealed class MessageRepositoryTests
    : IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture fixture;

    public MessageRepositoryTests(PostgresFixture fixture)
    {
        this.fixture = fixture;
    }

    private static string NewUserId()
        => $"user-{Guid.NewGuid():N}";

    private static Conversation CreateConversation(string userId)
        => Conversation.Create(userId, "Test conversation", "Test description");

    private static Message CreateMessage(
        string userId,
        ConversationId conversationId,
        string content)
        => Message.Create(userId, conversationId, content);

    private static async Task CreateUserAsync(
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

    private async Task CreateConversationAsync(
        string ownerId,
        Conversation conversation)
    {
        await using var context = fixture.CreateContext();

        // The owner must exist because Participants.UserId
        // references AspNetUsers.Id.
        await CreateUserAsync(context, ownerId);

        var repository = new ConversationRepository(context);
        repository.Add(conversation);

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task Add_ShouldPersistMessage()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);
        var message = CreateMessage(
            userId,
            conversation.ConversationId,
            "Hello, world!");

        await CreateConversationAsync(userId, conversation);

        await using (var context = fixture.CreateContext())
        {
            var repository = new MessageRepository(context);

            repository.Add(message);
            await context.SaveChangesAsync();
        }

        await using var verifyContext = fixture.CreateContext();
        var verifyRepository = new MessageRepository(verifyContext);

        var result = await verifyRepository.GetMessageByIdAsync(
            message.MessageId.Value);

        Assert.NotNull(result);
        Assert.Equal("Hello, world!", result.Content);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(
            conversation.ConversationId.Value,
            result.ConversationId.Value);
        Assert.False(result.IsDeleted);
    }

    [Fact]
    public async Task GetAllMessagesAsync_ShouldReturnMessagesInChronologicalOrder()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);

        var otherUserId = NewUserId();

        var earlier = new Message(
            MessageId.Generate(),
            userId,
            conversation.ConversationId,
            "First message",
            DateTime.UtcNow.AddMinutes(-10),
            DateTime.UtcNow.AddMinutes(-10),
            false);

        var later = new Message(
            MessageId.Generate(),
            userId,
            conversation.ConversationId,
            "Second message",
            DateTime.UtcNow,
            DateTime.UtcNow,
            false);

        var otherConversation = CreateConversation(otherUserId);
        var unrelatedMessage = CreateMessage(
            otherUserId,
            otherConversation.ConversationId,
            "Unrelated message");

        await using (var context = fixture.CreateContext())
        {
            await CreateUserAsync(context, userId);
            await CreateUserAsync(context, otherUserId);

            var conversationRepository = new ConversationRepository(context);
            conversationRepository.Add(conversation);
            conversationRepository.Add(otherConversation);

            await context.SaveChangesAsync();

            var messageRepository = new MessageRepository(context);
            messageRepository.Add(later);
            messageRepository.Add(unrelatedMessage);
            messageRepository.Add(earlier);

            await context.SaveChangesAsync();
        }

        await using var queryContext = fixture.CreateContext();
        var repository = new MessageRepository(queryContext);

        var result = await repository.GetAllMessagesAsync(
            conversation.ConversationId.Value);

        Assert.Equal(2, result.Count);
        Assert.Equal("First message", result[0].Content);
        Assert.Equal("Second message", result[1].Content);
        Assert.True(result[0].CreatedAt <= result[1].CreatedAt);
    }

    [Fact]
    public async Task GetAllMessagesAsync_WhenConversationHasNoMessages_ShouldReturnEmptyList()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);

        await CreateConversationAsync(userId, conversation);

        await using var context = fixture.CreateContext();
        var repository = new MessageRepository(context);

        var result = await repository.GetAllMessagesAsync(
            conversation.ConversationId.Value);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMessageByIdAsync_WhenMessageDoesNotExist_ShouldReturnNull()
    {
        await using var context = fixture.CreateContext();
        var repository = new MessageRepository(context);

        var result = await repository.GetMessageByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetMessageByIdAsync_WhenMessageExists_ShouldReturnMessage()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);
        var message = CreateMessage(
            userId,
            conversation.ConversationId,
            "Find this message");

        await CreateConversationAsync(userId, conversation);

        await using (var context = fixture.CreateContext())
        {
            var repository = new MessageRepository(context);
            repository.Add(message);

            await context.SaveChangesAsync();
        }

        await using var verifyContext = fixture.CreateContext();
        var verifyRepository = new MessageRepository(verifyContext);

        var result = await verifyRepository.GetMessageByIdAsync(
            message.MessageId.Value);

        Assert.NotNull(result);
        Assert.Equal("Find this message", result.Content);
        Assert.Equal(message.MessageId, result.MessageId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistContentChange()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);
        var message = CreateMessage(
            userId,
            conversation.ConversationId,
            "Original content");

        await CreateConversationAsync(userId, conversation);

        await using (var setupContext = fixture.CreateContext())
        {
            var repository = new MessageRepository(setupContext);
            repository.Add(message);

            await setupContext.SaveChangesAsync();
        }

        // Change the domain object, then persist the update.
        message.ChangeContent("Updated content");

        await using (var updateContext = fixture.CreateContext())
        {
            var repository = new MessageRepository(updateContext);

            await repository.UpdateAsync(message);
            await updateContext.SaveChangesAsync();
        }

        await using var verifyContext = fixture.CreateContext();
        var verifyRepository = new MessageRepository(verifyContext);

        var result = await verifyRepository.GetMessageByIdAsync(
            message.MessageId.Value);

        Assert.NotNull(result);
        Assert.Equal("Updated content", result.Content);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistDeletionStatus()
    {
        var userId = NewUserId();
        var conversation = CreateConversation(userId);
        var message = CreateMessage(
            userId,
            conversation.ConversationId,
            "Message to delete");

        await CreateConversationAsync(userId, conversation);

        await using (var setupContext = fixture.CreateContext())
        {
            var repository = new MessageRepository(setupContext);
            repository.Add(message);

            await setupContext.SaveChangesAsync();
        }

        message.DeleteMessage();

        await using (var updateContext = fixture.CreateContext())
        {
            var repository = new MessageRepository(updateContext);

            await repository.UpdateAsync(message);
            await updateContext.SaveChangesAsync();
        }

        await using var verifyContext = fixture.CreateContext();
        var verifyRepository = new MessageRepository(verifyContext);

        var result = await verifyRepository.GetMessageByIdAsync(
            message.MessageId.Value);

        Assert.NotNull(result);
        Assert.True(result.IsDeleted);
    }
}
