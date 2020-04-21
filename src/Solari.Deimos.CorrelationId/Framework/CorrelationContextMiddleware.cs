using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationIdMiddleware
    {
        private readonly IServiceProvider _provider;
        private readonly RequestDelegate _next;
        private readonly ICorrelationContextManager _manager;

        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationContextManager manager,
                                       IServiceProvider provider)
        {
            _next = next;
            _manager = manager;

            _provider = provider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.TryExtractCorrelationContext(out ICorrelationContext correlationContext)
             && correlationContext.EnvoyCorrelationContext.IsValidEnvoyContext())
                _manager.Update(correlationContext);
            else
                _manager.CreateAndSet();
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