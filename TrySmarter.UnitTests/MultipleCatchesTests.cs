using TrySmarter.Extensions;

namespace TrySmarter.UnitTests;

public sealed class MultipleCatchesTests
{
    [Fact]
    public void MultipleCatches_MatchFirstException_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new ArgumentException("Test argument exception");

        // Act
        var result = TrySmarter.Try<int>(() => throw expectedException)
            .Catch<int, ArgumentException>(e => 1)
            .Catch<int, InvalidOperationException>(e => 2)
            .Catch<int, Exception>(e => 3)
            .ToResult();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void MultipleCatches_MatchSecondException_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test invalid operation exception");

        // Act
        var result = TrySmarter.Try<int>(() => throw expectedException)
            .Catch<int, ArgumentException>(e => 1)
            .Catch<int, InvalidOperationException>(e => 2)
            .Catch<int, Exception>(e => 3)
            .ToResult();

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void MultipleCatches_MatchLastException_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new NotImplementedException("Test not implemented exception");

        // Act
        var result = TrySmarter.Try<int>(() => throw expectedException)
            .Catch<int, ArgumentException>(e => 1)
            .Catch<int, InvalidOperationException>(e => 2)
            .Catch<int, Exception>(e => 3)
            .ToResult();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void MultipleCatches_NoMatchingException_ThrowsException()
    {
        // Arrange
        var expectedException = new NotImplementedException("Test not implemented exception");

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
        {
            TrySmarter.Try<int>(() => throw expectedException)
                .Catch<int, ArgumentException>(e => 1)
                .Catch<int, InvalidOperationException>(e => 2)
                .ToResult();
        });
    }

    [Fact]
    public async Task MultipleCatchesAsync_MatchFirstException_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new ArgumentException("Test argument exception");

        // Act
        var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
            .CatchAsync<int, ArgumentException>(async e => 1)
            .CatchAsync<int, InvalidOperationException>(async e => 2)
            .CatchAsync<int, Exception>(async e => 3)
            .ToResultAsync();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task MultipleCatchesAsync_MatchSecondException_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test invalid operation exception");

        // Act
        var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
            .CatchAsync<int, ArgumentException>(async e => 1)
            .CatchAsync<int, InvalidOperationException>(async e => 2)
            .CatchAsync<int, Exception>(async e => 3)
            .ToResultAsync();

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task MultipleCatchesAsync_MatchLastException_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new NotImplementedException("Test not implemented exception");

        // Act
        var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
            .CatchAsync<int, ArgumentException>(async e => 1)
            .CatchAsync<int, InvalidOperationException>(async e => 2)
            .CatchAsync<int, Exception>(async e => 3)
            .ToResultAsync();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task MultipleCatchesAsync_NoMatchingException_ThrowsException()
    {
        // Arrange
        var expectedException = new NotImplementedException("Test not implemented exception");

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(async () =>
        {
            await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
                .CatchAsync<int, ArgumentException>(async e => 1)
                .CatchAsync<int, InvalidOperationException>(async e => 2)
                .ToResultAsync();
        });
    }
}