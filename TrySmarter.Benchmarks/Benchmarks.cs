using BenchmarkDotNet.Attributes;
using TrySmarter.Extensions;

namespace TrySmarter.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark(Description = "Normal try-catch - No Exception")]
    public int NormalTryCatch_NoException()
    {
        int result;
        try
        {
            result = GetNumber(false);
        }
        catch (Exception)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - No Exception")]
    public int TrySmarterCatch_NoException()
    {
        var result = TrySmarter.Try(() => GetNumber(false))
            .Catch<int, Exception>(e => 0)
            .ToResult();

        return result;
    }

    [Benchmark(Description = "Normal try-catch - Exception")]
    public int NormalTryCatch_Exception()
    {
        int result;
        try
        {
            result = GetNumber(true);
        }
        catch (Exception)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - Exception")]
    public int TrySmarterCatch_Exception()
    {
        var result = TrySmarter.Try(() => GetNumber(true))
            .Catch<int, Exception>(e => 0)
            .ToResult();

        return result;
    }

    [Benchmark(Description = "Normal try-catch - Specific Exception")]
    public int NormalTryCatch_SpecificException()
    {
        int result;
        try
        {
            result = GetNumber(true);
        }
        catch (InvalidOperationException)
        {
            result = 0;
        }

        return result;
    }

    [Benchmark(Description = "TrySmarter - Specific Exception")]
    public int TrySmarterCatch_SpecificException()
    {
        var result = TrySmarter.Try(() => GetNumber(true))
            .Catch<int, InvalidOperationException>(e => 0)
            .ToResult();

        return result;
    }

    [Benchmark(Description = "Normal try-catch - Multiple Exception")]
    public int NormalTryCatch_MultipleException()
    {
        int result;
        try
        {
            result = GetNumber(true);
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

    [Benchmark(Description = "TrySmarter - Multiple Exception")]
    public int TrySmarterCatch_MultipleException()
    {
        var result = TrySmarter.Try(() => GetNumber(true))
            .Catch<int, ArgumentException>(e => 0)
            .Catch<int, TaskCanceledException>(e => 0)
            .Catch<int, InvalidOperationException>(e => 0)
            .ToResult();

        return result;
    }

    private int GetNumber(bool throwException)
    {
        if (throwException)
        {
            throw new InvalidOperationException("Test exception");
        }

        return 42;
    }
}