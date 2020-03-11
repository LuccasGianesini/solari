using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Solari.Ganymede.Extensions;

namespace Solari.Ganymede.Framework.DelegatingHandlers
{
    public class HttpRequestMessageCancelledDelegatingHandler : DelegatingHandler
    {
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
                    throw new TimeoutException("The request message was cancelled.");
                }
            }
        }

        private static CancellationTokenSource GetCancellationTokenSource(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            CancellationToken cancellation = httpRequestMessage.GetCancellationToken();

            if (cancellation == CancellationToken.None) return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var cts = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken, cancellation);

            cts.Cancel();

            return cts;
        }
    }
}