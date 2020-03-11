﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Net.Http;
using Microsoft.Extensions.Options;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;
using Solari.Ganymede.Pipeline;

namespace Solari.Ganymede
{
    public abstract class GanymedeClient<TClientImplementation>
    {
        
        protected readonly HttpClient HttpClient;
        private readonly IGanymedeRequest<TClientImplementation> _request;

        protected GanymedeClient(HttpClient httpClient, IGanymedeRequest<TClientImplementation> request)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _request = request;
        }

        protected Requester<TClientImplementation> NewRequest => new Requester<TClientImplementation>(HttpClient, _request);

    }
}