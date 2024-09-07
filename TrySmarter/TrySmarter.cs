using OneOf;
using OneOf.Types;
using TrySmarter.Entities;
using TrySmarter.Interfaces;

namespace TrySmarter;

public static class TrySmarter
{
    #region With Return Type

    public static ICatchable<TResult, Exception> Try<TResult>(this Func<TResult> action)
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

    public static async Task<ICatchable<TResult, Exception>> TryAsync<TResult>(this Func<Task<TResult>> action)
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

    #endregion

    #region Without Return Type

    public static ICatchable<Unit, Exception> Try(this Action action)
    {
        try
        {
            action();
            return new Catchable<Unit>
            {
                Result = Unit.Value
            };
        }
        catch (Exception e)
        {
            return new Catchable<Unit>
            {
                Result = e
            };
        }
    }

    public static async Task<ICatchable<Unit, Exception>> TryAsync(this Func<Task> action)
    {
        try
        {
            await action();
            return new Catchable<Unit>
            {
                Result = Unit.Value
            };
        }
        catch (Exception e)
        {
            return new Catchable<Unit>
            {
                Result = e
            };
        }
    }

    #endregion
}