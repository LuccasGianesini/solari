using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Deimos;
using Solari.Oberon;
using Solari.Samples.Domain.Person;
using Solari.Samples.Infrastructure;
using Solari.Sol;
using Solari.Titan.DependencyInjection;
using Solari.Vanth.DependencyInjection;

namespace Solari.Samples.Di
{
    public static class ContainerConfiguration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
         



            return services;
        }
    }
}