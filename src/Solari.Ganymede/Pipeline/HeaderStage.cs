using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Solari.Deimos.CorrelationId;
using Solari.Ganymede.Builders;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;

namespace Solari.Ganymede.Pipeline
{
    public class HeaderStage : IPipelineStage
    {
        private readonly HttpRequestMessageHeaderBuilder _headerBuilder;

        public HeaderStage(PipelineDescriptor pipeline)
        {
            PipelineDescriptor = pipeline;
            _headerBuilder = new HttpRequestMessageHeaderBuilder(pipeline.RequestMessage);
        }

        public PipelineDescriptor PipelineDescriptor { get; }

        public static implicit operator PipelineDescriptor(HeaderStage headerStage) { return headerStage.PipelineDescriptor; }

        public HeaderStage Accept(string value, double? quality = null)
        {
            _headerBuilder.Accept(value, quality);

            return this;
        }

        public HeaderStage AcceptCharset(string value, double? quality = null)
        {
            _headerBuilder.AcceptCharset(value, quality);

            return this;
        }

        public HeaderStage AcceptEncoding(string value, double? quality = null)
        {
            _headerBuilder.AcceptEncoding(value, quality);

            return this;
        }

        public HeaderStage AcceptLanguage(string value, double? quality = null)
        {
            _headerBuilder.AcceptLanguage(value, quality);

            return this;
        }

        public HeaderStage Authorization(string key, string value = null)
        {
            _headerBuilder.Authorization(key, value);

            return this;
        }

        public HeaderStage BasicAuthorization(string username, SecureString password)
        {
            Authorization(GanymedeConstants.Basic, Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));

            return this;
        }

        public HeaderStage CorrelationContext(ICorrelationContextManager manager, string messageId = "")
        {
            PipelineDescriptor.RequestMessage.AddCorrelationContext(manager.GetOrCreateAndSet(messageId));
            return this;
        }

        public HeaderStage BearerAuthorization(string value)
        {
            Authorization(GanymedeConstants.BearerAuth, value);

            return this;
        }

        public HeaderStage CacheControl(string value)
        {
            _headerBuilder.CacheControl(value);

            return this;
        }

        public HeaderStage CorrelationId(string value)
        {
            _headerBuilder.CorrelationId(value);

            return this;
        }

        public HeaderStage CustomHeader(string key, string value)
        {
            _headerBuilder.CustomHeader(key, value);

            return this;
        }

        public HeaderStage CustomHeader(string key, IEnumerable<string> values)
        {
            _headerBuilder.CustomHeader(key, values);

            return this;
        }

        public HeaderStage Expect(string key, string value = null)
        {
            _headerBuilder.Expect(key, value);

            return this;
        }

        public HeaderStage From(string value)
        {
            _headerBuilder.From(value);

            return this;
        }

        public HeaderStage Host(string value)
        {
            _headerBuilder.Host(value);

            return this;
        }

        public HeaderStage IfModifiedSince(DateTimeOffset? modifiedDate)
        {
            _headerBuilder.IfModifiedSince(modifiedDate);

            return this;
        }

        public HeaderStage IfUnmodifiedSince(DateTimeOffset? modifiedDate)
        {
            _headerBuilder.IfUnmodifiedSince(modifiedDate);

            return this;
        }

        public HeaderStage ProxyAuthorization(string key, string value = null)
        {
            _headerBuilder.ProxyAuthorization(key, value);

            return this;
        }

        public HeaderStage Range(long? from, long? to)
        {
            _headerBuilder.Range(from, to);

            return this;
        }

        public HeaderStage Referrer(Uri uri)
        {
            _headerBuilder.Referrer(uri);

            return this;
        }

        public HeaderStage Referrer(string value)
        {
            _headerBuilder.Referrer(value);

            return this;
        }

        public HeaderStage UserAgent(string value)
        {
            _headerBuilder.UserAgent(value);

            return this;
        }

        /// <summary>
        ///     Use the <see cref="GanymedeRequestResource" /> object to set the necessary attribute values.
        /// </summary>
        /// <returns></returns>
        public HeaderStage UseGanymedeEndpointOptions()
        {
            if (PipelineDescriptor.Resource.ResourceHeaders.Count <= 0) return this;

            foreach (GanymedeRequestHeader headerOptions in PipelineDescriptor.Resource.ResourceHeaders)
            {
                IHeaderBuilderCommand headerCommand = CommandTypeRegistry.Instance.TryGetCommandType(headerOptions.Name.Trim().ToUpperInvariant());
                headerCommand?.Execute(_headerBuilder, headerOptions.KeyOrQuality, headerOptions.Value);
            }

            return this;
        }
    }
}