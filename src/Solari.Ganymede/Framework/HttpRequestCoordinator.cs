using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Pipeline;

namespace Solari.Ganymede.Framework
{
    public class HttpRequestCoordinator
    {
        private readonly HttpClient _client;
        private readonly PipelineContext _context;

        public HttpRequestCoordinator(HttpClient client, PipelineContext context)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context;
        }

        public async ValueTask<GanymedeHttpResponse> Send(HttpRequestMessage requestMessage)
        {
            if (!CanMessageBeSent(requestMessage)) throw new GanymedeMessageValidationException("Required headers are not present in the current HttpRequestMessage");

            DateTimeOffset started = DateTimeOffset.UtcNow;
            HttpResponseMessage response = await _client.SendAsync(requestMessage).ConfigureAwait(false);
            DateTimeOffset ended = DateTimeOffset.UtcNow;

            return new GanymedeHttpResponse(response, started, ended);
        }

        private bool CanMessageBeSent(HttpRequestMessage requestMessage)
        {
            if (!_context.Resource.RequiredHeaders.Any())
                return true;
            return requestMessage.Headers.All(requestMessageHeader => _context.Resource.RequiredHeaders.Contains(requestMessageHeader.Key));
        }
    }
}
