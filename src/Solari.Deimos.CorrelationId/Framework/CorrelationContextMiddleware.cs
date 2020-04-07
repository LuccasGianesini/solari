using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationIdMiddleware
    {
        private readonly ICorrelationContextAccessor _accessor;
        private readonly ICorrelationContextHandler _handler;
        private readonly IServiceProvider _provider;
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationContextAccessor accessor, ICorrelationContextHandler handler,
                                       IServiceProvider provider)
        {
            _next = next;
            _accessor = accessor;
            _handler = handler;
            _provider = provider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.TryExtractCorrelationContext(out ICorrelationContext correlationContext)
             && correlationContext.EnvoyCorrelationContext.IsValidEnvoyContext())
                _accessor.Current = correlationContext;
            else
                _accessor.Current = _handler.Create_Root_From_System_Diagnostics_Activity_And_Tracers();
            context.Response.OnStarting(state =>
            {
                var ctx = state as HttpContext;
                ctx.Response.AddCorrelationContext(_provider);
                return Task.CompletedTask;
            }, context);
            await _next.Invoke(context);
        }
    }
}