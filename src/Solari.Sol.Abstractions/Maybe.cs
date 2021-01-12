using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Solari.Sol.Abstractions
{
    /// <summary>
    ///     Base on Eric Andres and Pluralsight code. Check him out at https://www.pluralsight.com/tech-blog/maybe/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public struct Maybe<T> : ISerializable
    {
        private readonly IEnumerable<T> _values;
        private Maybe(IEnumerable<T> values) { _values = values; }

        public Maybe(SerializationInfo info, StreamingContext context)
        {
            _values = new[] {(T) info.GetValue(nameof(Value), typeof(T))};
        }
        private Maybe(T value)
        {
            _values = new[] {value};
        }
        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> with a value added to the values array. HasValue evaluates to true.
        /// </summary>
        /// <param name="value"></param>
        /// <returns><see cref="Maybe{T}" />. T being the value type</returns>
        /// <exception cref="InvalidOperationException">If provided value is equals to null</exception>
        public static Maybe<T> Some(T value)
        {
            Check.ThrowIfNull(value, nameof(value), new InvalidOperationException("Cannot create a maybe with a null value. Call Maybe<T>.None()."));

            return new Maybe<T>(new[] {value});
        }

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> with no value added to the array. HasValue evaluates to false.
        /// </summary>
        public static Maybe<T> None => new(new T[0]);

        /// <summary>
        ///     Checks if the Values array contains any element.
        /// </summary>
        public bool HasValue => _values != null;
        public bool HasNoValue => !HasValue;

        /// <summary>
        ///     The value of the <see cref="Maybe{T}" />
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

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (value is Maybe<T>)
                return maybe.Equals(value);

            if (maybe.HasNoValue)
                return false;

            return maybe.Value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(Maybe<T>))
            {
                if (obj is T)
                {
                    obj = new Maybe<T>((T)obj);
                }

                if (!(obj is Maybe<T>))
                    return false;
            }

            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            if (HasNoValue)
                return 0;

            return Value.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Value), Value, typeof(T));
        }

    }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value) { return Maybe<T>.Some(value); }
    }
}
