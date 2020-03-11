using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Domain.Options;

namespace Solari.Ganymede
{
    public class GanymedeRequest<TClientImplementation> : IGanymedeRequest<TClientImplementation>
    {
        public GanymedeRequestSettings RequestSettings { get; }

        private readonly IImmutableDictionary<string, GanymedeRequestResource> _resources;

        public GanymedeRequest(GanymedeRequestSettings requestSettings)
        {
            RequestSettings = requestSettings;
            _resources = BuildResourceDictionary(RequestSettings);
        }

        public GanymedeRequestResource GetResource(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName)) throw new ArgumentException("Value cannot be null or empty.", nameof(resourceName));
            if (!_resources.ContainsKey(resourceName))
                throw new RequestResourceNotFoundException($"The Resource {resourceName} does not exists in the current resource dictionary");

            return _resources.FirstOrDefault(a => a.Key == resourceName).Value;
        }

        private IImmutableDictionary<string, GanymedeRequestResource> BuildResourceDictionary(GanymedeRequestSettings requestSettings)
        {
            if (requestSettings == null)
                throw new NullGanymedeRequestSettingsException($"Unable to create resource dictionary because{nameof(requestSettings)} is null");

            return requestSettings.Resources.ToImmutableDictionary(pair => pair.Name, pair => pair);
        }
    }
}