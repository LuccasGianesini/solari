using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Solari.Ganymede.ContentSerializers;
using Solari.Ganymede.Domain;
using Solari.Sol.Extensions;
using Solari.Sol.Utils;

namespace Solari.Ganymede.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        ///     Clone a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        public static HttpRequestMessage Clone(this HttpRequestMessage httpRequestMessage)
        {
            var clone = new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri);

            foreach (KeyValuePair<string, object> prop in httpRequestMessage.Properties) clone.Properties.Add(prop);
            foreach ((string key, IEnumerable<string> value) in httpRequestMessage.Headers) clone.Headers.TryAddWithoutValidation(key, value);

            return clone;
        }

        /// <summary>
        ///     Deep clone a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        public static HttpRequestMessage DeepClone(this HttpRequestMessage httpRequestMessage)
        {
            var clone = new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri)
            {
                Content = httpRequestMessage.Content.CloneContent(), Version = httpRequestMessage.Version
            };

            foreach (KeyValuePair<string, object> prop in httpRequestMessage.Properties) clone.Properties.Add(prop);
            foreach ((string key, IEnumerable<string> value) in httpRequestMessage.Headers) clone.Headers.TryAddWithoutValidation(key, value);

            return clone;
        }

        /// <summary>
        ///     Get the cancellation token of a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        public static CancellationToken GetCancellationToken(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            httpRequestMessage.Properties.TryGetValue(HttpRequestMessageProperties.CancellationTokenProperty, out object propertyValue);

            return (CancellationToken) (propertyValue ?? CancellationToken.None);
        }

        /// <summary>
        ///     Get the completion option of a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        public static HttpCompletionOption GetCompletionOption(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            httpRequestMessage.Properties.TryGetValue(HttpRequestMessageProperties.CompletionOptionProperty, out object propertyValue);

            return (HttpCompletionOption) (propertyValue ?? HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>
        ///     Get the content deserializer property from the specified <see cref="HttpRequestMessage" /> properties dictionary.
        /// </summary>
        /// <param name="requestMessage">The request message containing the property.</param>
        /// <returns>
        ///     The <see cref="IContentSerializer" /> to use when serializing content to send in the request message.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="requestMessage" /> is <see langword="null" /></exception>
        public static IContentDeserializer GetContentDeserializer(this HttpRequestMessage requestMessage)
        {
            return GetDeserializerByPropertyKey(requestMessage, HttpRequestMessageProperties.ContentDeserializerProperty);
        }

        /// <summary>
        ///     Get the content serializer property from the specified <see cref="HttpRequestMessage" /> properties dictionary.
        /// </summary>
        /// <param name="requestMessage">The request message containing the property.</param>
        /// <returns>
        ///     The <see cref="IContentSerializer" /> to use when serializing content to send in the request message.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="requestMessage" /> is <see langword="null" /></exception>
        public static IContentSerializer GetContentSerializer(this HttpRequestMessage requestMessage)
        {
            return GetSerializerByPropertyKey(requestMessage, HttpRequestMessageProperties.ContentSerializerProperty);
        }

        /// <summary>
        ///     Get the form data of a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        public static Dictionary<string, ICollection<string>> GetFormData(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            httpRequestMessage
                .Properties
                .TryGetOrAdd(HttpRequestMessageProperties.RequestFormDataProperty, _ => new Dictionary<string, ICollection<string>>(),
                             out object resultedValue);

            return resultedValue as Dictionary<string, ICollection<string>>;
        }

        /// <summary>
        ///     Get the timeout for a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        /// <returns></returns>
        public static Maybe<TimeSpan> GetTimeout(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            if (httpRequestMessage.Properties.TryGetValue(HttpRequestMessageProperties.RequestMessageTimeout, out object value)
             && value is TimeSpan timeout)
                return Maybe<TimeSpan>.Some(timeout);

            return Maybe<TimeSpan>.None;
        }

        /// <summary>
        ///     Set the cancellation token of a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static void SetCancellationToken(this HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            httpRequestMessage.Properties[HttpRequestMessageProperties.CancellationTokenProperty] = cancellationToken;
        }

        /// <summary>
        ///     Set <see cref="HttpCompletionOption" /> for a <see cref="HttpRequestMessage" />.
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        /// <param name="completionOption">
        ///     <see cref="HttpCompletionOption" />
        /// </param>
        public static void SetCompletionOption(this HttpRequestMessage httpRequestMessage, HttpCompletionOption completionOption)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            httpRequestMessage.Properties[HttpRequestMessageProperties.CompletionOptionProperty] = completionOption;
        }


        /// <summary>
        ///     Set the content deserializer property on the specified <see cref="HttpRequestMessage" /> properties dictionary.
        /// </summary>
        /// <param name="requestMessage">The request message containing the property.</param>
        /// <param name="contentDeserializer">
        ///     The <see cref="IContentSerializer" /> to use when serializing content to send in the
        ///     request message.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="requestMessage" /> is <see langword="null" /></exception>
        public static void SetContentDeserializer(this HttpRequestMessage requestMessage, IContentDeserializer contentDeserializer)
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            requestMessage.Properties[HttpRequestMessageProperties.ContentDeserializerProperty] = contentDeserializer;
        }

        /// <summary>
        ///     Set the content serializer property on the specified <see cref="HttpRequestMessage" /> properties dictionary.
        /// </summary>
        /// <param name="requestMessage">The request message containing the property.</param>
        /// <param name="contentSerializer">
        ///     The <see cref="IContentSerializer" /> to use when serializing content to send in the
        ///     request message.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="requestMessage" /> is <see langword="null" /></exception>
        public static void SetContentSerializer(this HttpRequestMessage requestMessage, IContentSerializer contentSerializer)
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            requestMessage.Properties[HttpRequestMessageProperties.ContentSerializerProperty] = contentSerializer;
        }


        /// <summary>
        ///     Set a timeout for a <see cref="HttpRequestMessage" />
        /// </summary>
        /// <param name="httpRequestMessage">Message</param>
        /// <param name="timeout">Timeout</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetTimeout(this HttpRequestMessage httpRequestMessage, TimeSpan? timeout)
        {
            if (httpRequestMessage == null) throw new ArgumentNullException(nameof(httpRequestMessage));

            httpRequestMessage.Properties[HttpRequestMessageProperties.RequestMessageTimeout] = timeout;
        }

        private static IContentSerializer GetSerializerByPropertyKey(HttpRequestMessage requestMessage, string propertyKey)
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            if (!requestMessage.Properties.TryGetValue(propertyKey, out object contentSerializer)) return new JsonContentSerializer();
            if (contentSerializer == null) return new JsonContentSerializer();

            return (IContentSerializer) contentSerializer;
        }
        private static IContentDeserializer GetDeserializerByPropertyKey(HttpRequestMessage requestMessage, string propertyKey)
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            if (!requestMessage.Properties.TryGetValue(propertyKey, out object contentSerializer)) return new JsonContentDeserializer();
            if (contentSerializer == null) return new JsonContentDeserializer();

            return (IContentDeserializer) contentSerializer;
        }

    }
}