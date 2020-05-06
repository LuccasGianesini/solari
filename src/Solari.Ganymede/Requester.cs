using System;
using System.Net.Http;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Pipeline;

namespace Solari.Ganymede
{
    public class Requester<TClientImplementation>
    {
        private readonly HttpClient _httpClient;
        private readonly IGanymedeRequest<TClientImplementation> _request;


        public Requester(HttpClient httpClient, IGanymedeRequest<TClientImplementation> request)
        {
            _httpClient = httpClient;
            _request = request;
        }


        public PipelineManager ForResource(string resourceName) { return new PipelineManager(_httpClient, _request.GetResource(resourceName)); }

        public PipelineManager ForResource(GanymedeRequestResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            return new PipelineManager(_httpClient, resource);
        }

        public PipelineManager ForUri(string uri)
        {
            if (string.IsNullOrEmpty(uri)) throw new ArgumentException("Value cannot be null or empty.", nameof(uri));
            return new PipelineManager(_httpClient, uri);
        }
    }
}