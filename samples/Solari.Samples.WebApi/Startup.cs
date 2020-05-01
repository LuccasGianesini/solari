using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Mvc;
using Solari.Callisto;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Ceres;
using Solari.Deimos;
using Solari.Eris;
using Solari.Ganymede.DependencyInjection;
using Solari.Hyperion;
using Solari.Oberon;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Infrastructure;
using Solari.Sol;
using Solari.Titan.DependencyInjection;
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

            services
                .AddHealthChecksUI(a => a.AddHealthCheckEndpoint("health", "/health"));

            services.AddSol(Configuration)
                    // .AddVanth()
                    .AddEris()
                    .AddOberon()
                    .AddGanymede(requests => requests.AddGanymedeClient<IGitHubClient, GitHubClient>("GitHub"))
                    .AddCeres()
                    .AddHyperion()
                    .AddCallistoConnector()
                    .AddCallisto(callistoConfiguration => callistoConfiguration
                                                          .RegisterDefaultConventionPack()
                                                          .RegisterDefaultClassMaps()
                                                          .RegisterCollection<IPersonRepository, PersonRepository, Person>("person", ServiceLifetime.Scoped))
                    .AddDeimos(manager => manager.Register(new CallistoTracerPlugin()));

            services.AddScoped<IPersonOperations, PersonOperations>();
            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "Solari Sample Api");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSol();
        }
    }
}