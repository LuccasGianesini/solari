using System;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Options;
using Solari.Sol.Extensions;

namespace Solari.Ganymede.Framework
{
    public class GanymedePolicyRegistry
    {
        private readonly GanymedePolicyOptions _policyOptions;
        private readonly IPolicyRegistry<string> _policyRegistry;

        public GanymedePolicyRegistry(IPolicyRegistry<string> policyRegistry, IOptions<GanymedePolicyOptions> policyOptions)
        {
            _policyRegistry = policyRegistry;
            _policyOptions = policyOptions.Value;
            DefaultRetryPolicy();
            DefaultCircuitBrakerPolicy();
        }

        public GanymedePolicyRegistry AddPolicy(string key, IAsyncPolicy policy)
        {
            _policyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry AddPolicy<T>(string key, IAsyncPolicy<T> policy)
        {
            _policyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry AddPolicy(string key, ISyncPolicy policy)
        {
            _policyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry AddPolicy<T>(string key, ISyncPolicy<T> policy)
        {
            _policyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry ClearRegistry()
        {
            _policyRegistry.Clear();

            return this;
        }

        public GanymedePolicyRegistry RemovePolicy(string key)
        {
            _policyRegistry.Remove(key);

            return this;
        }

        private void DefaultCircuitBrakerPolicy()
        {
            _policyRegistry
                .Add(GanymedeConstants.HttpCircuitBraker,
                     HttpPolicyExtensions.HandleTransientHttpError()
                                         .CircuitBreaker(_policyOptions.HttpCircuitBreaker.NumberOfExceptionsBeforeBreaking,
                                                         _policyOptions.HttpCircuitBreaker.Duration.ToTimeSpan()));
        }

        private void DefaultRetryPolicy()
        {
            _policyRegistry
                .Add(GanymedeConstants.HttpRetry, HttpPolicyExtensions
                                                  .HandleTransientHttpError()
                                                  .WaitAndRetryAsync(_policyOptions.HttpRetry.Count,
                                                                     retryAttempt => TimeSpan.FromSeconds(
                                                                                                          Math.Pow(_policyOptions.HttpRetry.BackOff,
                                                                                                                   retryAttempt))));
        }
    }
}