using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Solari.Ganymede.Framework;
using Solari.Sol.Extensions;
using Solari.Sol.Utils;

namespace Solari.Ganymede.ContentSerializers
{
    public class UrlEncodedDeserializer : IContentDeserializer
    {
        public async Task<Maybe<TModel>> Deserialize<TModel>(HttpContent content)
        {
            if (content == null) return Maybe<TModel>.None;

            string value = await StreamHelper.StreamToStringAsync(await content.ReadAsStreamAsync());
            var model = UrlDecoder.Decode(value).ToObject<TModel>();

            return model != null ? Maybe<TModel>.Some(model) : Maybe<TModel>.None;
        }
    }

    public class UrlEncodedSerializer : IContentSerializer
    {
        public HttpContent Serialize(object content, string contentType, Encoding encoding = null)
        {
            if (content == null) return new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>());

            return content.GetType() == typeof(IEnumerable<KeyValuePair<string, string>>)
                       ? new FormUrlEncodedContent(content as IEnumerable<KeyValuePair<string, string>>)
                       : new FormUrlEncodedContent(content.ToKeyValue());
        }
    }
}