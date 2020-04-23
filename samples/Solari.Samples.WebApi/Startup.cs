using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Solari.Callisto;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Deimos;
using Solari.Eris;
using Solari.Ganymede.DependencyInjection;
using Solari.Miranda.DependencyInjection;
using Solari.Oberon;
using Solari.Samples.Application;
using Solari.Samples.Di;
using Solari.Samples.Domain;
using Solari.Samples.Domain.Person;
using Solari.Samples.Domain.Person.Validators;
using Solari.Samples.Infrastructure;
using Solari.Sol;
using Solari.Titan.DependencyInjection;
using Solari.Vanth.DependencyInjection;
using Solari.Vanth.Validation;

namespace Solari.Samples.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSol(Configuration)
                    .AddVanth()
                    .AddTitan()
                    .AddEris()
                    .AddMiranda()
                    .AddOberon() 
                    .AddGanymede(requests => requests.AddGanymedeClient<IGitHubClient, GitHubClient>("GitHub"))
                    .AddCallistoConnector()
                    .AddCallisto(callistoConfiguration => callistoConfiguration
                                                          .RegisterDefaultConventionPack()
                                                          .RegisterDefaultClassMaps()
                                                          .RegisterCollection<IPersonRepository, PersonRepository, Person>("person", ServiceLifetime.Scoped))
                    .AddDeimos(manager => manager.Register(new CallistoTracerPlugin()));

            services.AddScoped<IPersonOperations, PersonOperations>();
            services.AddTransient<IMirandaSubscriber, MirandaSubscriber>();
            // services.AddSwaggerGen(a => a.SwaggerDoc("v1", new OpenApiInfo
            // {
            //     Description = "Solari Sample WebApi;",
            //     License = new OpenApiLicense
            //     {
            //         Name = "GNU General Public License v3.0",
            //         Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html")
            //     },
            //     Title = "Solari Sample WebApi",
            //     Version = "v1",
            // }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseSol();
            // app.UseSwaggerUI(options =>
            // {
            //     options.RoutePrefix = "swagger";
            //     options.SwaggerEndpoint("./v1/swagger.json", "Solari Samples WebApi");
            // });
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}