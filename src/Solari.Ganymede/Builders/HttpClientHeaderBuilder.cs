using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Solari.Ganymede.Framework;

namespace Solari.Ganymede.Builders
{
    public class HttpClientHeaderCommandBuilder : GanymedeHeaderBuilder
    {
        public HttpClientHeaderCommandBuilder(HttpClient httpClient) { HttpClient = httpClient; }

        public HttpClient HttpClient { get; }

        /// <inheritdoc />
        public override void Accept(string value, double? quality = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HttpClient.DefaultRequestHeaders.Accept.Add(MediaTypeQualityHeader(value, quality));
        }

        /// <inheritdoc />
        public override void AcceptCharset(string value, double? quality = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HttpClient.DefaultRequestHeaders.AcceptCharset.Add(StringMediaQualityHeader(value, quality));
        }

        /// <inheritdoc />
        public override void AcceptEncoding(string value, double? quality = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringMediaQualityHeader(value, quality));
        }

        /// <inheritdoc />
        public override void AcceptLanguage(string value, double? quality = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HttpClient.DefaultRequestHeaders.AcceptLanguage.Add(StringMediaQualityHeader(value, quality));
        }

        /// <inheritdoc />
        public override void Authorization(string key, string value = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key, value);
        }

        /// <inheritdoc />
        public override void CacheControl(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HttpClient.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse(value);
        }

        /// <inheritdoc />
        public override void CustomHeader(string key, string value) { HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value); }

        /// <inheritdoc />
        public override void CustomHeader(string key, IEnumerable<string> values) { HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, values); }

        /// <inheritdoc />
        public override void Expect(string key, string value = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            HttpClient.DefaultRequestHeaders.Expect.Add(new NameValueWithParametersHeaderValue(key, value));
        }

        /// <inheritdoc />
        public override void From(string value) { HttpClient.DefaultRequestHeaders.From = value ?? throw new ArgumentNullException(nameof(value)); }

        /// <inheritdoc />
        public override void Host(string value) { HttpClient.DefaultRequestHeaders.Host = value ?? throw new ArgumentNullException(nameof(value)); }

        /// <inheritdoc />
        public override void IfModifiedSince(DateTimeOffset? modifiedDate) { HttpClient.DefaultRequestHeaders.IfModifiedSince = modifiedDate; }

        /// <inheritdoc />
        public override void IfUnmodifiedSince(DateTimeOffset? modifiedDate) { HttpClient.DefaultRequestHeaders.IfUnmodifiedSince = modifiedDate; }

        /// <inheritdoc />
        public override void ProxyAuthorization(string key, string value = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            HttpClient.DefaultRequestHeaders.ProxyAuthorization = new AuthenticationHeaderValue(key, value);
        }

        /// <inheritdoc />
        public override void Range(long? from, long? to) { HttpClient.DefaultRequestHeaders.Range = new RangeHeaderValue(from, to); }

        /// <inheritdoc />
        public override void Referrer(Uri uri) { HttpClient.DefaultRequestHeaders.Referrer = uri ?? throw new ArgumentNullException(nameof(uri)); }

        /// <inheritdoc />
        public override void Referrer(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            HttpClient.DefaultRequestHeaders.Referrer = new Uri(value);
        }

        /// <inheritdoc />
        public override void UserAgent(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            // HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(value));
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", value);
        }
    }
}