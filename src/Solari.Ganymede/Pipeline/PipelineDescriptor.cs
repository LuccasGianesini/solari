using System.Net.Http;
using Solari.Ganymede.Domain.Options;

namespace Solari.Ganymede.Pipeline
{
    public class PipelineDescriptor
    {
        public PipelineDescriptor(GanymedeRequestResource resource)
        {
            Resource = resource;
            ConfigureInstance(resource.Uri);
        }

        public PipelineDescriptor(string uri) { ConfigureInstance(uri); }

        public GanymedeRequestResource Resource { get; }
        public HttpRequestMessage RequestMessage { get; private set; }
        public string TargetUri { get; private set; }

        private void ConfigureInstance(string uri)
        {
            RequestMessage = new HttpRequestMessage();
            TargetUri = uri;
        }
    }
}