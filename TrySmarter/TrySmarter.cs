using OneOf;
using TrySmarter.Entities;
using TrySmarter.Interfaces;

namespace TrySmarter;

public static class TrySmarter
{
    public static ICatchable<TResult, Exception> Try<TResult>(Func<TResult> action)
    {
        try
        {
            return new Catchable<TResult>
            {
                Result = action()
            };
        }
        catch (Exception e)
        {
            return new Catchable<TResult>
            {
                Result = e
            };
        }
    }

    public static async Task<ICatchable<TResult, Exception>> TryAsync<TResult>(Func<Task<TResult>> action)
    {
        try
        {
            return new Catchable<TResult>
            {
                Result = await action()
            };
        }
        catch (Exception e)
        {
            return new Catchable<TResult>
            {
                Result = e
            };
        }
    }
}