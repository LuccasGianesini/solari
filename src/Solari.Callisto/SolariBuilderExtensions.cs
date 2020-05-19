﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;
using Solari.Callisto.Framework;
using Solari.Rhea.Utils;
using Solari.Sol;

namespace Solari.Callisto
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        /// Add callisto and callisto connector into the DI container.
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