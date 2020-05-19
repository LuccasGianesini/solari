using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Solari.Sol.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Serializes an object into a Json, then gets the bytes from the resulting Json.
        /// </summary>
        /// <param name="object">The object</param>
        /// <returns>Array of bytes</returns>
        public static byte[] ToJsonByteArray(this object @object)
        {
            return @object == null ? new byte[0] : Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@object));
        }

        /// <summary>
        ///     Get the string from a byte array created from a Json string, and deserializes the resulting Json to an object of type T.
        /// </summary>
        /// <param name="bytes">The array of bytes</param>
        /// <typeparam name="T">Type of T</typeparam>
        /// <returns>T</returns>
        public static T JsonByteArrayToObject<T>(this byte[] bytes)
        {
            return bytes.Length == 0 ? default : JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }


        /// <summary>
        ///     Transforms an object into a <see cref="KeyValuePair{TKey,TValue}" />.
        /// </summary>
        /// <param name="object">The object</param>
        /// <returns>
        ///     <see cref="IDictionary{TKey,TValue}" />
        /// </returns>
        public static IDictionary<string, string> ToKeyValue(this object @object)
        {
            while (true)
            {
                if (@object == null) return null;

                if (!(@object is JToken token))
                {
                    @object = JObject.FromObject(@object);

                    continue;
                }

                if (token.HasValues)
                {
                    var contentData = new Dictionary<string, string>();

                    return token.Children()
                                .ToList()
                                .Select(child => child.ToKeyValue())
                                .Where(childContent => childContent != null)
                                .Aggregate(contentData, (current, childContent) => current.Concat(childContent)
                                                                                          .ToDictionary(k => k.Key, v => v.Value));
                }

                var jValue = token as JValue;

                if (jValue?.Value == null) return null;

                string value = jValue?.Type == JTokenType.Date
                                   ? jValue?.ToString("o", CultureInfo.InvariantCulture)
                                   : jValue?.ToString(CultureInfo.InvariantCulture);

                return new Dictionary<string, string> {{token.Path, value}};
            }
        }
    }
}