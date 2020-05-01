using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly.Registry;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Extensions;
using Solari.Ganymede.Framework.DelegatingHandlers;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Ganymede.DependencyInjection
{
    public class HttpClientActions
    {
        private readonly ISolariBuilder _builder;
        private readonly IReadOnlyDictionary<string, GanymedeRequestSettings> _requestSettings;

        public HttpClientActions(ISolariBuilder builder, IReadOnlyDictionary<string, GanymedeRequestSettings> requestSettings)
        {
            _builder = builder;
            _requestSettings = requestSettings;
        }

        /// <summary>
        ///     Add an <see cref="HttpClient" /> without any Polly policies.
        /// </summary>
        /// <typeparam name="TService">Service interface</typeparam>
        /// <typeparam name="TImplementation">Service concrete implementation</typeparam>
        /// <param name="section">Section key in AppSettings.json</param>
        public HttpClientActions AddGanymedeClient<TService, TImplementation>(string section)
            where TService : class
            where TImplementation : class, TService
        {
            ConfigureGanymedeHttpClient<TService, TImplementation>(section);

            return this;
        }

        /// <summary>
        ///     Add an <see cref="HttpClient" /> with default circuit breaker policy enabled.
        /// </summary>
        /// <typeparam name="TService">Service interface</typeparam>
        /// <typeparam name="TImplementation">Service concrete implementation</typeparam>
        /// <param name="section">Section key in AppSettings.json</param>
        public HttpClientActions AddGanymedeClientWithCircuitBreakerPolicy<TService, TImplementation>(string section)
            where TService : class
            where TImplementation : class, TService
        {
            ConfigureGanymedeHttpClient<TService, TImplementation>(section)
                .AddPolicyHandlerFromRegistry(GanymedeConstants.HttpCircuitBraker);

            return this;
        }

        /// <summary>
        ///     Add an <see cref="HttpClient" /> with a custom policy.
        /// </summary>
        /// <typeparam name="TService">Service interface</typeparam>
        /// <typeparam name="TImplementation">Service concrete implementation</typeparam>
        /// <param name="name">Section key in AppSettings.json</param>
        /// <param name="policyNames">Array of policy names</param>
        public HttpClientActions AddGanymedeClientWithCustomPolicy<TService, TImplementation>(string name, IEnumerable<string> policyNames)
            where TService : class
            where TImplementation : class, TService
        {
            IHttpClientBuilder httpBuilder = ConfigureGanymedeHttpClient<TService, TImplementation>(name);

            foreach (string policyName in policyNames) httpBuilder.AddPolicyHandlerFromRegistry(policyName);

            return this;
        }

        /// <summary>
        ///     Add an <see cref="HttpClient" /> with a custom policy.
        /// </summary>
        /// <typeparam name="TService">Service interface</typeparam>
        /// <typeparam name="TImplementation">Service concrete implementation</typeparam>
        /// <param name="name">Section key in AppSettings.json</param>
        /// <param name="policyName">Name of the policy inside the <see cref="IPolicyRegistry{TKey}" /></param>
        public HttpClientActions AddGanymedeClientWithCustomPolicy<TService, TImplementation>(string name, string policyName)
            where TService : class
            where TImplementation : class, TService
        {
            ConfigureGanymedeHttpClient<TService, TImplementation>(name)
                .AddPolicyHandlerFromRegistry(policyName);

            return this;
        }

        /// <summary>
        ///     Add an <see cref="HttpClient" /> with default circuit breaker, retry and custom policies enabled.
        /// </summary>
        /// <typeparam name="TService">Service interface</typeparam>
        /// <typeparam name="TImplementation">Service concrete implementation</typeparam>
        /// <param name="name">Section key in AppSettings.json</param>
        public HttpClientActions AddGanymedeClientWithPolicies<TService, TImplementation>(string name)
            where TService : class
            where TImplementation : class, TService
        {
            ConfigureGanymedeHttpClient<TService, TImplementation>(name)
                .AddPolicyHandlerFromRegistry(GanymedeConstants.HttpRetry)
                .AddPolicyHandlerFromRegistry(GanymedeConstants.HttpCircuitBraker);

            return this;
        }

        /// <summary>
        ///     Add an <see cref="HttpClient" /> with default retry policy enabled.
        /// </summary>
        /// <typeparam name="TService">Service interface</typeparam>
        /// <typeparam name="TImplementation">Service concrete implementation</typeparam>
        /// <param name="name">Section key in AppSettings.json</param>
        public HttpClientActions AddGanymedeClientWithRetryPolicy<TService, TImplementation>(string name)
            where TService : class
            where TImplementation : class, TService
        {
            ConfigureGanymedeHttpClient<TService, TImplementation>(name)
                .AddPolicyHandlerFromRegistry(GanymedeConstants.HttpRetry);

            return this;
        }


        private IHttpClientBuilder ConfigureGanymedeHttpClient<TService, TImplementation>(string name)
            where TService : class
            where TImplementation : class, TService
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return _builder
                   .Services
                   .AddSingleton<IGanymedeRequest<TImplementation>>(a =>
                   {
                       if (_requestSettings.ContainsKey(name))
                       {
                           return new GanymedeRequest<TImplementation>(_requestSettings.FirstOrDefault(a => a.Key == name).Value);
                       }

                       throw new
                           RequestNotFoundException($"The request {name} was not found in the dictionary. " +
                                                    "Make sure the yaml file contains the request");
                   })
                   .AddHttpClient<TService, TImplementation>()
                   .ConfigureHttpClient((provider, httpClient) =>
                   {
                       var request = provider.GetRequiredService<IGanymedeRequest<TImplementation>>();

                       httpClient.SetBaseAddress(request.RequestSettings.BaseAddress.Trim());
                       httpClient.SetDefaultRequestHeaders(request.RequestSettings.DefaultRequestHeaders);
                       httpClient.SetMaxResponseContentBufferSize(request.RequestSettings.MaxResponseContentBufferSize);
                       httpClient.SetTimeout(request.RequestSettings.Timeout.ToTimeSpan());
                   })
                   .ConfigurePrimaryHttpMessageHandler(_ => new DefaultHttpClientDelegatingHandler());
        }
    }
}