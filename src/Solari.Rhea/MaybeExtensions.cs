using System.Collections.Generic;
using System.Linq;

namespace Solari.Rhea
{
    public static class MaybeExtensions
    {
        /// <summary>
        /// Gets the first element of and <see cref="IEnumerable{T}"/> and returns it as a <see cref="Maybe{T}"/>
        /// </summary>
        /// <param name="self"><see cref="IEnumerable{T}"/></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>First value of the <see cref="IEnumerable{T}"/> as a <see cref="Maybe{T}"/></returns>
        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> self)
            where T : class
        {
            return self.FirstOrDefault().ToMaybe();
        }

        /// <summary>
        /// Gets the first element of and <see cref="IEnumerable{T}"/> and returns it as a <see cref="Maybe{T}"/>
        /// </summary>
        /// <param name="self"><see cref="IEnumerable{T}"/></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>First value of the <see cref="IEnumerable{T}"/> as a <see cref="Maybe{T}"/></returns>
        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T?> self)
            where T : struct
        {
            return self.FirstOrDefault().ToMaybe();
        }

        /// <summary>
        /// Check's if the string is empty and returns a <see cref="Maybe{t}"/>
        /// </summary>
        /// <param name="string">String value.</param>
        /// <returns><see cref="Maybe{T}.None"/> if @string is null or empty, <see cref="Maybe{T}.Some"/> if it's not</returns>
        public static Maybe<string> NoneIfEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string)
                       ? Maybe<string>.None
                       : Maybe.Some(@string);
        }

        /// <summary>
        /// Verifies the value of self and returns a maybe.
        /// </summary>
        /// <param name="self">Self</param>
        /// <typeparam name="T">Type of self</typeparam>
        /// <returns><see cref="Maybe{T}.None"/> if self is null, <see cref="Maybe{T}.Some"/> if it's not</returns>
        public static Maybe<T> NoneIfNullOrDefault<T>(this T self)
            where T : class
        {
            return self == null ? Maybe<T>.None : Maybe<T>.Some(self);
        }

        /// <summary>
        /// Transforms a value in an <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="Maybe{T}.None"/> if value is null, <see cref="Maybe{T}.Some"/> if it's not</returns>
        public static Maybe<T> ToMaybe<T>(this T value)
            where T : class
        {
            return value != null
                       ? Maybe.Some(value)
                       : Maybe<T>.None;
        }

        /// <summary>
        /// Transforms a value in an <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="nullable">The value</param>
        /// <typeparam name="T">The value type</typeparam>
        /// <returns><see cref="Maybe{T}.None"/> if value is null, <see cref="Maybe{T}.Some"/> if it's not</returns>
        public static Maybe<T> ToMaybe<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue
                       ? Maybe.Some(nullable.Value)
                       : Maybe<T>.None;
        }
    }
}