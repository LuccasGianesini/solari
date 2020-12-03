using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Solari.Sol.Abstractions
{
    public class SolariJsonSerializer
    {
        public static SolariJsonSerializer New => new SolariJsonSerializer();

        private readonly JsonSerializerSettings _serializerSettings;
        private JsonSerializer _serializer;

        public SolariJsonSerializer()
        {
            _serializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
            _serializer = JsonSerializer.Create(_serializerSettings);
        }

        public SolariJsonSerializer(JsonSerializerSettings settings)
        {
            Check.ThrowIfNull(settings, nameof(JsonSerializerSettings));
            _serializerSettings = settings;
            _serializer = JsonSerializer.Create(settings);
        }

        public string SerializeToJson<T>(T value)
        {
            Check.ThrowIfNull(value, nameof(value));
            using var writer = new StringWriter();
            using var jsonWriter = new JsonTextWriter(writer);
            _serializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
            return writer.ToString();
        }


        public T DeserializeFromString<T>(string json)
        {
            Check.ThrowIfNullOrEmpty(json, nameof(json));
            using TextReader text = new StringReader(json);
            using JsonReader reader = new JsonTextReader(text);
            return _serializer.Deserialize<T>(reader);
        }

        public T DeserializeFromStream<T>(Stream stream)
        {
            Check.ThrowIfNull(stream, nameof(Stream));
            using var reader = new StreamReader(stream);
            using JsonReader json = new JsonTextReader(reader);
            return _serializer.Deserialize<T>(json);
        }
    }
}
