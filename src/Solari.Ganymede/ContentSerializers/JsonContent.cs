using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;
using Solari.Sol.Abstractions.Utils;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Solari.Ganymede.ContentSerializers
{
    public class JsonContentDeserializer : IContentDeserializer
    {
        private readonly JsonSerializerSettings _settings;

        private readonly JsonSerializer _serializer;
        public JsonContentDeserializer(JsonSerializerSettings settings = null)
        {
            _settings = settings;
            _serializer = JsonSerializer.Create(_settings);
        }
        public async Task<Maybe<TModel>> Deserialize<TModel>(HttpContent content)
        {
            await using Stream stream = await content.ReadAsStreamAsync().DefaultAwait();
            using var reader = new StreamReader(stream);
            using JsonReader json = new JsonTextReader(reader);
            var model = _serializer.Deserialize<TModel>(json);
            return model is null ? Maybe<TModel>.None : Maybe<TModel>.Some(model);
        }
    }

    public class JsonContentSerializer : IContentSerializer
    {
        private readonly JsonSerializer _serializer;

        public JsonContentSerializer(JsonSerializerSettings jsonSerializerSettings = null)
        {
            _serializer = _createJsonSerializer(jsonSerializerSettings);
        }

        public HttpContent Serialize(object content, string contentType, Encoding encoding = null)
        {
            if (content == null) return new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain);

            using var writer = new StringWriter();
            using var jsonWriter = new JsonTextWriter(writer);
            _serializer.Serialize(jsonWriter, content);
            jsonWriter.Flush();

            return new StringContent(writer.ToString(), encoding ?? Encoding.UTF8, contentType);
        }

        private static JsonSerializer _createJsonSerializer(JsonSerializerSettings jsonSerializerSettings)
        {
            return jsonSerializerSettings == null ? JsonSerializer.CreateDefault() : JsonSerializer.Create(jsonSerializerSettings);
        }
    }
}
