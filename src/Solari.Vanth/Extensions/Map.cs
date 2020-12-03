using System;
using Solari.Sol.Abstractions;

namespace Solari.Vanth.Extensions
{
    public static partial class ResultExtensions
    {
        //TODO Fix
        // public static T Map<T, TData>(this SimpleResult<TData> simpleResult, Func<SimpleResult<TData>, T> func)
        // {
        //     Check.ThrowIfNull(func, nameof(Func<SimpleResult<TData>>), new VanthException("Cannot invoke a null mapping function"));
        //     return func(simpleResult);
        // }
        //
        // public static ISimpleResult<K> Map<TData, K>(this SimpleResult<TData> simpleResult, Func<TData, K> func)
        // {
        //     Check.ThrowIfNull(func, nameof(Func<SimpleResult<TData>>), new VanthException("Cannot invoke a null mapping function"));
        //     if (simpleResult.HasErrors())
        //         return Result.Error<K>(simpleResult.Errors);
        //
        //     return Result.Data(func(simpleResult.Data));
        // }


    }
}
