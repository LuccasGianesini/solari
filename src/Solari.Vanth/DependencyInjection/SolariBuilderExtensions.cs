using System;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Vanth.Validation;

namespace Solari.Vanth.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        /// <summary>
        ///     Add Vanth into the DI Container.
        /// </summary>
        /// <param name="builder">
        ///     <see cref="ISolariBuilder" />
        /// </param>
        /// <returns>
        ///     <see cref="ISolariBuilder" />
        /// </returns>
        public static ISolariBuilder AddVanth(this ISolariBuilder builder)
        {
            IConfigurationSection section = builder.Configuration.GetSection(VanthLibConstants.AppSettingsSection);
            if (!section.Exists())
                throw new VanthException("Unable to find Vanth AppSettings section");
            var opt = builder.Configuration.GetOptions<VanthOptions>(section);
            builder.Services.AddSingleton<IResultFactory, ResultFactory>();
            ConfigureFluentValidation(opt, builder);
            ConfigureExceptionMiddleware(opt, builder, section);
            return builder;
        }

        private static void ConfigureExceptionMiddleware(VanthOptions options, ISolariBuilder builder, IConfigurationSection section)
        {
            if(!options.UseExceptionHandlingMiddleware)
                return;
            builder.Services.Configure<VanthOptions>(section);
            builder.AddBuildAction(new BuildAction("Solari.Vanth (ExceptionHandlingMiddleware)")
            {
                Action = provider =>
                {
                    var marshal = provider.GetService<ISolariMarshal>();
                    if (marshal.ApplicationBuilder == null)
                        return;
                    marshal.ApplicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
                }
            });
        }
        private static void ConfigureFluentValidation(VanthOptions options, ISolariBuilder builder)
        {
            if (!options.UseFluentValidation) return;
            builder.Services.AddSingleton<IVanthValidationService, VanthValidationService>();
            builder.Services.AddSingleton<IValidatorFactory, VanthValidatorFactory>();
            builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic));
        }
    }
}
