using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Sol.Utils
{
    /// <summary>
    /// Base on Eric Andres and Pluralsight code. Check him out at https://www.pluralsight.com/tech-blog/maybe/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Maybe<T>
    {
        private readonly IEnumerable<T> _values;

        private Maybe(IEnumerable<T> values)
        {
            _values = values;
        }

        /// <summary>
        /// Creates a <see cref="Maybe{T}"/> with no value added to the array. HasValue evaluates to false.
        /// </summary>
        public static Maybe<T> None => new Maybe<T>(new T[0]);

        /// <summary>
        /// Checks if the Values array contains any element.
        /// </summary>
        public bool HasValue => _values != null && _values.Any();

        /// <summary>
        /// The value of the <see cref="Maybe{T}"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">If has value evaluates to false.</exception>
        public T Value
        {
            get
            {
                if (!HasValue) throw new InvalidOperationException("Maybe does not have a value");

                return _values.Single();
            }
        }

        /// <summary>
        /// Creates a <see cref="Maybe{T}"/> with a value added to the values array. HasValue evaluates to true.
        /// </summary>
        /// <param name="value"></param>
        /// <returns><see cref="Maybe{T}"/>. T being the value type</returns>
        /// <exception cref="InvalidOperationException">If provided value is equals to null</exception>
        public static Maybe<T> Some(T value)
        {
            if (value == null) throw new InvalidOperationException();

            return new Maybe<T>(new[] {value});
        }

        /// <summary>
        /// Executes a function based on the values of the Maybe.
        /// </summary>
        /// <param name="some">Function if there is a value</param>
        /// <param name="none">Function when no value is present in the values arrary</param>
        /// <typeparam name="U">Return Type</typeparam>
        /// <returns>Mutated value</returns>
        public U Case<U>(Func<T, U> some, Func<U> none)
        {
            return HasValue
                       ? some(Value)
                       : none();
        }

        /// <summary>
        /// Executes an action based on the values of the <see cref="Maybe{T}"/>
        /// </summary>
        /// <param name="some">Action when there is a value present in the values array.</param>
        /// <param name="none">Action when there is no value in the values array</param>
        public void Case(Action<T> some, Action none)
        {
            if (HasValue)
                some(Value);
            else
                none();
        }
        
        /// <summary>
        /// Executes an action when the values arrays is not empty.
        /// </summary>
        /// <param name="some">Action</param>
        public void IfSome(Action<T> some)
        {
            if (HasValue) some(Value);
        }

        /// <summary>
        /// Checks to see if the HasValue property is true and returns the value.
        /// </summary>
        /// <param name="default">Default value of T</param>
        /// <returns>Value containing in the array or the default value of T</returns>
        public T ValueOrDefault(T @default)
        {
            return !HasValue ? @default : _values.Single();
        }
        /// <summary>
        /// Checks to see if the HasValue property is true and returns the value.
        /// </summary>
        /// <returns>Value containing in the array or the default value of T</returns>
        public T ValueOrDefault()
        {
            return !HasValue ? default(T) : _values.Single();
        }

        /// <summary>
        /// Throws an '<see cref="Exception"/> when HasValue evaluates to false.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T ValueOrThrow(Exception e)
        {
            if (HasValue) return Value;

            throw e;
        }
    }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value)
        {
            return Maybe<T>.Some(value);
        }
    }
}