using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Sol.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Executes a function in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="func">Function</param>
        /// <typeparam name="TReturn">Return type</typeparam>
        /// <typeparam name="T">Input type</typeparam>
        /// <returns>IEnumerable{TReturn}</returns>
        public static IEnumerable<TReturn> ExecuteFunc<T, TReturn>(this IEnumerable<T> enumerable, Func<T, TReturn> func) { return enumerable.Select(func); }

        /// <summary>
        ///     Executes a <see cref="Action" /> in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="action">Action</param>
        /// <typeparam name="T">Input</typeparam>
        public static void ExecuteAction<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable) action(item);
        }

        /// <summary>
        ///     Executes all the actions in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="instance">Instance of T</param>
        /// <typeparam name="T">Input</typeparam>
        public static void ExecuteAction<T>(this IEnumerable<Action<T>> enumerable, T instance)
        {
            foreach (Action<T> action in enumerable) action(instance);
        }

        /// <summary>
        ///     Executes all the actions in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="instance">Instance of T</param>
        /// <param name="instance2">Instance of T2</param>
        /// <typeparam name="T1">Input 1</typeparam>
        /// <typeparam name="T2">Input 2</typeparam>
        public static void ExecuteAction<T1, T2>(this IEnumerable<Action<T1, T2>> enumerable, T1 instance, T2 instance2)
        {
            foreach (Action<T1, T2> action in enumerable) action(instance, instance2);
        }

        /// <summary>
        ///     Executes all the actions in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="instance">Instance of T</param>
        /// <param name="instance2">Instance of T2</param>
        /// <param name="instance3">Instance of T3</param>
        /// <typeparam name="T1">Input 1</typeparam>
        /// <typeparam name="T2">Input 2</typeparam>
        /// <typeparam name="T3">Input 3</typeparam>
        public static void ExecuteAction<T1, T2, T3>(this IEnumerable<Action<T1, T2, T3>> enumerable, T1 instance, T2 instance2, T3 instance3)
        {
            foreach (Action<T1, T2, T3> action in enumerable) action(instance, instance2, instance3);
        }

        /// <summary>
        ///     Executes all the actions in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="instance">Instance of T</param>
        /// <param name="instance2">Instance of T2</param>
        /// <param name="instance3">Instance of T3</param>
        /// <param name="instance4">Instance of T4</param>
        /// <typeparam name="T1">Input 1</typeparam>
        /// <typeparam name="T2">Input 2</typeparam>
        /// <typeparam name="T3">Input 3</typeparam>
        /// <typeparam name="T4">Input 4</typeparam>
        public static void ExecuteAction<T1, T2, T3, T4>(this IEnumerable<Action<T1, T2, T3, T4>> enumerable, T1 instance, T2 instance2, T3 instance3,
                                                         T4 instance4)
        {
            foreach (Action<T1, T2, T3, T4> action in enumerable) action(instance, instance2, instance3, instance4);
        }

        /// <summary>
        ///     Executes all the actions in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="instance">Instance of T</param>
        /// <param name="instance2">Instance of T2</param>
        /// <param name="instance3">Instance of T3</param>
        /// <param name="instance4">Instance of T4</param>
        /// <param name="instance5">Instance of T5</param>
        /// <typeparam name="T1">Input 1</typeparam>
        /// <typeparam name="T2">Input 2</typeparam>
        /// <typeparam name="T3">Input 3</typeparam>
        /// <typeparam name="T4">Input 4</typeparam>
        /// <typeparam name="T5">Input 5</typeparam>
        public static void ExecuteAction<T1, T2, T3, T4, T5>(this IEnumerable<Action<T1, T2, T3, T4, T5>> enumerable, T1 instance, T2 instance2, T3 instance3,
                                                             T4 instance4, T5 instance5)
        {
            foreach (Action<T1, T2, T3, T4, T5> action in enumerable) action(instance, instance2, instance3, instance4, instance5);
        }

        /// <summary>
        ///     Executes all the actions in the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="instance">Instance of T</param>
        /// <param name="instance2">Instance of T2</param>
        /// <param name="instance3">Instance of T3</param>
        /// <param name="instance4">Instance of T4</param>
        /// <param name="instance5">Instance of T5</param>
        /// ///
        /// <param name="instance6">Instance of T6</param>
        /// <typeparam name="T1">Input 1</typeparam>
        /// <typeparam name="T2">Input 2</typeparam>
        /// <typeparam name="T3">Input 3</typeparam>
        /// <typeparam name="T4">Input 4</typeparam>
        /// <typeparam name="T5">Input 5</typeparam>
        /// ///
        /// <typeparam name="T6">Input 6</typeparam>
        public static void ExecuteAction<T1, T2, T3, T4, T5, T6>(this IEnumerable<Action<T1, T2, T3, T4, T5, T6>> enumerable, T1 instance, T2 instance2,
                                                                 T3 instance3, T4 instance4, T5 instance5, T6 instance6)
        {
            foreach (Action<T1, T2, T3, T4, T5, T6> action in enumerable) action(instance, instance2, instance3, instance4, instance5, instance6);
        }
    }
}