# TrySmarter

TrySmarter is a lightweight C# library that provides a more expressive and flexible way to handle exceptions in both synchronous and asynchronous code. It aims to make error handling more readable and maintainable while reducing boilerplate code.

## Features

- Fluent API for exception handling
- Support for both synchronous and asynchronous operations
- Ability to chain multiple exception handlers
- Generic exception handling
- Easy conversion between exceptions and results
- No more try block contexts.

## Installation

You can install TrySmarter via NuGet Package Manager:

```
Install-Package TrySmarter
```

Or via .NET CLI:

```
dotnet add package TrySmarter
```

## Usage

Here's a usual way of handling exceptions:
```csharp
// Synchronous
int result = 0;

try{
  result = SomeAsyncOperationThatMightThrow();
}
catch (MySpecificException ex) {
  result = HandleSpecificException(ex)
}
catch (Exception ex) {
  result = HandleGenericException(ex)
}
```

Here's a basic example of how to use TrySmarter for same results:

```csharp
using TrySmarter;

// Synchronous
var result = TrySmarter.Try(() => SomeOperationThatMightThrow())
    .Catch<MySpecificException>(e => HandleSpecificException(e))
    .Catch<Exception>(e => HandleGenericException(e))
    .ToResult();

// Asynchronous
var result = await TrySmarter.TryAsync(() => SomeAsyncOperationThatMightThrow())
    .CatchAsync<MySpecificException>(async e => await HandleSpecificExceptionAsync(e))
    .CatchAsync<Exception>(async e => await HandleGenericExceptionAsync(e))
    .ToResultAsync();
```

## How I can make it throw an other exception
Just **return** your exception in any of the catch methods and ToResult method will throw it.

Here's how: 

```csharp
using TrySmarter;

await TrySmarter
            .TryAsync(() => GetNumberAsync(true))
            .CatchAsync<int, ArgumentException>(async e =>
                new InvalidOperationException("Test exception from CatchAsync<int, ArgumentException>")) // just return your desired exception here.
            .ToResultAsync()
```

## How I can make it return a different value from the catch block
Just **return** your value in any of the catch methods and ToResult method will throw it.

Here's how: 

```csharp
using TrySmarter;

await TrySmarter
            .TryAsync(() => GetNumberAsync(true))
            .CatchAsync<int, ArgumentException>(async e => 1024) // just return your desired value here.
            .ToResultAsync()
```



## More examples
You can have a look for all the unit tests for different kind of [examples](https://github.com/PuFGGs/TrySmarter/blob/master/TrySmarter.UnitTests/).
Here's some of the interesting ones:

### Nested try catch
```csharp
 var result = TrySmarter.Try<int>(() =>
            {
                return TrySmarter.Try<int>(() => throw innerException)
                    .Catch<int, ArgumentException>(e => throw outerException)
                    .ToResult();
            })
            .Catch<int, InvalidOperationException>(e => 42)
            .ToResult();
```

### Multiple catches
```csharp
 var result = TrySmarter.Try<int>(() => throw expectedException)
            .Catch<int, ArgumentException>(e => 1)
            .Catch<int, InvalidOperationException>(e => 2)
            .Catch<int, Exception>(e => 3)
            .ToResult();
```

## Running Tests

To run the unit tests:

1. Navigate to the test project directory.
2. Run the following command:

```
dotnet test
```

## Running Benchmarks

To run the benchmarks:

1. Navigate to the benchmark project directory.
2. Run the following command:

```
dotnet run -c Release
```

## Latest Benchmarks
```
| Type            | Method                                          | Mean          | Error      | StdDev     | Gen0   | Allocated |
|---------------- |------------------------------------------------ |--------------:|-----------:|-----------:|-------:|----------:|
| Benchmarks      | 'Normal try-catch - No Exception'               |     0.1940 ns |  0.0106 ns |  0.0089 ns |      - |         - |
| BenchmarksAsync | 'Normal try-catch - No Exception - Async'       |    12.7432 ns |  0.1258 ns |  0.1177 ns | 0.0086 |     144 B |
| Benchmarks      | 'TrySmarter - No Exception'                     |    11.6664 ns |  0.0785 ns |  0.0696 ns | 0.0057 |      96 B |
| BenchmarksAsync | 'TrySmarter - No Exception - Async'             |    56.8024 ns |  0.2411 ns |  0.2255 ns | 0.0272 |     456 B |
| Benchmarks      | 'Normal try-catch - Exception'                  | 4,303.3353 ns | 15.4885 ns | 12.9336 ns | 0.0153 |     344 B |
| BenchmarksAsync | 'Normal try-catch - Exception - Async'          | 4,371.0769 ns | 16.9152 ns | 14.1249 ns | 0.0153 |     344 B |
| Benchmarks      | 'TrySmarter - Exception'                        | 4,405.0389 ns | 31.3402 ns | 27.7823 ns | 0.0229 |     504 B |
| BenchmarksAsync | 'TrySmarter - Exception - Async'                | 4,610.9128 ns | 21.7251 ns | 19.2587 ns | 0.0381 |     728 B |
| Benchmarks      | 'Normal try-catch - Specific Exception'         | 4,302.4634 ns | 16.6633 ns | 14.7716 ns | 0.0153 |     344 B |
| BenchmarksAsync | 'Normal try-catch - Specific Exception - Async' | 4,346.0616 ns | 27.4426 ns | 25.6698 ns | 0.0153 |     344 B |
| Benchmarks      | 'TrySmarter - Specific Exception'               | 4,442.3897 ns | 27.9442 ns | 26.1391 ns | 0.0229 |     504 B |
| BenchmarksAsync | 'TrySmarter - Specific Exception - Async'       | 4,565.7266 ns | 18.9015 ns | 17.6805 ns | 0.0381 |     728 B |
| Benchmarks      | 'Normal try-catch - Multiple Exception'         | 4,388.4622 ns | 22.8639 ns | 21.3869 ns | 0.0153 |     344 B |
| BenchmarksAsync | 'Normal try-catch - Multiple Exception - Async' | 4,451.1154 ns | 21.4792 ns | 19.0407 ns | 0.0153 |     344 B |
| Benchmarks      | 'TrySmarter - Multiple Exception'               | 4,393.2478 ns | 15.5280 ns | 13.7652 ns | 0.0229 |     504 B |
| BenchmarksAsync | 'TrySmarter - Multiple Exception - Async'       | 4,609.4423 ns | 20.7837 ns | 17.3553 ns | 0.0458 |     872 B |

```

This will compile the project in Release mode and run the benchmarks, providing a detailed performance report.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

