using System;
using System.Collections.Generic;
using System.Net.Http;
using Solari.Ganymede.ContentSerializers;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Sol.Extensions;

// ReSharper disable CollectionNeverUpdated.Global

namespace Solari.Ganymede.Domain.Options
{
    public class GanymedeRequestResource
    {
        /// <summary>
        ///     Endpoint description.
        /// </summary>
        public string Description { get; set; }

        public string Deserializer { get; set; }


        public List<string> RequiredHeaders { get; set; } = new List<string>(2);

        /// <summary>
        ///     Pre-populate endpoint headers.
        /// </summary>
        public List<GanymedeRequestHeader> ResourceHeaders { get; set; } = new List<GanymedeRequestHeader>(2);

        /// <summary>
        ///     http version.
        /// </summary>
        public string HttpVersion { get; set; }

        /// <summary>
        ///     Endpoint name.
        /// </summary>
        public string Name { get; set; }

        public string Serializer { get; set; }

        /// <summary>
        ///     Sets the timeout for the request.
        /// </summary>
        public string Timeout { get; set; }

        /// <summary>
        ///     Enpoint uri.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     Http method.
        /// </summary>
        public string Verb { get; set; }

        /// <summary>
        ///     Completion option of the request.
        ///     Available values:
        ///     content-read
        ///     headers-read
        /// </summary>
        public string CompletionOption { get; set; }

        /// <summary>
        ///     Gets the completion option of the request.
        ///     Defaults to <see cref="HttpCompletionOption.ResponseContentRead" />;
        /// </summary>
        /// <returns>
        ///     <see cref="HttpCompletionOption" />
        /// </returns>
        public HttpCompletionOption GetCompletionOption()
        {
            if (string.IsNullOrEmpty(CompletionOption)) return HttpCompletionOption.ResponseContentRead;
            return CompletionOption.ToUpperInvariant().Trim() switch
                   {
                       "CONTENT-READ" => HttpCompletionOption.ResponseContentRead,
                       "HEADERS-READ" => HttpCompletionOption.ResponseHeadersRead,
                       _              => HttpCompletionOption.ResponseContentRead
                   };
        }

        /// <summary>
        ///     Gets the timeout of the request.
        ///     Defaults to 30 seconds.
        /// </summary>
        /// <returns>
        ///     <see cref="TimeSpan" />
        /// </returns>
        public TimeSpan GetTimeout()
        {
            if (string.IsNullOrEmpty(Timeout)) return TimeSpan.FromSeconds(30);
            var timespan = Timeout.ToTimeSpan();
            return timespan == TimeSpan.MinValue ? TimeSpan.FromSeconds(30) : timespan;
        }

        /// <summary>
        ///     Gets the http version of the request. Defaults to 1.1
        /// </summary>
        /// <returns></returns>
        public Version GetHttpVersion()
        {
            if (string.IsNullOrEmpty(HttpVersion)) return Version.Parse("1.1");
            return HttpVersion.Trim() switch
                   {
                       "0.9" => Version.Parse("0.9"),
                       "1.0" => Version.Parse("1.0"),
                       "1.1" => Version.Parse("1.1"),
                       "2.0" => Version.Parse("2.0"),
                       _     => Version.Parse("1.1")
                   };
        }

        /// <summary>
        ///     Gets the <see cref="HttpMethod" /> of the request.
        /// </summary>
        /// <exception cref="ArgumentException">When invalid verb is provided</exception>
        /// <returns>
        ///     <see cref="HttpMethod" />
        /// </returns>
        public HttpMethod GetVerb()
        {
            if (string.IsNullOrEmpty(Verb)) return HttpMethod.Get;
            return Verb.Trim().ToUpperInvariant() switch
                   {
                       "POST"    => HttpMethod.Post,
                       "GET"     => HttpMethod.Get,
                       "DELETE"  => HttpMethod.Delete,
                       "PATCH"   => HttpMethod.Patch,
                       "PUT"     => HttpMethod.Put,
                       "HEAD"    => HttpMethod.Head,
                       "TRACE"   => HttpMethod.Trace,
                       "OPTIONS" => HttpMethod.Options,
                       _         => throw new GanymedeException($"Invalid http method found while setting method for RequestMessage. Provided value: {Verb}")
                   };
        }

        public IContentSerializer GetSerializer()
        {
            if (string.IsNullOrEmpty(Serializer)) return new JsonContentSerializer();
            return Serializer.ToUpperInvariant().Trim() switch
                   {
                       "JSON"        => new JsonContentSerializer(),
                       "XML"         => new XmlContentSerializer(),
                       "URL-ENCODED" => new UrlEncodedSerializer(),
                       "json"        => new JsonContentSerializer(),
                       "xml"         => new XmlContentSerializer(),
                       "url-encoded" => new UrlEncodedSerializer(),
                       "urlencoded"  => new UrlEncodedSerializer(),
                       "url"         => new UrlEncodedSerializer(),
                       _             => new JsonContentSerializer()
                   };
        }

        public IContentDeserializer GetDeserializer()
        {
            if (string.IsNullOrEmpty(Deserializer)) return new JsonContentDeserializer();
            return Deserializer.ToUpperInvariant().Trim() switch
                   {
                       "JSON"        => new JsonContentDeserializer(),
                       "XML"         => new XmlContentDeserializer(),
                       "URL-ENCODED" => new UrlEncodedDeserializer(),
                       "json"        => new JsonContentDeserializer(),
                       "xml"         => new XmlContentDeserializer(),
                       "url-encoded" => new UrlEncodedDeserializer(),
                       "urlencoded"  => new UrlEncodedDeserializer(),
                       "url"         => new UrlEncodedDeserializer(),
                       _             => new JsonContentDeserializer()
                   };
        }
    }
}
