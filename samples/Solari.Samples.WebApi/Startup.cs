using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Solari.Callisto.Connector;
using Solari.Callisto.DependencyInjection;
using Solari.Callisto.Tracer;
using Solari.Eris;
using Solari.Ganymede.DependencyInjection;
using Solari.Io;
using Solari.Oberon;
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
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMetrics();
            
            services.AddSol(Configuration)
                    .AddVanth()
                    // .AddOberon()
                    .AddGanymede(requests => requests.AddGanymedeClient<IGitHubClient, GitHubClient>("GitHub"))
                    .AddCallistoWithDefaults(callistoConfiguration => callistoConfiguration
                                                 .RegisterScopedCollection<IPersonRepository, PersonRepository, Person>("person"))
                    .AddThemis(tracing => tracing.AddCallistoTracing(),
                               health => health.AddCallistoHealthCheck());

            services.AddScoped<IPersonOperations, PersonOperations>();
            
            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "Solari Sample Api");
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<CreatePersonConsumer>();
                cfg.AddRequestClient<CreatePersonCommand>();
                cfg.AddMediator();
            });
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