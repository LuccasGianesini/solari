using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IConfiguration _configuration;

        public HttpClientActions(ISolariBuilder builder, IConfiguration configuration)
        {
            _builder = builder;
            _configuration = configuration;
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
            ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(section);

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
            ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(section)
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
            IHttpClientBuilder httpBuilder = ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(name);

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
            ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(name)
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
            ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(name)
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
            ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(name)
                .AddPolicyHandlerFromRegistry(GanymedeConstants.HttpRetry);

            return this;
        }
        
        private IHttpClientBuilder ConfigureGanymedeHttpClientUsingAppSettings<TService, TImplementation>(string section)
            where TService : class
            where TImplementation : class, TService
        {

            return _builder
                   .Services
                   .AddSingleton<IGanymedeRequest<TImplementation>>(a =>
                   {
                       IConfigurationSection cfgSection = _configuration.GetSection(section);
                       if (cfgSection.Exists())
                           return new GanymedeRequest<TImplementation>(_configuration.GetOptions<GanymedeRequestSpecification>(cfgSection));
                       throw new
                           RequestNotFoundException($"The request {section} was not found in the AppSettings.json file.");
                   })
                   .AddHttpClient<TService, TImplementation>()
                   .ConfigureHttpClient((provider, httpClient) =>
                   {
                       var request = provider.GetRequiredService<IGanymedeRequest<TImplementation>>();

                       httpClient.SetBaseAddress(request.RequestSpecification.BaseAddress.Trim());
                       httpClient.SetDefaultRequestHeaders(request.RequestSpecification.DefaultRequestHeaders);
                       httpClient.SetMaxResponseContentBufferSize(request.RequestSpecification.MaxResponseContentBufferSize);
                       httpClient.SetTimeout(request.RequestSpecification.Timeout.ToTimeSpan());
                   })
                   .ConfigurePrimaryHttpMessageHandler(_ => new DefaultHttpClientDelegatingHandler());
        }
    }
}