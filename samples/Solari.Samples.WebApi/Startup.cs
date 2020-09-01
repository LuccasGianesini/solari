using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Solari.Callisto.Connector;
using Solari.Callisto.DependencyInjection;
using Solari.Callisto.Identity;
using Solari.Callisto.Tracer;
using Solari.Eris;
using Solari.Ganymede.DependencyInjection;
using Solari.Io;
using Solari.Oberon;
using Solari.Callisto.Integrations.MassTransit;
using Solari.Samples.Application.Person;
using Solari.Samples.Application.Person.Consumers;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Commands;
using Solari.Samples.Infrastructure;
using Solari.Sol;
using Solari.Themis;
using Solari.Vanth.DependencyInjection;

namespace Solari.Samples.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSol(Configuration)
                    .AddVanth()
                    .AddGanymede(policy => policy.RetryPolicy(5, 5),
                                 a => a.AddGanymedeClientWithRetryPolicy<IKeycloakClient, KeycloakClient>("Keycloak"))
                    .AddCallistoWithDefaults(configurator =>
                                                 configurator.RegisterClient("localhost", collection => collection
                                                                                 .ConfigureScopedCollection<
                                                                                     PersonCollection,
                                                                                     PersonCollection,
                                                                                     Person>("solari-samples", "person"))
                                           , ServiceLifetime.Scoped)
                    .AddCallistoIdentityProvider<PersonId>("localhost-identity", "solari-samples");


            services.AddScoped<IPersonOperations, PersonOperations>();

            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "Solari Sample Api");
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            // services.AddMediator(mediator =>
            // {
            //     mediator.SetKebabCaseEndpointNameFormatter();
            //     mediator.AddConsumer<CreatePersonConsumer>();
            //     mediator.AddRequestClient<CreatePersonCommand>();
            //     mediator.AddSagaStateMachine<PersonStateMachine, PersonState>()
            //             .CallistoSagaRepository("localhost", "callisto-sagas", Configuration);
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3();
            // app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapIo();
                endpoints.MapControllers();
            });
            app.UseSol();
        }
    }
}
