using Domain.Conversations;

namespace Tests.UnitTests.Messages;

public class MessageTests
{
    private static Message CreateMessage(
        string userId = "user-1",
        string content = "Hello, world!")
    {
        return Message.Create(
            userId,
            ConversationId.Generate(),
            content);
    }

    // Creation

    [Fact]
    public void Create_WithValidData_CreatesMessage()
    {
        // Act
        var message = CreateMessage();

        // Assert
        Assert.NotNull(message.MessageId);
        Assert.Equal("user-1", message.UserId);
        Assert.Equal("Hello, world!", message.Content);
        Assert.False(message.IsDeleted);
    }

    [Fact]
    public void Create_GeneratesMessageId()
    {
        // Act
        var first = CreateMessage();
        var second = CreateMessage();

        // Assert
        Assert.NotEqual(first.MessageId, second.MessageId);
    }

    [Fact]
    public void Create_SetsCreatedAtAndLastUpdatedAtToSameTime()
    {
        // Act
        var message = CreateMessage();

        // Assert
        Assert.Equal(message.CreatedAt, message.LastUpdatedAt);
    }

    [Fact]
    public void Create_SetsCreatedAtToUtcTime()
    {
        // Act
        var message = CreateMessage();

        // Assert
        Assert.Equal(DateTimeKind.Utc, message.CreatedAt.Kind);
    }

    // User validation

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidUserId_ThrowsArgumentException(
        string? userId)
    {
        // Act
        var act = () => Message.Create(
            userId!,
            ConversationId.Generate(),
            "Hello");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Conversation validation

    [Fact]
    public void Create_WithNullConversationId_ThrowsArgumentNullException()
    {
        // Act
        var act = () => Message.Create(
            "user-1",
            null!,
            "Hello");

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    // Content validation

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidContent_ThrowsArgumentException(
        string? content)
    {
        // Act
        var act = () => Message.Create(
            "user-1",
            ConversationId.Generate(),
            content!);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithContentOf500Characters_Succeeds()
    {
        // Arrange
        var content = new string('a', 500);

        // Act
        var message = CreateMessage(content: content);

        // Assert
        Assert.Equal(500, message.Content.Length);
    }

    [Fact]
    public void Create_WithContentLongerThan500Characters_ThrowsArgumentException()
    {
        // Arrange
        var content = new string('a', 501);

        // Act
        var act = () => CreateMessage(content: content);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // ChangeContent

    [Fact]
    public void ChangeContent_WithValidContent_UpdatesContent()
    {
        // Arrange
        var message = CreateMessage();

        // Act
        message.ChangeContent("Updated message");

        // Assert
        Assert.Equal("Updated message", message.Content);
    }

    [Fact]
    public void ChangeContent_UpdatesLastUpdatedAt()
    {
        // Arrange
        var message = CreateMessage();
        var previousUpdatedAt = message.LastUpdatedAt;

        // Act
        message.ChangeContent("Updated message");

        // Assert
        Assert.True(message.LastUpdatedAt >= previousUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ChangeContent_WithInvalidContent_ThrowsArgumentException(
        string? content)
    {
        // Arrange
        var message = CreateMessage();

        // Act
        var act = () => message.ChangeContent(content!);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // DeleteMessage

    [Fact]
    public void DeleteMessage_MarksMessageAsDeleted()
    {
        // Arrange
        var message = CreateMessage();

        // Act
        message.DeleteMessage();

        // Assert
        Assert.True(message.IsDeleted);
    }

    [Fact]
    public void DeleteMessage_UpdatesLastUpdatedAt()
    {
        // Arrange
        var message = CreateMessage();
        var previousUpdatedAt = message.LastUpdatedAt;

        // Act
        message.DeleteMessage();

        // Assert
        Assert.True(message.LastUpdatedAt >= previousUpdatedAt);
    }

    [Fact]
    public void DeleteMessage_WhenAlreadyDeleted_DoesNotChangeTimestamp()
    {
        // Arrange
        var message = CreateMessage();
        message.DeleteMessage();
        var deletedAt = message.LastUpdatedAt;

        // Act
        message.DeleteMessage();

        // Assert
        Assert.True(message.IsDeleted);
        Assert.Equal(deletedAt, message.LastUpdatedAt);
    }

    [Fact]
    public void ChangeContent_WhenMessageIsDeleted_ThrowsInvalidOperationException()
    {
        // Arrange
        var message = CreateMessage();
        message.DeleteMessage();

        // Act
        var act = () => message.ChangeContent("Updated message");

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void ChangeContent_WithContentLongerThan500Characters_ThrowsArgumentException()
    {
        // Arrange
        var message = CreateMessage();
        var content = new string('a', 501);

        // Act
        var act = () => message.ChangeContent(content);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}