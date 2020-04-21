using System;
using System.Net.Http;
using System.Threading.Tasks;
using Elastic.CommonSchema;
using Solari.Deimos.CorrelationId;
using Solari.Ganymede.ContentSerializers;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Extensions;
using Solari.Ganymede.Framework;
using Solari.Rhea;
using Solari.Sol;
using Solari.Vanth;
using Solari.Vanth.Builders;

// ReSharper disable MemberCanBePrivate.Global

namespace Solari.Ganymede.Pipeline
{
    public class PipelineManager
    {
        private readonly HttpClient _client;
        private PipelineDescriptor _currentDescriptor;
        private bool _messageConfigured;
        private bool _uriConfigured;

        public PipelineManager(HttpClient client, GanymedeRequestResource resource)
        {
            _client = client;
            _currentDescriptor = new PipelineDescriptor(resource);
        }

        public PipelineManager(HttpClient client, string uri)
        {
            _client = client;
            _currentDescriptor = new PipelineDescriptor(uri);
        }

        /// <summary>
        /// Configure the basics of a <see cref="HttpRequestMessage"/> like <see cref="HttpMethod"/> or <see cref="HttpCompletionOption"/>
        /// </summary>
        /// <param name="messageStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureHttpMessage(Func<MessageStage, PipelineDescriptor> messageStage)
        {
            _currentDescriptor = messageStage(new MessageStage(_currentDescriptor));
            _messageConfigured = true;

            return this;
        }

        /// <summary>
        /// Configure the body content of the request, as well as the serializers to be used. 
        /// </summary>
        /// <param name="contentStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureRequestContent(Func<ContentStage, PipelineDescriptor> contentStage)
        {
            _currentDescriptor = contentStage(new ContentStage(_currentDescriptor));

            return this;
        }

        /// <summary>
        /// Configures the headers of the request.
        /// </summary>
        /// <param name="headerStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureRequestHeaders(Func<HeaderStage, PipelineDescriptor> headerStage)
        {
            _currentDescriptor = headerStage(new HeaderStage(_currentDescriptor));

            return this;
        }

        /// <summary>
        /// Configure the Uri of the request.
        /// </summary>
        /// <param name="uriStage">Function to be invoked</param>
        /// <returns></returns>
        public PipelineManager ConfigureRequestUri(Func<UriStage, PipelineDescriptor> uriStage)
        {
            _currentDescriptor = uriStage(new UriStage(_currentDescriptor));
            _uriConfigured = true;

            return this;
        }

        public Task<GanymedeHttpResponse> Send()
        {
            _executeDefaultActions();
            return new HttpRequestCoordinator(_client, _currentDescriptor).Send(_currentDescriptor.RequestMessage);
        }

        private void _executeDefaultActions()
        {
            if (!_uriConfigured) ConfigureRequestUri(uri => uri);

            if (!_messageConfigured) ConfigureHttpMessage(message => message.UseGanymedeEndpointOptions());
        }

      
    }
}