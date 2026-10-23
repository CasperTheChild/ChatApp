using Domain.Conversations;

namespace Tests.UnitTests.Conversations;

public class ConversationIdTests
{
    [Fact]
    public void Constructor_WhenGuidIsEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => new ConversationId(Guid.Empty));
    }

    [Fact]
    public void Constructor_WhenGuidIsValid_PreservesValue()
    {
        var value = Guid.NewGuid();

        var id = new ConversationId(value);

        Assert.Equal(value, id.Value);
    }

    [Fact]
    public void Generate_ReturnsNonEmptyId()
    {
        var id = ConversationId.Generate();

        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void TwoIdsWithSameValue_AreEqual()
    {
        var value = Guid.NewGuid();

        var first = new ConversationId(value);
        var second = new ConversationId(value);

        Assert.Equal(first, second);
    }

    [Fact]
    public void TwoIdsWithDifferentValues_AreNotEqual()
    {
        var first = ConversationId.Generate();
        var second = ConversationId.Generate();

        Assert.NotEqual(first, second);
    }
}