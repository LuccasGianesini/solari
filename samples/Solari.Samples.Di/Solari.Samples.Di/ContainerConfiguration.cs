using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto;
using Solari.Callisto.Connector;
using Solari.Deimos.Elastic;
using Solari.Samples.Application;
using Solari.Samples.Domain.Person;
using Solari.Samples.Infrastructure;
using Solari.Sol;
using Solari.Vanth.DependencyInjection;

namespace Solari.Samples.Di
{
    public static class ContainerConfiguration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSol(configuration)
                    .AddVanth()
                    .AddCallistoConnector()
                    .AddCallisto(callistoConfiguration => callistoConfiguration
                                                          .RegisterDefaultConventionPack()
                                                          .RegisterDefaultClassMaps()
                                                          .RegisterCollection<IPersonRepository, PersonRepository, Person>("person", ServiceLifetime.Scoped));
            services.AddScoped<IPersonOperations, PersonOperations>();
            services.AddScoped<IPersonApplication, PersonApplication>();
            
            return services;
        }
    }
}