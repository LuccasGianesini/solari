using System.Threading.Tasks;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Extensions;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Utils;

namespace Solari.Ganymede.Pipeline
{
    public static class DeserializationStage
    {
        public static async ValueTask<string> AsString(this ValueTask<GanymedeHttpResponse> httpResponse)
        {
            Maybe<string> maybe = await httpResponse.AsMaybeOfString();
            return maybe.Value;
        }

        public static async ValueTask<string> AsString(this GanymedeHttpResponse httpResponse)
        {
            return await httpResponse.ResponseMessage.Content.ReadAsStringAsync();
        }

        public static async ValueTask<Maybe<string>> AsMaybeOfString(this GanymedeHttpResponse httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                return Maybe<string>.Some(await httpResponse.AsString());
            }

            return Maybe<string>.None;
        }

        public static async ValueTask<Maybe<string>> AsMaybeOfString(this ValueTask<GanymedeHttpResponse> httpResponse)
        {
            GanymedeHttpResponse response = await httpResponse;
            return await response.AsMaybeOfString();
        }

        public static async ValueTask<Maybe<T>> AsMaybeOf<T>(this ValueTask<GanymedeHttpResponse> httpResponse)
        {
            GanymedeHttpResponse response = await httpResponse;
            return await response.AsMaybeOf<T>();
        }
        public static async ValueTask<Maybe<T>> AsMaybeOf<T>(this GanymedeHttpResponse httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse
                             .ResponseMessage
                             .RequestMessage.GetContentDeserializer()
                             .Deserialize<T>(httpResponse.ResponseMessage.Content);

            }

            return Maybe<T>.None;
        }

        public static async ValueTask<T> As<T>(this ValueTask<GanymedeHttpResponse> httpResponse)
        {
            GanymedeHttpResponse response = await httpResponse;
            return await response.As<T>();
        }

        public static async ValueTask<T> As<T>(this GanymedeHttpResponse httpResponse)
        {
            Maybe<T> deserialized = await httpResponse.ResponseMessage
                                                      .RequestMessage.GetContentDeserializer()
                                                      .Deserialize<T>(httpResponse.ResponseMessage.Content);
            return deserialized.Value;
        }
    }
}
