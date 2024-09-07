using TrySmarter.Extensions;
using TrySmarter;
using TrySmarter.Entities;

namespace TrySmarter.UnitTests;

public sealed class ExampleUsageTests
{
    // Helper method for testing
    private async Task<int> GetNumberAsync(bool throwException)
    {
        if (throwException)
        {
            throw new ArgumentException("Test exception from GetNumberAsync");
        }

        return await Task.FromResult(42);
    }

    private Task UnitOfWorkAsync(bool throwException)
    {
        if (throwException)
        {
            throw new ArgumentException("Test exception from UnitOfWorkAsync");
        }

        return Task.CompletedTask;
    }

    [Fact]
    public async Task ExampleUsage_SuccessfulExecution_ReturnsResult()
    {
        // Act
        var result = await TrySmarter.TryAsync(() => GetNumberAsync(false))
            .CatchAsync<int, ArgumentException>(async e => 0)
            .ToResultAsync();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task ExampleUsage_SuccessfulExtensionExecution_ReturnsResult()
    {
        var task = () => GetNumberAsync(false);
        // Act
        var result = await task
            .TryAsync()
            .CatchAsync<int, ArgumentException>(async e => 0)
            .ToResultAsync();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task ExampleUsage_ThrowsDifferentException()
    {
        // Act &Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await TrySmarter
            .TryAsync(() => GetNumberAsync(true))
            .CatchAsync<int, ArgumentException>(async e =>
                new InvalidOperationException("Test exception from CatchAsync<int, ArgumentException>"))
            .ToResultAsync());
    }

    [Fact]
    public async Task ExampleUsage_SuccessfulExecution_ReturnsResult2()
    {
        // Act
        var result = await TrySmarter
            .TryAsync(() => GetNumberAsync(false))
            .ToResultAsync();

        // Assert
        Assert.Equal(42, result);
    }


    [Fact]
    public async Task ExampleUsage_SuccessfulExecution_ReturnsResult3()
    {
        // Act
        var result = await TrySmarter
            .TryAsync(() => UnitOfWorkAsync(false))
            .ToResultAsync();

        // Assert
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task ExampleUsage_ThrowsException_HandlesException()
    {
        // Act
        var result = await TrySmarter.TryAsync(() => GetNumberAsync(true))
            .CatchAsync<int, ArgumentException>(async e => 0)
            .ToResultAsync();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task ExampleUsage_ThrowsException_HandlesException2()
    {
        // Act
        var result = await TrySmarter.TryAsync(() => UnitOfWorkAsync(true))
            .CatchAsync<ArgumentException>(async e => Unit.Value)
            .ToResultAsync();

        // Assert
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task ExampleUsage_ThrowsException_HandlesException3()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => await TrySmarter
            .TryAsync(() => UnitOfWorkAsync(true))
            .CatchAsync<ArgumentException>(async e =>
                new NullReferenceException("Test exception from CatchAsync<ArgumentException>"))
            .ToResultAsync());
    }
}