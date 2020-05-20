using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Solari.Ganymede.Framework;

namespace Solari.Ganymede.Builders
{
    internal sealed class HttpRequestMessageHeaderBuilder : GanymedeHeaderBuilder

    {
        public HttpRequestMessageHeaderBuilder(HttpRequestMessage requestMessage) { RequestMessage = requestMessage; }

        public HttpRequestMessage RequestMessage { get; }


        public override void Accept(string value, double? quality = null)
        {
            RequestMessage
                .Headers
                .Accept
                .Add(MediaTypeQualityHeader(value, quality));
        }


        public override void AcceptCharset(string value, double? quality = null)
        {
            RequestMessage
                .Headers
                .AcceptCharset
                .Add(StringMediaQualityHeader(value, quality));
        }


        public override void AcceptEncoding(string value, double? quality = null)
        {
            StringWithQualityHeaderValue headerToAdd = StringMediaQualityHeader(value, quality);

            RequestMessage
                .Headers
                .AcceptEncoding
                .Add(headerToAdd);
        }


        public override void AcceptLanguage(string value, double? quality = null)
        {
            RequestMessage
                .Headers
                .AcceptLanguage
                .Add(StringMediaQualityHeader(value, quality));
        }


        public override void Authorization(string key, string value = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            RequestMessage
                .Headers
                .Authorization = new AuthenticationHeaderValue(key, value);
        }


        public override void CacheControl(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            RequestMessage
                .Headers
                .CacheControl = CacheControlHeaderValue.Parse(value);
        }

        public override void CustomHeader(string key, string value)
        {
            RequestMessage
                .Headers
                .Add(key, value);
        }

        public override void CustomHeader(string key, IEnumerable<string> values)
        {
            RequestMessage
                .Headers
                .Add(key, values);
        }


        public override void Expect(string key, string value = null)
        {
            RequestMessage
                .Headers
                .Expect
                .Add(new NameValueWithParametersHeaderValue(key, value));
        }


        public override void From(string value)
        {
            RequestMessage
                .Headers
                .From = value;
        }


        public override void Host(string value)
        {
            RequestMessage
                .Headers
                .Host = value;
        }


        public override void IfModifiedSince(DateTimeOffset? modifiedDate)
        {
            RequestMessage
                .Headers
                .IfModifiedSince = modifiedDate;
        }


        public override void IfUnmodifiedSince(DateTimeOffset? modifiedDate)
        {
            RequestMessage
                .Headers
                .IfUnmodifiedSince = modifiedDate;
        }


        public override void ProxyAuthorization(string key, string value = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            RequestMessage
                .Headers
                .ProxyAuthorization = new AuthenticationHeaderValue(key, value);
        }


        public override void Range(long? from, long? to)
        {
            RequestMessage
                .Headers
                .Range = new RangeHeaderValue(from, to);
        }

        public override void Referrer(Uri uri)
        {
            RequestMessage
                .Headers
                .Referrer = uri ?? throw new ArgumentNullException(nameof(uri));
        }


        public override void Referrer(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

            RequestMessage
                .Headers
                .Referrer = new Uri(value);
        }


        public override void UserAgent(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

            RequestMessage
                .Headers
                .UserAgent
                .Add(ProductInfoHeaderValue.Parse(value));
        }
    }
}