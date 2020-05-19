using System;
using System.Net.Http;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable once ClassNeverInstantiated.Global
namespace Solari.Ganymede.Domain
{
    public sealed class GanymedeHttpResponse
    {
        public GanymedeHttpResponse(HttpResponseMessage responseMessage, DateTimeOffset startedUtc, DateTimeOffset endedUtc)
        {
            ResponseMessage = responseMessage;
            IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;
            StartedUtc = startedUtc;
            EndedUtc = endedUtc;
        }

        public HttpResponseMessage ResponseMessage { get; }
        public TimeSpan? Duration => EndedUtc - StartedUtc;
        public DateTimeOffset? EndedUtc { get; }
        public bool IsSuccessStatusCode { get; }
        public DateTimeOffset StartedUtc { get; }
    }
}