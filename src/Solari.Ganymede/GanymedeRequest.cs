using System;
using System.Collections.Immutable;
using System.Linq;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Domain.Options;

namespace Solari.Ganymede
{
    public class GanymedeRequest<TClientImplementation> : IGanymedeRequest<TClientImplementation>
    {
        private readonly IImmutableDictionary<string, GanymedeRequestResource> _resources;

        public GanymedeRequest(GanymedeRequestSpecification requestSpecification)
        {
            RequestSpecification = requestSpecification;
            _resources = BuildResourceDictionary(RequestSpecification);
        }

        public GanymedeRequestSpecification RequestSpecification { get; }

        public GanymedeRequestResource GetResource(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new GanymedeException("Unable to search a resource with the given term.", new
                                                ArgumentNullException("Value cannot be null or empty.", nameof(resourceName)));
            if (!_resources.ContainsKey(resourceName))
                throw new GanymedeException("The resource dictionary does not contains the given key.",
                                                new RequestResourceNotFoundException($"Search key:{resourceName}"));
                    

            return _resources.FirstOrDefault(a => a.Key == resourceName).Value;
        }

        private IImmutableDictionary<string, GanymedeRequestResource> BuildResourceDictionary(
            GanymedeRequestSpecification requestSpecification)
        {
            if (requestSpecification == null)
                throw new GanymedeException($"The request specification for {nameof(TClientImplementation)} is null.");
            
            return requestSpecification.Resources.ToImmutableDictionary(pair => pair.Name, pair => pair);
        }
    }
}