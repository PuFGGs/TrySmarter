using BenchmarkDotNet.Attributes;
using TrySmarter.Extensions;

namespace TrySmarter.Benchmarks;

[MemoryDiagnoser]
public class BenchmarksAsync
{
    [Benchmark(Description = "Normal try-catch - No Exception - Async")]
    public async Task<int> NormalTryCatch_NoExceptionAsync()
    {
        int result;
        try
        {
            result = await GetNumberAsync(false);
        }
        catch (Exception)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - No Exception - Async")]
    public async Task<int> TrySmarterCatch_NoExceptionAsync()
    {
        var result = await TrySmarter.TryAsync(() => GetNumberAsync(false))
            .CatchAsync(async e => 0)
            .ToResultAsync();

        return result;
    }

    [Benchmark(Description = "Normal try-catch - Exception - Async")]
    public async Task<int> NormalTryCatch_ExceptionAsync()
    {
        int result;
        try
        {
            result = await GetNumberAsync(true);
        }
        catch (Exception)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - Exception - Async")]
    public async Task<int> TrySmarterCatch_ExceptionAsync()
    {
        var result = await TrySmarter.TryAsync(() => GetNumberAsync(true))
            .CatchAsync(async e => 0)
            .ToResultAsync();

        return result;
    }

    [Benchmark(Description = "Normal try-catch - Specific Exception - Async")]
    public async Task<int> NormalTryCatch_SpecificExceptionAsync()
    {
        int result;
        try
        {
            result = await GetNumberAsync(true);
        }
        catch (InvalidOperationException)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - Specific Exception - Async")]
    public async Task<int> TrySmarterCatch_SpecificExceptionAsync()
    {
        var result = await TrySmarter.TryAsync(() => GetNumberAsync(true))
            .CatchAsync<int, InvalidOperationException>(async e => 0)
            .ToResultAsync();

        return result;
    }

    [Benchmark(Description = "Normal try-catch - Multiple Exception - Async")]
    public async Task<int> NormalTryCatch_MultipleExceptionAsync()
    {
        int result;
        try
        {
            result = await GetNumberAsync(true);
        }
        catch (ArgumentException)
        {
            result = 0;
        }
        catch (TaskCanceledException)
        {
            result = 0;
        }
        catch (InvalidOperationException)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - Multiple Exception - Async")]
    public async Task<int> TrySmarterCatch_MultipleExceptionAsync()
    {
        var result = await TrySmarter.TryAsync(() => GetNumberAsync(true))
            .CatchAsync<int, ArgumentException>(async e => 0)
            .CatchAsync<int, TaskCanceledException>(async e => 0)
            .CatchAsync<int, InvalidOperationException>(async e => 0)
            .ToResultAsync();

        return result;
    }

    private Task<int> GetNumberAsync(bool throwException)
    {
        if (throwException)
        {
            throw new InvalidOperationException("Test exception");
        }

        return Task.FromResult(42);
    }
}