using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Solari.Ganymede.Framework
{
    public static class StreamHelper
    {
        
        public static T DeserializeJsonFromStream<T>(Stream stream, string rootNode)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);
            var js = new JsonSerializer();

            return js.Deserialize<T>(jtr);
        }

        public static async Task<string> StreamToStringAsync(Stream stream)
        {
            if (stream == null) return null;

            using var sr = new StreamReader(stream);
            return await sr.ReadToEndAsync();
        }
    }
}