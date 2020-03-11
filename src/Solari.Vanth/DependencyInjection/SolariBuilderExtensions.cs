
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol;
using Solari.Vanth.Builders;

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
            return builder;
        }
    }
}