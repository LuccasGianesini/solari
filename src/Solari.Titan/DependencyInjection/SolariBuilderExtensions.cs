using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Sol;
using Solari.Titan.Framework;

namespace Solari.Titan.DependencyInjection
{
    public static class TitanBuilderExtensions
    {
        // /// <summary>
        // ///     Registers the core services for the logging library into the service collection.
        // /// </summary>
        // /// <param name="builder">Service Collection</param>
        // /// <exception cref="ArgumentNullException"></exception>
        // public static ISolariBuilder AddTitan(this ISolariBuilder builder)
        // {
        //     if (builder == null) throw new ArgumentNullException(nameof(builder));
        //
        //     builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogEnricher<>), typeof(LogEnricher<>)));
        //     builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ITitanLogger<>), typeof(TitanLogger<>)));
        //     return builder;
        // }
    }
}