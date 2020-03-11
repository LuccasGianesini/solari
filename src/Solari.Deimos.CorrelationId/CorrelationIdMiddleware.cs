using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos.CorrelationId
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICorrelationContextHandler handler, ICorrelationContextFactory factory)
        {
            DeimosLogger.CorrelationIdLogger.InvokedMiddleware();
            if (!TrySetCorrelationId(context, out StringValues correlationId))
            {
                context.TraceIdentifier = correlationId;
            }
            DeimosLogger.CorrelationIdLogger.GotCorrelationIdValue(correlationId);
            handler.Current = factory.CreateHttpContext(DeimosConstants.DefaultCorrelationIdHeader, correlationId);
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(DeimosConstants.DefaultCorrelationIdHeader))
                    context.Response.Headers.Add(DeimosConstants.DefaultCorrelationIdHeader, correlationId);

                DeimosLogger.CorrelationIdLogger.AddCorrelationIdInResponse();
                return Task.CompletedTask;
            });
            await _next(context);
            handler.Current = null;
        }
        
        
        
        private static bool TrySetCorrelationId(HttpContext context, out StringValues correlationId)
        {
            DeimosLogger.CorrelationIdLogger.TryGetCorrelationId();
            bool correlationIdFoundInRequestHeader = context.Request.Headers.TryGetValue(DeimosConstants.DefaultCorrelationIdHeader, out correlationId);
            if (!correlationIdFoundInRequestHeader)
            {
                correlationId = TraceIdGenerator.Create();
            }
            return correlationIdFoundInRequestHeader;
        }
    }
}