using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Solari.Ganymede.Framework
{
    public abstract class GanymedeHeaderBuilder
    {
        /// <summary>
        ///     Accept.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="quality">Quality of the content. Must be between 0 and 1</param>
        public virtual void Accept(string value, double? quality = null)
        {
        }

        /// <summary>
        ///     AcceptCharset.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="quality">Quality of the content. Must be between 0 and 1</param>
        public virtual void AcceptCharset(string value, double? quality = null)
        {
        }

        /// <summary>
        ///     AcceptEnconding.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="quality">QoS da requisição</param>
        public virtual void AcceptEncoding(string value, double? quality = null)
        {
        }

        /// <summary>
        ///     AcceptLanguage.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="quality">QoS da requisição</param>
        public virtual void AcceptLanguage(string value, double? quality = null)
        {
        }

        /// <summary>
        ///     Authorization.
        /// </summary>
        /// <param name="key">KeyOrQuality</param>
        /// <param name="value">Value</param>
        public virtual void Authorization(string key, string value = null)
        {
        }

        /// <summary>
        ///     CacheControl.
        /// </summary>
        /// <param name="value">Value</param>
        public virtual void CacheControl(string value)
        {
        }

        /// <summary>
        ///     Add correlation-id header.
        /// </summary>
        /// <param name="value">Value</param>
        public virtual void CorrelationId(string value)
        {
        }

        /// <summary>
        ///     Add correlation-id header.
        /// </summary>
        public virtual void CorrelationId()
        {
        }

        /// <summary>
        ///     Add a custom header.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public virtual void CustomHeader(string key, string value)
        {
        }

        /// <summary>
        ///     Add a custom header.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="values">Values</param>
        public virtual void CustomHeader(string key, IEnumerable<string> values)
        {
        }

        /// <summary>
        ///     Expect.
        /// </summary>
        /// <param name="key">KeyOrQuality</param>
        /// <param name="value">Value</param>
        public virtual void Expect(string key, string value = null)
        {
        }

        /// <summary>
        ///     From.
        /// </summary>
        /// <param name="value">Value</param>
        public virtual void From(string value)
        {
        }

        /// <summary>
        ///     Host.
        /// </summary>
        /// <param name="value">Value</param>
        public virtual void Host(string value)
        {
        }

        /// <summary>
        ///     IfModifiedSince.
        /// </summary>
        /// <param name="modifiedDate">Value</param>
        public virtual void IfModifiedSince(DateTimeOffset? modifiedDate)
        {
        }

        /// <summary>
        ///     IfUnmodifiedSince.
        /// </summary>
        /// <param name="modifiedDate">Value</param>
        public virtual void IfUnmodifiedSince(DateTimeOffset? modifiedDate)
        {
        }

        /// <summary>
        ///     ProxyAuthorization.
        /// </summary>
        /// <param name="key">KeyOrQuality</param>
        /// <param name="value">Value</param>
        public virtual void ProxyAuthorization(string key, string value = null)
        {
        }

        /// <summary>
        ///     Range.
        /// </summary>
        /// <param name="from">Value incial</param>
        /// <param name="to">Value final</param>
        public virtual void Range(long? from, long? to)
        {
        }

        /// <summary>
        ///     Referrer.
        /// </summary>
        /// <param name="uri">Uri no formato <see cref="Uri" /></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void Referrer(Uri uri)
        {
        }

        /// <summary>
        ///     Referrer.
        /// </summary>
        /// <param name="value">Value</param>
        public virtual void Referrer(string value)
        {
        }

        /// <summary>
        ///     UserAgent.
        /// </summary>
        /// <param name="value">Value</param>
        public virtual void UserAgent(string value)
        {
        }

        protected static MediaTypeWithQualityHeaderValue MediaTypeQualityHeader(string mediaType, double? quality = null)
        {
            return quality.HasValue
                       ? new MediaTypeWithQualityHeaderValue(mediaType, quality.Value)
                       : new MediaTypeWithQualityHeaderValue(mediaType);
        }

        protected static StringWithQualityHeaderValue StringMediaQualityHeader(string value, double? quality)
        {
            return quality.HasValue
                       ? new StringWithQualityHeaderValue(value, quality.Value)
                       : new StringWithQualityHeaderValue(value);
        }
    }
}