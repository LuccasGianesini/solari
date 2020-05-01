using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solari.Io.Abstractions;

namespace Solari.Io
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapIo(this IEndpointRouteBuilder builder)
        {
            IoOptions options = builder.ServiceProvider.GetService<IOptions<IoOptions>>().Value;
            if (!options.Enabled)
                return builder;
            if (options.EnableUi)
            {
                builder.MapHealthChecks(options.HealthEndpoint, new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                builder.MapHealthChecksUI();
                return builder;
            }

            builder.MapHealthChecks(options.HealthEndpoint, new HealthCheckOptions()
            {
                Predicate = _ => true
            });
            return builder;
        }
    }
}