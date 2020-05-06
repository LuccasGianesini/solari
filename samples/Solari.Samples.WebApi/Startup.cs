using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Callisto.Connector;
using Solari.Callisto.Connector.DependencyInjection;
using Solari.Callisto.DependencyInjection;
using Solari.Callisto.Tracer;
using Solari.Ceres.DependencyInjection;
using Solari.Deimos;
using Solari.Eris;
using Solari.Ganymede.DependencyInjection;
using Solari.Hyperion;
using Solari.Io;
using Solari.Oberon;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Infrastructure;
using Solari.Sol;

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
                    // .AddVanth()
                    .AddEris()
                    .AddOberon()
                    .AddGanymede(requests => requests.AddGanymedeClient<IGitHubClient, GitHubClient>("GitHub"))
                    .AddCeres()
                    .AddIo(a => a.AddCallistoHealthCheck()
                                 .AddPrivateMemoryHealthCheck(524288000)
                                 .AddProcessAllocatedMemoryHealthCheck(209715200))
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
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3();
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