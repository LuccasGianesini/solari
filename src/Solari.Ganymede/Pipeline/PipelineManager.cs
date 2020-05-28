using System;
using System.Net.Http;
using System.Threading.Tasks;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;

// ReSharper disable MemberCanBePrivate.Global

namespace Solari.Ganymede.Pipeline
{
    public class PipelineManager
    {
        private readonly HttpClient _client;
        private PipelineContext _currentContext;
        private bool _messageConfigured;
        private bool _uriConfigured;

        public PipelineManager(HttpClient client, GanymedeRequestResource resource)
        {
            _client = client;
            _currentContext = new PipelineContext(resource);
        }

        public PipelineManager(HttpClient client, string uri)
        {
            _client = client;
            _currentContext = new PipelineContext(uri);
        }

        /// <summary>
        ///     Configure the basics of a <see cref="HttpRequestMessage" /> like <see cref="HttpMethod" /> or <see cref="HttpCompletionOption" />
        /// </summary>
        /// <param name="messageStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureHttpMessage(Func<MessageStage, PipelineContext> messageStage)
        {
            _currentContext = messageStage(new MessageStage(_currentContext));
            _messageConfigured = true;

            return this;
        }

        /// <summary>
        ///     Configure the body content of the request, as well as the serializers to be used.
        /// </summary>
        /// <param name="contentStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureRequestContent(Func<ContentStage, PipelineContext> contentStage)
        {
            _currentContext = contentStage(new ContentStage(_currentContext));

            return this;
        }

        /// <summary>
        ///     Configures the headers of the request.
        /// </summary>
        /// <param name="headerStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureRequestHeaders(Func<HeaderStage, PipelineContext> headerStage)
        {
            _currentContext = headerStage(new HeaderStage(_currentContext));

            return this;
        }

        /// <summary>
        ///     Configure the Uri of the request.
        /// </summary>
        /// <param name="uriStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureRequestUri(Func<UriStage, PipelineContext> uriStage)
        {
            _currentContext = uriStage(new UriStage(_currentContext));
            _uriConfigured = true;

            return this;
        }

        public ValueTask<GanymedeHttpResponse> Send()
        {
            _executeDefaultActions();
            return new HttpRequestCoordinator(_client, _currentContext).Send(_currentContext.RequestMessage);
        }

        private void _executeDefaultActions()
        {
            if (!_uriConfigured) ConfigureRequestUri(uri => uri);

            if (!_messageConfigured) ConfigureHttpMessage(message => message.UseGanymedeEndpointOptions());
        }
    }
}