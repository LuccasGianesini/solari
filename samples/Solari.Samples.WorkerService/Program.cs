using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Deimos;
using Solari.Ganymede.DependencyInjection;
using Solari.Oberon;
using Solari.Sol;
using Solari.Titan.DependencyInjection;

namespace Solari.Samples.WorkerService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args)
                         .UseTitan()
                         .Build();
            try
            {
                host.UseSol();
                await host.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSol(hostContext.Configuration)
                            .AddDeimos()
                            .AddOberon()
                            .AddGanymede(actions => actions.AddGanymedeClient<ITestClient, TestClient>("Solari-Samples"));
                    
                    services.AddHostedService<Worker>();
                });
    }
}