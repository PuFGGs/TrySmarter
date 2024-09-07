using TrySmarter.Extensions;

namespace TrySmarter.UnitTests;

public sealed class CatchSmarterTests
{
    [Fact]
    public void Catch_MatchingException_HandlesException()
    {
        // Arrange
        var expectedException = new ArgumentException("Test argument exception");

        // Act
        var result = TrySmarter.Try<int>(() => throw expectedException)
            .Catch<int, ArgumentException>(e => 0)
            .ToResult();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void Catch_NonMatchingException_DoesNotHandleException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test invalid operation exception");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            TrySmarter.Try<int>(() => throw expectedException)
                .Catch<int, ArgumentException>(e => 0)
                .ToResult();
        });
    }

    [Fact]
    public async Task CatchAsync_MatchingException_HandlesException()
    {
        // Arrange
        var expectedException = new ArgumentException("Test argument exception");

        // Act
        var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
            .CatchAsync<int, ArgumentException>(async e => 0)
            .ToResultAsync();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CatchAsync_NonMatchingException_DoesNotHandleException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test invalid operation exception");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
                .CatchAsync<int, ArgumentException>(async e => 0)
                .ToResultAsync();
        });
    }
}