using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Solari.Ganymede.Domain;
using Solari.Sol.Utils;

namespace Solari.Ganymede.ContentSerializers
{
    public class XmlContentDeserializer : IContentDeserializer
    {
        public async Task<Maybe<TModel>> Deserialize<TModel>(HttpContent content)
        {
            if (content == null) return Maybe<TModel>.None;

            var serializer = new XmlSerializer(typeof(TModel));

            using var sr = new StreamReader(await content.ReadAsStreamAsync());
            var model = (TModel) serializer.Deserialize(sr);

            return model != null ? Maybe<TModel>.Some(model) : Maybe<TModel>.None;
        }
    }

    public class XmlContentSerializer : IContentSerializer
    {
        public HttpContent Serialize(object content, string contentType, Encoding encoding = null)
        {
            if (content == null) return new StringContent(string.Empty);

            if (string.IsNullOrEmpty(contentType)) contentType = MediaTypeNames.Application.Xml;
            var xmlSerializer = new XmlSerializer(content.GetType());

            using var writer = new StringWriter();
            using var xmlWriter = XmlWriter.Create(writer);
            xmlSerializer.Serialize(writer, content);

            return new StringContent(writer.ToString(), encoding ?? Encoding.UTF8, contentType);
        }
    }
}