using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Solari.Ganymede.Extensions;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Utils;

namespace Solari.Ganymede.Framework.DelegatingHandlers
{
    public class HttpRequestMessageTimeoutDelegatingHandler : DelegatingHandler
    {
        public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(100);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (CancellationTokenSource cts = GetCancellationTokenSource(request, cancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException("The request message timed out.");
                }
            }
        }

        private static CancellationTokenSource GetCancellationTokenSource(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            Maybe<TimeSpan> maybe = httpRequestMessage.GetTimeout();
            TimeSpan timeout = maybe.HasValue ? maybe.Value : DefaultTimeout;

            if (timeout == Timeout.InfiniteTimeSpan) return null;

            var cts = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);

            cts.CancelAfter(timeout);

            return cts;
        }
    }
}
