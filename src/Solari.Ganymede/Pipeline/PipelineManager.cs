using System;
using System.Net.Http;
using System.Threading.Tasks;
using Solari.Ganymede.ContentSerializers;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Extensions;
using Solari.Ganymede.Framework;
using Solari.Rhea;
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

        public async Task<CommonResponse<GanymedeHttpResponse<Null>>> Send(bool stringifyResponseContent = false, bool stringifyRequestContent = false)
            => await ExecuteRequest<Null>(stringifyResponseContent, stringifyRequestContent);


        public async Task<CommonResponse<GanymedeHttpResponse<T>>> Send<T>(bool stringifyResponseContent = false, bool stringifyRequestContent = false)
        {
            CommonResponse<GanymedeHttpResponse<T>> commonResponse = await ExecuteRequest<T>(stringifyResponseContent, stringifyRequestContent);

            if (commonResponse.HasErrors) return commonResponse;

            IContentDeserializer deserializer = commonResponse.Result.ResponseMessage.RequestMessage.GetContentDeserializer();

            try
            {
                Maybe<T> maybe = await deserializer.Deserialize<T>(commonResponse.Result.ResponseMessage.Content);
                if (maybe.HasValue)
                    commonResponse.Result.AddDeserializedContent(maybe.Value);
                return commonResponse;
            }
            catch (Exception e)
            {
                commonResponse.AddError(builder => builder
                                                   .WithMessage("Error while deserializing response")
                                                   .WithErrorType("Deserialization Error")
                                                   .WithDetail(detail => detail
                                                                         .WithMessage(e.Message)
                                                                         .WithTarget(nameof(T))
                                                                         .WithException(e)
                                                                         .Build())
                                                   .Build());
                return commonResponse;
            }
        }


        private async Task<CommonResponse<GanymedeHttpResponse<T>>> ExecuteRequest<T>(bool stringifyResponseContent, bool stringifyRequestContent)
        {
            _executeDefaultActions();
            var commonResponse = new CommonResponse<GanymedeHttpResponse<T>>();
            try
            {
                commonResponse.AddResult(await new HttpRequestCoordinator(_client, _currentDescriptor).Send<T>(_currentDescriptor.RequestMessage));
                if (stringifyResponseContent)
                {
                    await commonResponse.Result.StringifyResponseBody();
                }

                if (stringifyRequestContent)
                {
                    await commonResponse.Result.StringifyRequestBody();
                }

                return commonResponse;
            }
            catch (HttpRequestException httpRequestException)
            {
                return CreateExceptionError(commonResponse, httpRequestException, "An HttpRequestException happened while sending the request!");
            }
            catch (MessageValidationException argument)
            {
                return CreateExceptionError(commonResponse, argument, "An MessageValidationException happened. The current message could not be sent out!");
            }
            catch (NullReferenceException nullReferenceException)
            {
                return CreateExceptionError(commonResponse, nullReferenceException, "An NullReferenceException happened. The current message could not be sent out");
            }
        }


        private void _executeDefaultActions()
        {
            if (!_uriConfigured) ConfigureRequestUri(uri => uri);

            if (!_messageConfigured) ConfigureHttpMessage(message => message.UseGanymedeEndpointOptions());
        }

        private CommonResponse<T> CreateExceptionError<T>(CommonResponse<T> response, Exception exception, string message)
        {
            response.AddError(builder => builder
                                         .WithMessage(message)
                                         .WithTarget(nameof(GanymedeHttpResponse<T>))
                                         .WithErrorType(CommonErrorType.Exception)
                                         .WithDetail(detail => detail.WithException(exception)
                                                                     .WithMessage(exception.Message)
                                                                     .WithTarget(exception.Source)
                                                                     .Build())
                                         .Build());
            return response;
        }
    }
}