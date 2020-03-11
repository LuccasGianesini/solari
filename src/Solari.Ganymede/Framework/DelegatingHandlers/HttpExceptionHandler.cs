using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Solari.Ganymede.Domain.Exceptions;

namespace Solari.Ganymede.Framework.DelegatingHandlers
{
    public class HttpExceptionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            try
            {
                response = await base.SendAsync(request, cancellationToken);

                return response;
            }
            catch (ArgumentNullException arg)
            {
                throw new SolariHttpException("HttpRequestMessage is null", arg);
            }
            catch (InvalidOperationException inv)
            {
                throw new SolariHttpException("Request message was already sent by the HttpClient instance", inv);
            }
            catch (HttpRequestException req)
            {
                throw new SolariHttpException("Request failed due to an underlying network issue.", req);
            }
            catch (TaskCanceledException canc)
            {
                throw new SolariHttpException("A task was canceled", canc);
            }
        }
    }
}