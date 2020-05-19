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
                throw new Domain.Exceptions.GanymedeException("HttpRequestMessage is null", arg);
            }
            catch (InvalidOperationException inv)
            {
                throw new Domain.Exceptions.GanymedeException("Request message was already sent by the HttpClient instance", inv);
            }
            catch (HttpRequestException req)
            {
                throw new Domain.Exceptions.GanymedeException("Request failed due to an underlying network issue.", req);
            }
            catch (TaskCanceledException canc)
            {
                throw new Domain.Exceptions.GanymedeException("A task was canceled", canc);
            }
        }
    }
}