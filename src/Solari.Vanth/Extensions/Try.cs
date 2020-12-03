using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Vanth.Extensions
{
    public static partial class ResultExtensions
    {
        //
        // public static ISimpleResult<T> Try<T>(this ISimpleResult<T> simpleResult, Action<ISimpleResult<T>> action, bool addStackTraceOnError)
        // {
        //     try
        //     {
        //         action(simpleResult);
        //         return simpleResult;
        //     }
        //     catch (Exception e)
        //     {
        //         return Result.Exception<T>(e, addStackTraceOnError);
        //     }
        // }

        // public static async Task<ISimpleResult<T>> Try<T>(this ISimpleResult<T> simpleResult, Func<Task> action, bool addStackTraceOnError)
        // {
        //     try
        //     {
        //         await action().DefaultAwait();
        //     }
        //     catch (Exception e)
        //     {
        //         return Result.Exception<T>(e, addStackTraceOnError);
        //     }
        //
        // }

    }
}
