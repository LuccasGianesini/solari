using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationContextHandler _handler;
        private readonly ICorrelationContextFactory _factory;

        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationContextHandler handler, ICorrelationContextFactory factory)
        {
            _next = next;
            _handler = handler;
            _factory = factory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.TryExtractCorrelationContext(out ICorrelationContext correlationContext)
            && correlationContext.EnvoyCorrelationContext.IsValidEnvoyContext())
            {
                _handler.Current = correlationContext;
            }
            else
            {
                _handler.Current = _factory.Create_Root_From_System_Diagnostics_Activity_And_Tracers();
            }

            await _next.Invoke(context);

        }
        
        
    }
}