using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Solari.Ganymede.Framework;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;
using Solari.Sol.Abstractions.Utils;

namespace Solari.Ganymede.ContentSerializers
{
    public class UrlEncodedDeserializer : IContentDeserializer
    {
        public async Task<Maybe<TModel>> Deserialize<TModel>(HttpContent content)
        {
            if (content == null) return Maybe<TModel>.None;
            await using Stream stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            using var sr = new StreamReader(stream);
            var model = UrlDecoder.Decode(await sr.ReadToEndAsync().ConfigureAwait(false)).ToObject<TModel>();
            return model is null ? Maybe<TModel>.None : Maybe<TModel>.Some(model);
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
