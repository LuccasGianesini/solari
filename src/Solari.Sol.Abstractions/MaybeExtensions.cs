using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Sol.Abstractions
{
    public static class MaybeExtensions
    {
        public static T Unwrap<T>(this Maybe<T> maybe, T defaultValue = default(T))
        {
            return maybe.Unwrap(x => x, defaultValue);
        }

        public static U Unwrap<T, U>(this Maybe<T> maybe, Func<T, U> selector, U defaultValue = default)
        {
            if (maybe.HasValue)
                return selector(maybe.Value);

            return defaultValue;
        }

        public static Maybe<U> Bind<T, U>(this Maybe<T> maybe, Func<T, Maybe<U>> selector)
        {
            if (maybe.HasNoValue)
                return Maybe<U>.None;

            return selector(maybe.Value);
        }
        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            if (maybe.HasNoValue)
                return Maybe<T>.None;

            if (predicate(maybe.Value))
                return maybe;

            return Maybe<T>.None;
        }

        public static Maybe<U> Select<T, U>(this Maybe<T> maybe, Func<T, U> selector)
        {
            return maybe.Map(selector);
        }

        [Obsolete("Use Bind instead of this method")]
        public static Maybe<U> Select<T, U>(this Maybe<T> maybe, Func<T, Maybe<U>> selector)
        {
            return maybe.Bind(selector);
        }

        public static Maybe<U> SelectMany<T, U>(this Maybe<T> maybe, Func<T, Maybe<U>> selector)
        {
            return maybe.Bind(selector);
        }

        public static Maybe<K> Map<T, K>(this Maybe<T> maybe, Func<T, K> selector)
        {
            if (maybe.HasNoValue)
                return Maybe<K>.None;

            K result = selector(maybe.Value);
            return result is null ? Maybe<K>.None : Maybe.Some(result);
        }

        /// <summary>
        ///     Executes a function based on the values of the Maybe.
        /// </summary>
        /// <param name="some">Function if there is a value</param>
        /// <param name="none">Function when no value is present in the values array</param>
        /// <typeparam name="U">Return Type</typeparam>
        /// <returns>Mutated value</returns>
        public static U Case<T,U>(this Maybe<T> maybe, Func<T, U> some, Func<U> none)
        {
            return maybe.HasValue
                       ? some(maybe.Value)
                       : none();
        }

        /// <summary>
        ///     Executes an action based on the values of the <see cref="Maybe{T}" />
        /// </summary>
        /// <param name="some">Action when there is a value present in the values array.</param>
        /// <param name="none">Action when there is no value in the values array</param>
        public static void Case<T>(this Maybe<T> maybe, Action<T> some, Action none)
        {
            if (maybe.HasValue)
                some(maybe.Value);
            else
                none();
        }



        /// <summary>
        ///     Executes an action when the values arrays is not empty.
        /// </summary>
        /// <param name="some">Action</param>
        public static void IfSome<T>(this Maybe<T> maybe, Action<T> some)
        {
            if (maybe.HasValue) some(maybe.Value);
        }


        /// <summary>
        ///     Checks to see if the HasValue property is true and returns the value.
        /// </summary>
        /// <param name="default">Default value of T</param>
        /// <returns>Value containing in the array or the default value of T</returns>
        public static T ValueOrDefault<T>(this Maybe<T> maybe,T @default) { return !maybe.HasNoValue ? @default : maybe.Value; }

        /// <summary>
        ///     Checks to see if the HasValue property is true and returns the value.
        /// </summary>
        /// <returns>Value containing in the array or the default value of T</returns>
        public static T ValueOrDefault<T>(this Maybe<T> maybe) { return maybe.HasNoValue ? default : maybe.Value; }

        /// <summary>
        ///     Throws an '<see cref="Exception" /> when HasValue evaluates to false.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T ValueOrThrow<T>(this Maybe<T> maybe, Exception e)
        {
            if (maybe.HasValue) return maybe.Value;
            throw e;
        }
        /// <summary>
        ///     Gets the first element of and <see cref="IEnumerable{T}" /> and returns it as a <see cref="Maybe" />
        /// </summary>
        /// <param name="self">
        ///     <see cref="IEnumerable{T}" />
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>First value of the <see cref="Maybe{T}" /> as a <see cref="Maybe" /></returns>
        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> self)
            where T : class
        {
            return self.FirstOrDefault().ToMaybe();
        }

        /// <summary>
        ///     Gets the first element of and <see cref="IEnumerable{T}" /> and returns it as a <see cref="Maybe{T}" />
        /// </summary>
        /// <param name="self">
        ///     <see cref="IEnumerable{T}" />
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns>First value of the <see cref="IEnumerable{T}" /> as a <see cref="Maybe{T}" /></returns>
        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T?> self)
            where T : struct
        {
            return self.FirstOrDefault().ToMaybe();
        }

        /// <summary>
        ///     Check's if the string is empty and returns a <see cref="Maybe{t}" />
        /// </summary>
        /// <param name="string">String value.</param>
        /// <returns><see cref="Maybe{T}.None" /> if @string is null or empty, <see cref="Maybe{T}.Some" /> if it's not</returns>
        public static Maybe<string> NoneIfEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string)
                       ? Maybe<string>.None
                       : Maybe.Some(@string);
        }

        /// <summary>
        ///     Verifies the value of self and returns a maybe.
        /// </summary>
        /// <param name="self">Self</param>
        /// <typeparam name="T">Type of self</typeparam>
        /// <returns><see cref="Maybe{T}.None" /> if self is null, <see cref="Maybe{T}.Some" /> if it's not</returns>
        public static Maybe<T> NoneIfNullOrDefault<T>(this T self)
            where T : class
        {
            return self == null ? Maybe<T>.None : Maybe<T>.Some(self);
        }

        /// <summary>
        ///     Transforms a value in an <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="Maybe{T}.None" /> if value is null, <see cref="Maybe{T}.Some" /> if it's not</returns>
        public static Maybe<T> ToMaybe<T>(this T value)
            where T : class
        {
            return value != null
                       ? Maybe.Some(value)
                       : Maybe<T>.None;
        }

        /// <summary>
        ///     Transforms a value in an <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="nullable">The value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="Maybe{T}.None" /> if value is null, <see cref="Maybe{T}.Some" /> if it's not</returns>
        public static Maybe<T> ToMaybe<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue
                       ? Maybe.Some(nullable.Value)
                       : Maybe<T>.None;
        }
    }
}
