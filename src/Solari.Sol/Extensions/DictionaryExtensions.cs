using System;
using System.Collections.Generic;

namespace Solari.Sol.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Tries to add a value into a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value Type</typeparam>
        /// <returns>True if the value was added. False if the dictionary already contains the provided key</returns>
        /// <exception cref="ArgumentNullException">If the key is null</exception>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (dictionary.ContainsKey(key)) return false;

            dictionary.Add(key, value);

            return true;
        }

        /// <summary>
        /// Tries to get a value of the dictionary. If the value is in the dictionary, the value is returned.
        /// If it is the value from valueFactory is added to the dictionary. 
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">Key</param>
        /// <param name="valueFactory">Value factory function</param>
        /// <param name="val">Value that was added or that was present in the dictionary</param>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <returns>Always true</returns>
        /// <exception cref="ArgumentNullException">If key is null</exception>
        public static bool TryGetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory, out TValue val)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (dictionary.TryGetValue(key, out TValue value))
            {
                val = value;

                return true;
            }

            value = valueFactory(key);
            dictionary.Add(key, value);
            val = value;

            return true;
        }

        /// <summary>
        /// Tries to remove a value from the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <typeparam name="TKey">key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <returns>True if the value exists and if it was removed</returns>
        /// <exception cref="ArgumentNullException">If key is null</exception>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return dictionary.TryGetValue(key, out value) && dictionary.Remove(key);
        }
        
        /// <summary>
        /// Tries to update a value in the dictionary. 
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">key</param>
        /// <param name="newValue">The value to be added</param>
        /// <typeparam name="TKey">key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <returns>False if the value is not in the dictionary.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (!dictionary.TryGetValue(key, out TValue value)) return false;

            // if (!Equals(value, newValue)) return false;

            dictionary[key] = newValue;

            return true;
        }
    }
}