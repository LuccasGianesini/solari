using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Pipeline;

namespace Solari.Ganymede.Framework
{
    public class HttpRequestCoordinator
    {
        private readonly HttpClient _client;
        private readonly PipelineDescriptor _descriptor;

        public HttpRequestCoordinator(HttpClient client, PipelineDescriptor descriptor)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _descriptor = descriptor;
        }

        public async Task<GanymedeHttpResponse<T>> Send<T>(HttpRequestMessage requestMessage)
        {
            if (!CanMessageBeSent(requestMessage)) throw new MessageValidationException("Required headers are not present in the current HttpRequestMessage");

            DateTimeOffset started = DateTimeOffset.UtcNow;
            HttpResponseMessage response = await _client.SendAsync(requestMessage);
            DateTimeOffset ended = DateTimeOffset.UtcNow;

            return new GanymedeHttpResponse<T>(response, started, ended);
        }

        private bool CanMessageBeSent(HttpRequestMessage requestMessage)
        {
            return requestMessage.Headers.All(requestMessageHeader => _descriptor.Resource.RequiredHeaders.Contains(requestMessageHeader.Key));
            // return requestMessage.RequestUri != null && headersOk;
        }
    }
}