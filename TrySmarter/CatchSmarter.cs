using OneOf;
using TrySmarter.Entities;
using TrySmarter.Interfaces;

namespace TrySmarter;

public static class CatchSmarter
{
    #region Catch

    public static ICatchable<TResult, Exception> Catch<TResult, TException>(
        this ICatchable<TResult, Exception> catchable,
        Func<TException, OneOf<TResult, Exception>> onError)
        where TException : Exception
    {
        if (catchable.Result is { IsT1: true, AsT1: TException e })
        {
            return new Catchable<TResult, Exception>
            {
                Result = onError(e)
            };
        }

        return catchable;
    }

    public static ICatchable<TResult, Exception> Catch<TResult>(
        this ICatchable<TResult, Exception> catchable,
        Func<Exception, OneOf<TResult, Exception>> onError)
    {
        return Catch<TResult, Exception>(catchable, onError);
    }

    #endregion

    #region CatchAsync

    public static async Task<ICatchable<TResult, Exception>> CatchAsync<TResult, TException>(
        this Task<ICatchable<TResult, Exception>> catchableTask,
        Func<TException, Task<OneOf<TResult, Exception>>> onError)
        where TException : Exception
    {
        var catchable = await catchableTask;

        if (catchable.Result is { IsT1: true, AsT1: TException e })
        {
            return new Catchable<TResult, Exception>
            {
                Result = await onError(e)
            };
        }

        return catchable;
    }

    public static Task<ICatchable<TResult, Exception>> CatchAsync<TResult>(
        this Task<ICatchable<TResult, Exception>> catchableTask,
        Func<Exception, Task<OneOf<TResult, Exception>>> onError)
    {
        return CatchAsync<TResult, Exception>(catchableTask, onError);
    }

    public static Task<ICatchable<Unit, Exception>> CatchAsync<TException>(
        this Task<ICatchable<Unit, Exception>> catchableTask,
        Func<TException, Task<OneOf<Unit, Exception>>> onError)
        where TException : Exception
    {
        return CatchAsync<Unit, TException>(catchableTask, onError);
    }

    #endregion
}