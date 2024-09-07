using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using TrySmarter.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run(
            typeof(Program).Assembly,
            ManualConfig
                .Create(DefaultConfig.Instance)
                .WithOptions(ConfigOptions.JoinSummary)
                .WithOptions(ConfigOptions.DisableLogFile));
    }
}