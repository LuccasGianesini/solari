using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Solari.Ganymede.ContentSerializers;
using Solari.Ganymede.Extensions;
using Solari.Sol.Utils;

namespace Solari.Ganymede.Framework
{
    public static class SerializerManager
    {
        public static async Task<Maybe<TModel>> SendToDeserializer<TModel>(HttpResponseMessage responseMessage, IContentDeserializer contentSerializer)
        {
            return await contentSerializer.Deserialize<TModel>(responseMessage.Content);
        }

        public static async Task<Maybe<TModel>> SendToDeserializer<TModel>(HttpResponseMessage responseMessage)
        {
            return await SendToDeserializer<TModel>(responseMessage, responseMessage.RequestMessage.GetContentDeserializer());
        }


        public static HttpContent SendToSerializer(object content, HttpRequestMessage requestMessage, string contentType, Encoding encoding)
        {
            return requestMessage.GetContentSerializer().Serialize(content, contentType, encoding);
        }
    }
}