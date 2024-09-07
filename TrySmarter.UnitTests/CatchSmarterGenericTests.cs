using TrySmarter.Extensions;

namespace TrySmarter.UnitTests;

public sealed class CatchSmarterGenericTests
{
    [Fact]
    public void Catch_GenericException_HandlesAnyExceptionType()
    {
        // Arrange
        var exceptions = new Exception[]
        {
            new ArgumentException("Test argument exception"),
            new InvalidOperationException("Test invalid operation exception"),
            new NotImplementedException("Test not implemented exception")
        };

        foreach (var expectedException in exceptions)
        {
            // Act
            var result = TrySmarter.Try<int>(() => throw expectedException)
                .Catch<int>(e =>
                {
                    Assert.Equal(expectedException.GetType(), e.GetType());
                    Assert.Equal(expectedException.Message, e.Message);
                    return 42;
                })
                .ToResult();

            // Assert
            Assert.Equal(42, result);
        }
    }

    [Fact]
    public void Catch_GenericException_HandlesNestedExceptions()
    {
        // Arrange
        var innerException = new ArgumentException("Inner exception");
        var outerException = new InvalidOperationException("Outer exception", innerException);

        // Act
        var result = TrySmarter.Try<int>(() => throw outerException)
            .Catch<int>(e =>
            {
                Assert.Equal(typeof(InvalidOperationException), e.GetType());
                Assert.Equal(outerException.Message, e.Message);
                Assert.Equal(typeof(ArgumentException), e.InnerException.GetType());
                Assert.Equal(innerException.Message, e.InnerException.Message);
                return 42;
            })
            .ToResult();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task CatchAsync_GenericException_HandlesAnyExceptionType()
    {
        // Arrange
        var exceptions = new Exception[]
        {
            new ArgumentException("Test argument exception"),
            new InvalidOperationException("Test invalid operation exception"),
            new NotImplementedException("Test not implemented exception")
        };

        foreach (var expectedException in exceptions)
        {
            // Act
            var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
                .CatchAsync<int>(async e =>
                {
                    Assert.Equal(expectedException.GetType(), e.GetType());
                    Assert.Equal(expectedException.Message, e.Message);
                    return 42;
                })
                .ToResultAsync();

            // Assert
            Assert.Equal(42, result);
        }
    }

    [Fact]
    public async Task CatchAsync_GenericException_HandlesNestedExceptions()
    {
        // Arrange
        var innerException = new ArgumentException("Inner exception");
        var outerException = new InvalidOperationException("Outer exception", innerException);

        // Act
        var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(outerException))
            .CatchAsync<int>(async e =>
            {
                Assert.Equal(typeof(InvalidOperationException), e.GetType());
                Assert.Equal(outerException.Message, e.Message);
                Assert.Equal(typeof(ArgumentException), e.InnerException.GetType());
                Assert.Equal(innerException.Message, e.InnerException.Message);
                return 42;
            })
            .ToResultAsync();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void Catch_GenericException_AllowsRethrowingException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            TrySmarter.Try<int>(() => throw expectedException)
                .Catch<int>(e => throw e)
                .ToResult();
        });

        Assert.Equal(expectedException.Message, exception.Message);
    }

    [Fact]
    public async Task CatchAsync_GenericException_AllowsRethrowingException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
                .CatchAsync<int>(async e => throw e)
                .ToResultAsync();
        });

        Assert.Equal(expectedException.Message, exception.Message);
    }

    [Fact]
    public void Catch_GenericException_ChainedWithSpecificCatch_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new NotImplementedException("Test not implemented exception");

        // Act
        var result = TrySmarter.Try<int>(() => throw expectedException)
            .Catch<int, ArgumentException>(e => 1)
            .Catch<int>(e =>
            {
                Assert.Equal(typeof(NotImplementedException), e.GetType());
                return 2;
            })
            .ToResult();

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task CatchAsync_GenericException_ChainedWithSpecificCatch_HandlesCorrectly()
    {
        // Arrange
        var expectedException = new NotImplementedException("Test not implemented exception");

        // Act
        var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException))
            .CatchAsync<int, ArgumentException>(async e => 1)
            .CatchAsync<int>(async e =>
            {
                Assert.Equal(typeof(NotImplementedException), e.GetType());
                return 2;
            })
            .ToResultAsync();

        // Assert
        Assert.Equal(2, result);
    }
}