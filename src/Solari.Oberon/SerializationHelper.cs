using Newtonsoft.Json;

namespace Solari.Oberon
{
    public static class SerializationHelper
    {
        public static T Deserialize<T>(string json) { return string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json); }

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