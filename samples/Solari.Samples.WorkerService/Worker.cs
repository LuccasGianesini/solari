using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using Solari.Deimos.Abstractions;
using Solari.Titan;

namespace Solari.Samples.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ITestClient _client;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, ITestClient client)
        {
            _logger = logger;
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            long i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
            
                i++;
            }
        }
    }
}