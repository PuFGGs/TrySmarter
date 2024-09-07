using OneOf;
using TrySmarter.Interfaces;

namespace TrySmarter.Entities;

public sealed class Catchable<TResult> : ICatchable<TResult, Exception>
{
    public OneOf<TResult, Exception> Result { get; init; }
}

public sealed class Catchable<TResult, TException> : ICatchable<TResult, TException>
    where TException : Exception
{
    public OneOf<TResult, TException> Result { get; init; }
}