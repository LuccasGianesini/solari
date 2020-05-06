using System;
using System.Net.Http;

namespace Solari.Ganymede
{
    public abstract class GanymedeClient<TClientImplementation>
    {
        private readonly IGanymedeRequest<TClientImplementation> _request;

        protected readonly HttpClient HttpClient;

        protected GanymedeClient(HttpClient httpClient, IGanymedeRequest<TClientImplementation> request)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _request = request;
        }

        protected Requester<TClientImplementation> NewRequest => new Requester<TClientImplementation>(HttpClient, _request);
    }
}