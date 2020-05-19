using System;
using System.Linq.Expressions;

namespace Solari.Sol.Utils
{
    /// <summary>
    ///     Creates instances of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type with a parameter-less constructor.</typeparam>
    public static class ObjectFactory<T>
        where T : new()
    {
        private static readonly Func<T> CreateInstanceFunc =
            Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

        /// <summary>
        ///     Creates an instance of type <typeparamref name="T" /> by calling it's parameter-less constructor.
        /// </summary>
        /// <returns>An instance of type <typeparamref name="T" />.</returns>
        public static T CreateInstance() { return CreateInstanceFunc(); }
    }
}