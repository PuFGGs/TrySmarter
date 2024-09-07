using TrySmarter.Extensions;

namespace TrySmarter.UnitTests
{
    public sealed class TrySmarterTests
    {
        [Fact]
        public void Try_SuccessfulExecution_ReturnsResult()
        {
            // Arrange
            const int expected = 42;

            // Act
            var result = TrySmarter.Try(() => expected).ToResult();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Try_ThrowsException_CatchesException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Test exception");

            // Act
            var result = TrySmarter.Try<int>(() => throw expectedException);

            // Assert
            Assert.IsType<InvalidOperationException>(result.Result.AsT1);
            Assert.Equal(expectedException.Message, result.Result.AsT1.Message);
        }

        [Fact]
        public async Task TryAsync_SuccessfulExecution_ReturnsResult()
        {
            // Arrange
            const int expected = 42;

            // Act
            var result = await TrySmarter.TryAsync(() => Task.FromResult(expected)).ToResultAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task TryAsync_ThrowsException_CatchesException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Test exception");

            // Act
            var result = await TrySmarter.TryAsync<int>(() => Task.FromException<int>(expectedException));

            // Assert
            Assert.IsType<InvalidOperationException>(result.Result.AsT1);
            Assert.Equal(expectedException.Message, result.Result.AsT1.Message);
        }
    }
}