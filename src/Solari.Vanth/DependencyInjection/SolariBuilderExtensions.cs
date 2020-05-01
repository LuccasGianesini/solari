using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Vanth.Builders;
using Solari.Vanth.Validation;

namespace Solari.Vanth.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        /// Add Vanth into the DI Container.
        /// </summary>
        /// <param name="builder"><see cref="ISolariBuilder"/></param>
        /// <returns><see cref="ISolariBuilder"/></returns>
        public static ISolariBuilder AddVanth(this ISolariBuilder builder)
        {
            builder.Services.AddSingleton<ICommonResponseFactory, CommonResponseFactory>();
            var opt = builder.AppConfiguration.GetOptions<VanthOptions>(VanthLibConstants.AppSettingsSection);
            if (!opt.UseFluentValidation) return builder;
            builder.Services.AddSingleton<IVanthValidationService, VanthValidationService>();
            builder.Services.AddSingleton<IValidatorFactory, VanthValidatorFactory>();
            builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            return builder;
        }
    }
}