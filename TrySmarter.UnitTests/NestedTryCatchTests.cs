using TrySmarter.Extensions;

namespace TrySmarter.UnitTests;

public sealed class NestedTryCatchTests
{
    [Fact]
    public void NestedTryCatch_HandlesCorrectly()
    {
        // Arrange
        var outerException = new InvalidOperationException("Outer exception");
        var innerException = new ArgumentException("Inner exception");

        // Act
        var result = TrySmarter.Try<int>(() =>
            {
                return TrySmarter.Try<int>(() => throw innerException)
                    .Catch<int, ArgumentException>(e => throw outerException)
                    .ToResult();
            })
            .Catch<int, InvalidOperationException>(e => 42)
            .ToResult();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task NestedTryCatchAsync_HandlesCorrectly()
    {
        // Arrange
        var outerException = new InvalidOperationException("Outer exception");
        var innerException = new ArgumentException("Inner exception");

        // Act
        var result = await TrySmarter.TryAsync<int>(async () =>
            {
                return await TrySmarter.TryAsync<int>(() => Task.FromException<int>(innerException))
                    .CatchAsync<int, ArgumentException>(async e => throw outerException)
                    .ToResultAsync();
            })
            .CatchAsync<int, InvalidOperationException>(async e => 42)
            .ToResultAsync();

        // Assert
        Assert.Equal(42, result);
    }
}