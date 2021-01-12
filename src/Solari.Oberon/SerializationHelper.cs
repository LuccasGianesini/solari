using System;
using Newtonsoft.Json;

namespace Solari.Oberon
{
    internal static class SerializationHelper
    {
        public static T Deserialize<T>(string json) => string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json);

        public static bool TrySerializeObject<T>(T @object, out string json)
        {
            if (@object == null)
            {
                json = "";
                return false;
            }

            try
            {
                json = JsonConvert.SerializeObject(@object);
                return true;
            }
            catch (JsonSerializationException)
            {
                json = "";
                return false;
            }
        }
    }
}
