using System;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Framework;
using Solari.Sol;

namespace Solari.Callisto.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        ///     Add callisto and callisto connector into the DI container.
        /// </summary>
        /// <param name="solariBuilder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ISolariBuilder AddCallisto(this ISolariBuilder solariBuilder, Action<ICallistoConfiguration> configure)
        {
            solariBuilder.Services.AddTransient<ICallistoOperationFactory, CallistoOperationFactory>();
            configure(new CallistoConfiguration(solariBuilder));
            return solariBuilder;
        }
    }
}