using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Solari.Deimos.Abstractions;
using Solari.Deimos.CorrelationId.Framework;
using Solari.Sol;

namespace Solari.Deimos.CorrelationId
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        ///     Add Deimos CorrelationId infrastructure into the DI Container.
        /// </summary>
        /// <param name="solariBuilder">The builder</param>
        /// <param name="useMiddleware">Indicates if <see cref="CorrelationIdMiddleware" /> should be used</param>
        /// <returns></returns>
        public static ISolariBuilder AddDeimosCorrelationId(this ISolariBuilder solariBuilder, bool useMiddleware)
        {
            if (useMiddleware)
            {
                solariBuilder.Services.AddSingleton<ICorrelationContextFactory, CorrelationContextFactory>();
                solariBuilder.Services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();
                solariBuilder.Services.AddSingleton<ICorrelationContextManager, CorrelationContextManager>();

                solariBuilder.AddBuildAction(new BuildAction("Deimos CorrelationId")
                {
                    Action = provider =>
                    {
                        var marshal = provider.GetRequiredService<ISolariMarshal>();
                        marshal.ApplicationBuilder?.UseMiddleware<CorrelationIdMiddleware>();
                        DeimosLogger.CorrelationIdLogger.UsingCorrelationIdMiddleware();
                    }
                });
            }
            return solariBuilder;
        }
    }
}