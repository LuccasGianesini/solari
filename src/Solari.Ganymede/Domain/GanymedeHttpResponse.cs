using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
namespace Solari.Ganymede.Domain
{
    public sealed class GanymedeHttpResponse<TSerializedResponseBody>
    {
        public GanymedeHttpResponse(HttpResponseMessage responseMessage, DateTimeOffset startedUtc, DateTimeOffset endedUtc)
        {
            ResponseMessage = responseMessage ?? throw new ArgumentNullException(nameof(responseMessage));
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;
            StartedUtc = startedUtc;
            EndedUtc = endedUtc;
        }

        public TSerializedResponseBody DeserializedContent { get; private set; }

        public TimeSpan? Duration => EndedUtc - StartedUtc;
        public DateTimeOffset? EndedUtc { get; }
        public bool IsSuccessStatusCode { get; }
        
        public string StringOfRequestBody { get; set; }
        public HttpResponseMessage ResponseMessage { get; }
        public string StringOfResponseBody { get; set; }
        public DateTimeOffset StartedUtc { get; }

        public void AddDeserializedContent(TSerializedResponseBody responseBody) => DeserializedContent = responseBody;



        public async Task StringifyResponseBody()
        {
            StringOfResponseBody = await ResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task StringifyRequestBody()
        {
            StringOfRequestBody = await ResponseMessage.RequestMessage.Content.ReadAsStringAsync();
        }
    }
}