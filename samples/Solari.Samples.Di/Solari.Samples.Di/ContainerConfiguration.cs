using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Deimos;
using Solari.Deimos.Elastic;
using Solari.Samples.Application;
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
            services.AddSol(configuration)
                    .AddVanth()
                    .AddTitan()
                    .AddCallistoConnector()
                    .AddCallisto(callistoConfiguration => callistoConfiguration
                                                          .RegisterDefaultConventionPack()
                                                          .RegisterDefaultClassMaps()
                                                          .RegisterCollection<IPersonRepository, PersonRepository, Person>("person", ServiceLifetime.Scoped))
                    .AddDeimos(manager => manager.Register(new CallistoTracerPlugin()));
            services.AddScoped<IPersonOperations, PersonOperations>();
            services.AddScoped<IPersonApplication, PersonApplication>();
            
            return services;
        }
    }
}