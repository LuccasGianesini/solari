// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Solari.Ganymede.Domain.Options
{
    public class GanymedePolicyOptions
    {
        public GanymedeCircuitBreakerOptions HttpCircuitBreaker { get; set; }
        public GanymedeRetryOptions HttpRetry { get; set; }
    }
}