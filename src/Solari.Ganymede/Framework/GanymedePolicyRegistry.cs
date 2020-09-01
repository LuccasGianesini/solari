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
        public IPolicyRegistry<string> PolicyRegistry { get; }

        public GanymedePolicyRegistry()
        {
            PolicyRegistry = new PolicyRegistry();
        }

        public GanymedePolicyRegistry UseDefaultPolicies(bool retry, bool circuitBreaker)
        {
            var options = new GanymedePolicyOptions();
            if (retry)
                DefaultRetryPolicy(options.HttpRetry.BackOff, options.HttpRetry.Count);
            if (circuitBreaker)
                DefaultCircuitBreakerPolicy(options.HttpCircuitBreaker.NumberOfExceptionsBeforeBreaking, options.HttpCircuitBreaker.Duration);
            return this;
        }

        public GanymedePolicyRegistry RetryPolicy(int backoff, int count)
        {
            DefaultRetryPolicy(backoff, count);
            return this;
        }

        public GanymedePolicyRegistry CircuitBreakerPolicy(int numberOfExceptionsBeforeBreaking, string duration)
        {
            DefaultCircuitBreakerPolicy(numberOfExceptionsBeforeBreaking, duration);
            return this;
        }

        public GanymedePolicyRegistry AddPolicy(string key, IAsyncPolicy policy)
        {
            PolicyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry AddPolicy<T>(string key, IAsyncPolicy<T> policy)
        {
            PolicyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry AddPolicy(string key, ISyncPolicy policy)
        {
            PolicyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry AddPolicy<T>(string key, ISyncPolicy<T> policy)
        {
            PolicyRegistry.Add(key, policy);

            return this;
        }

        public GanymedePolicyRegistry ClearRegistry()
        {
            PolicyRegistry.Clear();

            return this;
        }

        public GanymedePolicyRegistry RemovePolicy(string key)
        {
            PolicyRegistry.Remove(key);

            return this;
        }

        private void DefaultCircuitBreakerPolicy(int numberOfExceptionsBeforeBreaking, string duration)
        {
            PolicyRegistry
                .Add(GanymedeConstants.HttpCircuitBreaker,
                     HttpPolicyExtensions.HandleTransientHttpError()
                                         .CircuitBreaker(numberOfExceptionsBeforeBreaking, duration.ToTimeSpan()));
        }

        private void DefaultRetryPolicy(int backoff, int count)
        {
            PolicyRegistry.Add(GanymedeConstants.HttpRetry, HttpPolicyExtensions
                                                             .HandleTransientHttpError()
                                                             .WaitAndRetryAsync(count,
                                                                                retryAttempt => TimeSpan.FromSeconds(Math.Pow(backoff, retryAttempt))));
        }
    }
}
