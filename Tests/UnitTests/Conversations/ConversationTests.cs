using Domain.Conversations;

namespace Tests.UnitTests.Conversations;

public class ConversationTests
{
    private static Conversation CreateConversation()
    {
        return Conversation.Create(
            "user-1",
            "General",
            "General discussion");
    }

    [Fact]
    public void Create_AddsCreatorAsParticipant()
    {
        var conversation = CreateConversation();

        Assert.True(conversation.HasParticipant("user-1"));
        Assert.Single(conversation.Participants);
    }

    [Fact]
    public void Create_GeneratesConversationId()
    {
        var conversation = CreateConversation();

        Assert.NotEqual(Guid.Empty, conversation.ConversationId.Value);
    }

    [Fact]
    public void HasParticipant_WhenUserIsNotParticipant_ReturnsFalse()
    {
        var conversation = CreateConversation();

        Assert.False(conversation.HasParticipant("user-2"));
    }

    [Fact]
    public void AddParticipant_WhenUserIsNew_AddsParticipant()
    {
        var conversation = CreateConversation();

        conversation.AddParticipant("user-2");

        Assert.True(conversation.HasParticipant("user-2"));
        Assert.Equal(2, conversation.Participants.Count);
    }

    [Fact]
    public void AddParticipant_WhenUserAlreadyExists_Throws()
    {
        var conversation = CreateConversation();

        Assert.Throws<InvalidOperationException>(
            () => conversation.AddParticipant("user-1"));
    }

    [Fact]
    public void AddParticipant_WhenUserIdIsEmpty_Throws()
    {
        var conversation = CreateConversation();

        Assert.Throws<ArgumentException>(
            () => conversation.AddParticipant(""));
    }

    [Fact]
    public void RemoveParticipant_WhenUserExists_RemovesParticipant()
    {
        var conversation = CreateConversation();
        conversation.AddParticipant("user-2");

        conversation.RemoveParticipant("user-2");

        Assert.False(conversation.HasParticipant("user-2"));
        Assert.Single(conversation.Participants);
    }

    [Fact]
    public void RemoveParticipant_WhenUserDoesNotExist_Throws()
    {
        var conversation = CreateConversation();

        Assert.Throws<InvalidOperationException>(
            () => conversation.RemoveParticipant("user-2"));
    }

    [Fact]
    public void Leave_WhenUserExists_RemovesParticipant()
    {
        var conversation = CreateConversation();

        conversation.Leave("user-1");

        Assert.False(conversation.HasParticipant("user-1"));
        Assert.Empty(conversation.Participants);
    }

    [Fact]
    public void ChangeTitle_WhenTitleIsValid_ChangesTitle()
    {
        var conversation = CreateConversation();

        conversation.ChangeTitle("Development");

        Assert.Equal("Development", conversation.Title);
    }

    [Fact]
    public void ChangeTitle_WhenTitleIsEmpty_Throws()
    {
        var conversation = CreateConversation();

        Assert.Throws<ArgumentException>(
            () => conversation.ChangeTitle(""));
    }

    [Fact]
    public void ChangeDescription_WhenDescriptionIsValid_ChangesDescription()
    {
        var conversation = CreateConversation();

        conversation.ChangeDescription("Project discussion");

        Assert.Equal("Project discussion", conversation.Description);
    }
}
