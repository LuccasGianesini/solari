using System.Collections.Generic;
using System.Net.Http;

// ReSharper disable CollectionNeverUpdated.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Solari.Ganymede.Domain.Options
{
    public class GanymedeRequestSpecification
    {
        public string Name { get; set; }

        /// <summary>
        ///     <see cref="HttpClient" /> base address.
        /// </summary>
        public string BaseAddress { get; set; }

        public List<GanymedeRequestHeader> DefaultRequestHeaders { get; set; } = new List<GanymedeRequestHeader>(2);
        public List<GanymedeRequestResource> Resources { get; set; } = new List<GanymedeRequestResource>(5);
        public long MaxResponseContentBufferSize { get; set; }
        public string Timeout { get; set; }
    }
}