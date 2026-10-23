using Domain.Conversations;

namespace Tests.UnitTests.Conversations;

public class ParticipantTests
{
    [Fact]
    public void Constructor_WhenUserIdIsValid_StoresUserId()
    {
        var participant = new Participant("user-123");

        Assert.Equal("user-123", participant.UserId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_WhenUserIdIsEmptyOrWhitespace_Throws(
        string userId)
    {
        Assert.Throws<ArgumentException>(
            () => new Participant(userId));
    }

    [Fact]
    public void Create_WhenUserIdIsValid_ReturnsParticipant()
    {
        var participant = Participant.Create("user-123");

        Assert.NotNull(participant);
        Assert.Equal("user-123", participant.UserId);
    }
}