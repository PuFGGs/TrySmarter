using OneOf;

namespace TrySmarter.Interfaces;

public interface ICatchable<TResult, TException>
{
    OneOf<TResult, TException> Result { get; init; }
}