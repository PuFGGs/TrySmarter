using TrySmarter.Interfaces;

namespace TrySmarter.Extensions;

public static class CatchSmarterExtensions
{
    #region ToResult

    public static async Task<TResult> ToResultAsync<TResult>(this Task<ICatchable<TResult, Exception>> catchableTask)
    {
        var catchable = await catchableTask;

        var result = catchable.Result.Match(
            r => r,
            e => throw e
        );

        return result;
    }

    public static TResult ToResult<TResult>(this ICatchable<TResult, Exception> catchable)
    {
        return catchable.Result.Match(
            r => r,
            e => throw e
        );
    }

    #endregion
}