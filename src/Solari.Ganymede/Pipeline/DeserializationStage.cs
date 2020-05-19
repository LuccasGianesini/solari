using System.Threading.Tasks;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Extensions;
using Solari.Sol.Utils;

namespace Solari.Ganymede.Pipeline
{
    public static class DeserializationStage
    {
        public static async ValueTask<string> AsString(this ValueTask<GanymedeHttpResponse> httpResponse)
        {
            GanymedeHttpResponse response = await httpResponse;
            return await response.AsString();
        }

        public static async ValueTask<string> AsString(this GanymedeHttpResponse httpResponse)
        {
            return await httpResponse.ResponseMessage.Content.ReadAsStringAsync();
        }

        public static async ValueTask<T> AsModel<T>(this ValueTask<GanymedeHttpResponse> httpResponse)
        {
            GanymedeHttpResponse response = await httpResponse;
            return await response.AsModel<T>();
        }

        public static async ValueTask<T> AsModel<T>(this GanymedeHttpResponse httpResponse)
        {
            Maybe<T> deserialized = await httpResponse
                                          .ResponseMessage
                                          .RequestMessage.GetContentDeserializer()
                                          .Deserialize<T>(httpResponse.ResponseMessage.Content);

            return deserialized.Value;
        }
    }
}